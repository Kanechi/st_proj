using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{
    /// <summary>
    /// 区域施設
    /// 区域施設のみを管理
    /// 区域に存在する追加ボタンやロックボタンなどは管理していない
    /// </summary>
    public class SfZoneFacility : IJsonParser
    {
        // ユニーク ID
        public uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // 地域 ID
        public uint m_areaId = 0;
        public uint AreaId { get => m_areaId; set => m_areaId = value; }

        // 区域セルインデックス
        public int m_cellIndex = -1;
        public int CellIndex { get => m_cellIndex; set => m_cellIndex = value; }

        public uint m_facilityTypeId = 0;
        public uint FacilityTypeId { get => m_facilityTypeId; }

        // 区域施設のタイプ
        public eZoneFacilityCategory m_facilityCategory = eZoneFacilityCategory.None;
        public eZoneFacilityCategory FacilityCategory { get => m_facilityCategory; }

        // 拡張数
        public int m_expantionCount = 0;
        public int ExpantionCount { get => m_expantionCount; set => m_expantionCount = value; }

        public void Parse(IDictionary<string, object> data)
        {
            data.Get(nameof(m_id), out m_id);
            data.Get(nameof(m_areaId), out m_areaId);
            data.Get(nameof(m_cellIndex), out m_cellIndex);
            data.Get(nameof(m_facilityTypeId), out m_facilityTypeId);
            data.GetEnum(nameof(m_facilityCategory), out m_facilityCategory);
            data.Get(nameof(m_expantionCount), out m_expantionCount);


            // 施設の機能をここでタイプを調べて生成する
            SetFacilityData(m_facilityTypeId, m_facilityCategory);
        }

        // 施設の機能
        public SfZoneFacilityAbility Ability { get; private set; } = null;
        public SfZoneFacilityAbility NextAbility { get; private set; } = null;

        /// <summary>
        /// 施設データの設定
        /// 設定と同時に施設機能も設定
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        public void SetFacilityData(uint id, eZoneFacilityCategory category)
        {
            m_facilityTypeId = id;
            m_facilityCategory = category;

            NextAbility = SfZoneFacilityActiveAbilityTableManager.Instance.Table.Get(m_facilityTypeId, m_facilityCategory);

            if (NextAbility == null)
            {
                // 施設を破壊した際にここに入ってくるのでエラーではない
                // ID があるにも関わらずここに入ってきたらエラー
                if (m_facilityTypeId != 0)
                {
                    Debug.LogWarning("id != 0. Ability == null !!!");
                }
                else
                {
                    // 破壊処理
                    Ability = null;
                }
            }
        }

        public void Update()
        {
            if (NextAbility != null)
            {
                Ability?.OnRemove(this);

                Ability = NextAbility;

                NextAbility = null;

                Ability?.OnSetup(this);
            }

            Ability?.OnUpdate(0.0f, this);
        }
    }

    /// <summary>
    /// 区域テーブル
    /// 施設が立ったらここに取り付ける
    /// 取り付けられている施設は定期的に資源を生産する
    /// </summary>
    public class SfZoneFacilityTable : RecordTable<SfZoneFacility>
    {
        // 登録
        public void Regist(SfZoneFacility record) => RecordList.Add(record);

        // 区域施設レコードの取得
        public override SfZoneFacility Get(uint id) => RecordList.Find(r => r.Id == id);
        public SfZoneFacility Get(uint areaId, int cellIndex) => RecordList.Find(r => r.AreaId == areaId && r.CellIndex == cellIndex);

        // 区域施設の取り外し
        public void Remove(SfZoneFacility record) => RecordList.Remove(record);
        public void Remove(uint areaId, int cellIndex) => Remove(Get(areaId, cellIndex));


        /// <summary>
        /// 区域施設の初回建設
        /// 
        /// </summary>
        /// <param name="areaId">地域 ID</param>
        /// <param name="cellIndex">区域セルインデックス番号</param>
        /// <param name="type">建設する区域施設タイプ</param>
        /// <param name="exp">拡張数</param>
        public void BuildZoneFacilityType(uint areaId, int cellIndex, uint typeId, eZoneFacilityCategory category, int exp)
        {
            var zoneFacility = new SfZoneFacility();
            zoneFacility.Id = SfConstant.CreateUniqueId(ref SfZoneFacilityTableManager.Instance.m_uniqueIdList);
            zoneFacility.AreaId = areaId;
            zoneFacility.CellIndex = cellIndex;
            zoneFacility.SetFacilityData(typeId, category);
            zoneFacility.ExpantionCount = exp;

            Regist(zoneFacility);
        }

        /// <summary>
        /// 区域施設の変更
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="cellIndex"></param>
        /// <param name="type"></param>
        public void ChangeZoneFacilityType(uint areaId, int cellIndex, uint typeId, eZoneFacilityCategory category)
        {
            var facility = Get(areaId, cellIndex);
            facility.SetFacilityData(typeId, category);
        }

        /// <summary>
        /// 区域施設を破棄
        /// None に戻す
        /// 画面上ではプラスボタンに変わる
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="cellIndex"></param>
        public void DestroyZonefacilityType(uint areaId, int cellIndex)
        {
            Get(areaId, cellIndex).SetFacilityData(0, eZoneFacilityCategory.None);
        }

        /// <summary>
        /// 区域施設の拡張数を上げる
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="cellIndex"></param>
        /// <param name="exp"></param>
        public void IncreaseZoneFacilityExpantion(uint areaId, int cellIndex, int exp)
        {
            var zoneFacility = Get(areaId, cellIndex);
            if (zoneFacility.ExpantionCount >= SfConfigController.ZONE_MAX_EXPANTION_COUNT)
                return;
            zoneFacility.ExpantionCount += exp;
        }

        /// <summary>
        /// 区域施設の拡張数の設定
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="cellIndex"></param>
        /// <param name="exp"></param>
        public void SetZoneFacilityExpantion(uint areaId, int cellIndex, int exp)
        {
            var zoneFacility = Get(areaId, cellIndex);
            if (zoneFacility.ExpantionCount >= SfConfigController.ZONE_MAX_EXPANTION_COUNT)
                return;
            if (exp > SfConfigController.ZONE_MAX_EXPANTION_COUNT || exp < 0)
                return;
            zoneFacility.ExpantionCount = exp;
        }

        /// <summary>
        /// 更新処理
        /// 資源増加の施設が取り付けられていたらここで資源の増加を行う
        /// </summary>
        public void Update() {

            // 施設ごとの処理を行う、どの地域に対して何を行うか

            // 生産資源施設であれば地域にある生産資源を増加し、施設が建設されている地域の倉庫に保管する

            // 必要なら区域インデックスに生産カウントダウンを表示する

            // 一旦 switch でやってみるか

            if (RecordList.Count == 0)
                return;

            foreach (var facility in RecordList)
            {
                if (facility.FacilityTypeId != 0)
                {

                    // 時間設定が必要
                    facility.Update();
                }
            }

        }
    }

    /// <summary>
    /// 区域テーブル管理
    /// </summary>
    public class SfZoneFacilityTableManager : Singleton<SfZoneFacilityTableManager>
    {
        // ユニーク ID リスト
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        private SfZoneFacilityTable m_table = new SfZoneFacilityTable();
        public SfZoneFacilityTable Table => m_table;

        // レコードリストをクリア
        public void Clear() => m_table.RecordList.Clear();

        // 読み込み
        public void Load()
        {
            var director = new RecordTableESDirector<SfZoneFacility>(new ESLoadBuilder<SfZoneFacility, SfZoneFacilityTable>("SfZoneDataTable"));
            director.Construct();
            if (director.GetResult() != null && director.GetResult().RecordList.Count > 0)
                m_table.RecordList.AddRange(director.GetResult().RecordList);
        }

        /// <summary>
        /// 保存処理
        /// </summary>
        public void Save()
        {
            var director = new RecordTableESDirector<SfZoneFacility>(new ESSaveBuilder<SfZoneFacility>("SfZoneDataTable", m_table));
            director.Construct();
        }
    }
}