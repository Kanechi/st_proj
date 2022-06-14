using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sfproj
{
    /// <summary>
    /// 地域開拓状態
    /// </summary>
    public enum eAreaDevelopmentState
    {
        // 未開拓
        Not,
        // 開拓中
        During,
        // 開拓済み
        Completed
    }

    /// <summary>
    /// 地域タイプ
    /// </summary>
    public enum eAreaType
    {
        None = -1,

        /// <summary>
        /// 都市
        /// 通常の町
        /// 常用的に資源を得るのが目的
        /// 領域の防衛力を上げるのも目的
        /// </summary>
        Town = 1000,

        /// <summary>
        /// 首都
        /// 通常の町とあまり変わらないが、
        /// 首都のある領域が攻め込まれたらゲームオーバーとなるので、
        /// 首都のある領域の地域群は防衛力を大きく上げておく必要がある
        /// </summary>
        Capital = 1100,

        /// <summary>
        /// 要塞
        /// 防衛力に特化した地域
        /// 資源収入はほぼないが要塞が一つあるだけで、その領域の防衛力がかなり上がる
        /// </summary>
        StrongHold = 1200,

        /// <summary>
        /// レメゲトンポリス
        /// 魔導を極めた町
        /// 特殊な条件でレメゲトンポリスに変化させることが可能
        /// 資源収入が格段にあがり、防衛力もかなりあがる
        /// </summary>
        LemegetonPoris = 1300,



        /// <summary>
        /// 遺跡
        /// 調査を進めることで強力な領域バフを得られる
        /// 調査は何段階か存在しており、成功するたびに
        /// 期間、または永続的な領域バフを得られる
        /// </summary>
        Remains = 2000,

        /// <summary>
        /// 洞窟
        /// 探索を進めることで資源やアイテムを得られる
        /// 探索は何度も行う事が可能、
        /// 探索には将軍１人と兵士が必要
        /// 探索に成功すると熟練度が上がり、資源やアイテムを得られる
        /// </summary>
        Cave = 3000,
    }

    /// <summary>
    /// 区域タイプ
    /// </summary>
    public enum eZoneType
    {
        None = -1,

        /// <summary>
        /// 農業区
        /// 平地に隣接していれば建設可能
        /// 食料資源が増加
        /// </summary>
        Agriculture = 1000,

        /// <summary>
        /// 商業区
        /// 資金が増加
        /// さまざまなアイテムをトレード可能になる
        /// 町の特産品なども増える
        /// </summary>
        Commercial = 2000,

        /// <summary>
        /// 鉱業区
        /// 山に隣接していれば建設可能
        /// 鉱物資源が増える
        /// </summary>
        MiningIndustry = 3000,

        /// <summary>
        /// 伐採地
        /// 森に隣接していれば建設可能
        /// 材木資源が増加
        /// </summary>
        LoggingArea = 4000,

        /// <summary>
        /// 魔導区
        /// 魔力資源が増加
        /// 魔導系のアイテムクラフトを生成可能になる
        /// 魔法防御力をを上げる事も可能
        /// </summary>
        Witchcrafty = 5000,

        /// <summary>
        /// 城塞区
        /// 町の一区画の防御力を上げることが可能
        /// 壁とは違う
        /// 壁はまた別途城壁として建設可能
        /// </summary>
        Citadel = 6000,
    }

    /// <summary>
    /// 地域レコード
    /// 保存情報
    /// 2022/06/08
    ///     開拓システムあり
    ///     ゲーム開始時に一度だけ生成して領域レコードに紐づけ
    ///     統治後1つ目のセルに対してまずは開拓を行う
    ///     開拓と調査と探索は別のも
    ///     
    ///     「未開拓」の地域に対して行う事で「開拓済み」となり、都市を建設できるようになる。
    ///     開拓した際に、まれに遺跡か洞窟が発見される。
    ///     遺跡か洞窟になった地域は破壊することはできない
    ///     遺跡の場合は調査することでバフを得られる
    ///     洞窟の場合は探索することで資源やアイテムを得られる
    ///     
    /// 2022/06/14
    ///     ES3 で保存するなら内部で詳細データにすると保管できないな・・・
    ///     OdinInspector でリストかして表示する際はクラスを一つかます方が見やすいから Serialized する際は何か考えないとかな
    /// 
    /// </summary>
    [Serializable]
    public class SfAreaRecord
    {
        // 地域 ID (ユニーク ID)
        public uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // 地域名
        public string m_name = "";
        public string Name { get => m_name; set => m_name = value; }

        // 属している領域 ID
        public uint m_dominionId = 0;
        public uint DominionId { get => m_dominionId; set => m_dominionId = value; }

        // 地域インデックス(スクロールのセル番号)
        public int m_areaIndex = -1;
        public int AreaIndex { get => m_areaIndex; set => m_areaIndex = value; }

        // 地域開拓状態
        public eAreaDevelopmentState m_areaDevelopmentState = eAreaDevelopmentState.Not;
        public eAreaDevelopmentState AreaDevelopmentState { get => m_areaDevelopmentState; set => m_areaDevelopmentState = value; }

        // 地域タイプ
        public eAreaType m_areaType = eAreaType.None;
        public eAreaType AreaType { get => m_areaType; set => m_areaType = value; }


        // 最大区域数(設定可能な区域の最大数)
        public int m_maxZoneCount = -1;
        public int MaxZoneCount { get => m_maxZoneCount; set => m_maxZoneCount = value; }

        // 設定されている区域タイプリスト
        public List<eZoneType> m_zoneTypeList = new List<eZoneType>();
        public List<eZoneType> ZoneTypeList { get => m_zoneTypeList; set => m_zoneTypeList = value; }
    }

#if false
    /// <summary>
    /// 地域
    /// これは地域セルである領域スクロールセルになるから必要ない？
    /// </summary>
    public class SfArea : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
#endif
    /// <summary>
    /// 地域生成 工場 基底
    /// 2022/0608
    ///     都市と遺跡と洞窟で分けるべき？
    ///     最大区域数 = 最大調査数？
    /// </summary>
    public abstract class SfAreaFactoryBase
    {
        public SfAreaRecord Create(uint uniqueId, int areaIndex, uint dominionId)
        {
            var record = CreateAreaRecord();

            // 地域 ID の設定
            record.Id = uniqueId;

            // 地域名を設定
            record.Name = CreateRandomAreaName();

            // 領域 ID を設定
            record.DominionId = dominionId;

            // 地域インデックスの設定
            record.AreaIndex = areaIndex;

            // 地域タイプの設定

            // 最大区域数の設定

            return record;
        }

        /// <summary>
        /// 地域レコードを生成
        /// </summary>
        /// <returns></returns>
        protected abstract SfAreaRecord CreateAreaRecord();

        /// <summary>
        /// 地域名をランダムに生成
        /// </summary>
        /// <returns></returns>
        protected abstract string CreateRandomAreaName();

        /// <summary>
        /// 地域タイプを設定 (町、遺跡、洞窟)
        /// </summary>
        /// <returns></returns>
        protected abstract eAreaType SettingAreaType();

        protected abstract int SettingMaxZoneCount();
    }

    /// <summary>
    /// 地域レコード生成 工場
    /// </summary>
    public abstract class SfAreaCreateFactory : SfAreaFactoryBase
    {
        protected override SfAreaRecord CreateAreaRecord()
        {
            return new SfAreaRecord();
        }
    }

    /// <summary>
    /// 地域 町 生成 工場
    /// </summary>
    public class SfAreaFactoryTown : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test"; }

        protected override eAreaType SettingAreaType() { return eAreaType.Town; }

        protected override int SettingMaxZoneCount() { return UnityEngine.Random.Range(ConfigController.Instance.MinAreaValue, ConfigController.Instance.MaxAreaValue + 1); }
    }

    /// <summary>
    /// 地域 遺跡 生成 工場
    /// </summary>
    public class SfAreaFactoryRemains : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test"; }

        protected override eAreaType SettingAreaType() { return eAreaType.Remains; }

        protected override int SettingMaxZoneCount() { return UnityEngine.Random.Range(ConfigController.Instance.MinAreaValue, ConfigController.Instance.MaxAreaValue + 1); }
    }

    /// <summary>
    /// 地域 洞窟 生成 工場
    /// </summary>
    public class SfAreaFactoryCave : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test"; }

        protected override eAreaType SettingAreaType() { return eAreaType.Cave; }

        protected override int SettingMaxZoneCount() { return -1; }
    }

    /// <summary>
    /// 地域 生成 工場 管理
    /// </summary>
    public class SfAreaFactoryManager : Singleton<SfAreaFactoryManager>
    {
        private List<SfAreaFactoryBase> factoryList = null;

        public SfAreaFactoryManager() {

            factoryList = new List<SfAreaFactoryBase>()
            {
                 new SfAreaFactoryCave(),
                 new SfAreaFactoryRemains(),
                 new SfAreaFactoryTown()
            };
        }

        /// <summary>
        /// 地域の作成
        /// </summary>
        /// <param name="areaIndex">地域インデックス(セル番号)</param>
        /// <param name="dominionId">属している領域 ID</param>
        /// <returns></returns>
        public SfAreaRecord Create(int areaIndex, uint dominionId) 
        {
            // 町か遺跡か洞窟かをランダム
            // 割合は設定できるようにする
            float rate = UnityEngine.Random.value * 100.0f;

            int factoryType = -1;

            if (100.0f > rate && (ConfigController.Instance.AreaTownRate + ConfigController.Instance.AreaRemainsRate) <= rate)
            {
                // 洞窟
                factoryType = 0;
            }
            else if ((ConfigController.Instance.AreaTownRate + ConfigController.Instance.AreaRemainsRate) > rate &&
                (ConfigController.Instance.AreaTownRate) <= rate)
            {
                // 遺跡
                factoryType = 1;
            }
            else if (ConfigController.Instance.AreaTownRate < rate)
            {
                // 町
                factoryType = 2;
            }

            if (factoryType == -1)
            {
                Debug.LogError("factoryType == -1 !!!");
                return null;
            }

            // ユニーク ID の作成
            uint uniqueId = SfConstant.CreateUniqueId(ref SfAreaRecordTableManager.Instance.m_uniqueIdList);

            // 地域レコードを作成
            var record = factoryList[factoryType].Create(uniqueId, areaIndex, dominionId);

            return record;
        }
    }

    /// <summary>
    /// 地域 管理
    /// プレイ中に生成されているすべての SfAreaRecord
    /// 保存時は別ファイルの別クラスに実装
    /// </summary>
    public class SfAreaRecordTable : RecordTable<SfAreaRecord>
    {
        // ユニーク ID リスト
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        // 登録
        public void Regist(SfAreaRecord record) => RecordList.Add(record);

        // 地域レコードの取得
        public override SfAreaRecord Get(uint id) => RecordList.Find(r => r.Id == id);
    }

    /// <summary>
    /// 地域レコードテーブル管理
    /// </summary>
    public class SfAreaRecordTableManager : SfAreaRecordTable
    {
        private static SfAreaRecordTableManager s_instance = null;

        public static SfAreaRecordTableManager Instance {

            get
            {
                if (s_instance != null)
                    return s_instance;

                s_instance = new SfAreaRecordTableManager();

                s_instance.Load();

                return s_instance;
            }
        }

        /// <summary>
        /// 読み込み処理
        /// </summary>
        public void Load() {

            var builder = new ESLoadBuilder<SfAreaRecord, SfAreaRecordTable>("SfAreaRecordTable");

            var director = new RecordTableESDirector<SfAreaRecord>(builder);

            director.Construct();

            if (director.GetResult() != null && director.GetResult().RecordList.Count > 0)
            {
                m_recordList.AddRange(director.GetResult().RecordList);
            }
        }

        /// <summary>
        /// 保存処理
        /// </summary>
        public void Save() {

            var builder = new ESSaveBuilder<SfAreaRecord>("SfAreaRecordTable", this);

            var director = new RecordTableESDirector<SfAreaRecord>(builder);

            director.Construct();
        }
    }
}