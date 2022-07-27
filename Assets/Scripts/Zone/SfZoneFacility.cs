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

        // 区域施設のタイプ
        public eZoneFacilityType m_facilityType = eZoneFacilityType.None;
        public eZoneFacilityType FacilityType { get => m_facilityType; set => m_facilityType = value; }

        // 拡張数
        public int m_expantionCount = 0;
        public int ExpantionCount { get => m_expantionCount; set => m_expantionCount = value; }

        public void Parse(IDictionary<string, object> data)
        {
            data.Get(nameof(m_id), out m_id);
            data.Get(nameof(m_areaId), out m_areaId);
            data.Get(nameof(m_cellIndex), out m_cellIndex);
            data.GetEnum(nameof(m_facilityType), out m_facilityType);
            data.Get(nameof(m_expantionCount), out m_expantionCount);
        }
    }

    /// <summary>
    /// 区域テーブル
    /// 施設が立ったらここに取り付ける
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
        /// </summary>
        /// <param name="areaId">地域 ID</param>
        /// <param name="cellIndex">区域セルインデックス番号</param>
        /// <param name="type">建設する区域施設タイプ</param>
        /// <param name="exp">拡張数</param>
        public void BuildZoneFacilityType(uint areaId, int cellIndex, eZoneFacilityType type, int exp)
        {
            var zoneFacility = new SfZoneFacility();
            zoneFacility.Id = SfConstant.CreateUniqueId(ref SfZoneFacilityTableManager.Instance.m_uniqueIdList);
            zoneFacility.AreaId = areaId;
            zoneFacility.CellIndex = cellIndex;
            zoneFacility.FacilityType = type;
            zoneFacility.ExpantionCount = exp;

            Regist(zoneFacility);
        }

        /// <summary>
        /// 区域施設の変更
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="cellIndex"></param>
        /// <param name="type"></param>
        public void ChangeZoneFacilityType(uint areaId, int cellIndex, eZoneFacilityType type)
        {
            Get(areaId, cellIndex).FacilityType = type;
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
            Get(areaId, cellIndex).FacilityType = eZoneFacilityType.None;
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