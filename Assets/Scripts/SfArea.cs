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
    /// 地域に存在する地形
    /// 地形によって区域に設置できるものやトレードで販売しているものなどいろいろなものが変化する
    /// 海のみ領域が海に面しているかを判定して設定する
    /// </summary>
    public enum eExistingTerrain : uint
    {
        // 平原
        Plane = 1u << 0,
        // 森
        Forest = 1u << 1,
        // 山
        Mountain = 1u << 2,
        // 川
        River = 1u << 3,

        // 海(隣接するテラインに非表示が存在するなら海に隣接している)
        Ocean = 1u << 4,
    }

    /// <summary>
    /// 地域のタイプ
    /// 町系は 1000 番台
    /// 遺跡系は 2000 番台
    /// 洞窟系は 3000 番台
    /// </summary>
    public enum eAreaType
    {
        [EnumString("")]
        None = -1,

        /// <summary>
        /// 都市
        /// 通常の町
        /// 常用的に資源を得るのが目的
        /// 領域の防衛力を上げるのも目的
        /// </summary>
        [EnumString("debug_town")]
        Town = 1000,

        /// <summary>
        /// 首都
        /// 通常の町とあまり変わらないが、
        /// 首都のある領域が攻め込まれたらゲームオーバーとなるので、
        /// 首都のある領域の地域群は防衛力を大きく上げておく必要がある
        /// </summary>
        [EnumString("debug_castle")]
        Capital = 1100,

        /// <summary>
        /// 要塞
        /// 防衛力に特化した地域
        /// 資源収入はほぼないが要塞が一つあるだけで、その領域の防衛力がかなり上がる
        /// </summary>
        [EnumString("debug_twin_town")]
        StrongHold = 1200,

        /// <summary>
        /// レメゲトンポリス
        /// 魔導を極めた町
        /// 特殊な条件でレメゲトンポリスに変化させることが可能
        /// 資源収入が格段にあがり、防衛力もかなりあがる
        /// </summary>
        [EnumString("debug_castle")]
        LemegetonPoris = 1300,



        /// <summary>
        /// 遺跡
        /// 調査を進めることで強力な領域バフを得られる
        /// 調査は何段階か存在しており、成功するたびに
        /// 期間、または永続的な領域バフを得られる
        /// </summary>
        [EnumString("debug_remain")]
        Remains = 2000,

        /// <summary>
        /// 洞窟
        /// 探索を進めることで資源やアイテムを得られる
        /// 探索は何度も行う事が可能、
        /// 探索には将軍１人と兵士が必要
        /// 探索に成功すると熟練度が上がり、資源やアイテムを得られる
        /// </summary>
        [EnumString("debug_cave")]
        Cave = 3000,
    }

    static class eAreaTypeExtention
    {
        static readonly private Dictionary<eAreaType, string> s_dic_ = new Dictionary<eAreaType, string>();
        static eAreaTypeExtention() => EnumStringUtility.ForeachEnumAttribute<eAreaType, EnumStringAttribute>((e, attr) => { s_dic_.Add(e, attr.Value); });
        static public string ToEnumString(this eAreaType e) => s_dic_[e];
    }

    /// <summary>
    /// 地域種のタイプ
    /// 平地、遺跡、洞窟の3タイプ
    /// </summary>
    public enum eAreaGroupType {

        [EnumString("")]
        None = -1,

        // 平地
        [EnumString("area_town_bg_image")]
        Plane,

        // 遺跡
        [EnumString("area_remain_bg_image")]
        Remains,

        // 洞窟
        [EnumString("area_cave_bg_image")]
        Cave
    }

    static class eAreaGroupTypeExtention
    {
        static readonly private Dictionary<eAreaGroupType, string> s_dic_ = new Dictionary<eAreaGroupType, string>();
        static eAreaGroupTypeExtention() => EnumStringUtility.ForeachEnumAttribute<eAreaGroupType, EnumStringAttribute>((e, attr) => { s_dic_.Add(e, attr.Value); });
        static public string ToEnumString(this eAreaGroupType e) => s_dic_[e];
    }


    /// <summary>
    /// 区域タイプ
    /// stellaris は電気、工業、農業、産業区域を別枠で振り分けれるようになっているが
    /// それをすべて専門枠として扱うような感じ
    /// 利用できる土地があってそこに区域を振り分けるという考えかた
    /// </summary>
    public enum eZoneType
    {
        /// <summary>
        /// 何も設定されていない
        /// 破壊した際もこれに設定
        /// </summary>
        None = -1,

        /// <summary>
        /// 生産区_田畑
        /// 平地に隣接していれば建設可能
        /// 食料資源が増加
        /// </summary>
        Production_Fields = 1000,
        Production_Fishery = 1001,

        /// <summary>
        /// 生産区_採掘所
        /// 山に隣接していれば建設可能
        /// 鉱物資源が増える
        /// </summary>
        Production_Mining = 1010,

        /// <summary>
        /// 生産区_伐採所
        /// 森に隣接していれば建設可能
        /// 材木資源が増加
        /// </summary>
        Production_LoggingArea = 1020,

        /// <summary>
        /// 商業区
        /// 資金が増加
        /// さまざまなアイテムをトレード可能になる
        /// 町の特産品なども増える
        /// </summary>
        Commercial_MarketPlace  = 2000,
        Commercial_TradingPost  = 2010,
        // 港：海に面していないと設置不可
        Commercial_Harbor       = 2020,



        /// <summary>
        /// 魔導区
        /// 魔力資源が増加
        /// 魔導系のアイテムクラフトを生成可能
        /// 魔法防御力をを上げる事も可能
        /// </summary>
        Witchcrafty_MagicItemWorkshop = 3000,
        Witchcrafty_RuneEngravedStone = 3010,

        /// <summary>
        /// 城塞区
        /// 町の一区画の防御力を上げることが可能
        /// 壁とは違う
        /// 壁はまた別途城壁として建設可能
        /// </summary>
        Citadel = 4000,
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

        /// <summary>
        /// true...拠点
        /// 拠点が戦争時に攻め込まれた際の防衛の値に直結する
        /// 拠点にした町のバフが戦争時に大きく影響
        /// 拠点にすることでその町特有のバフがいろいろと影響する
        /// </summary>
        public bool m_baseFlag = false;
        public bool BaseFlag { get => m_baseFlag; set => m_baseFlag = value; }

        // 地域開拓状態
        public eAreaDevelopmentState m_areaDevelopmentState = eAreaDevelopmentState.Not;
        public eAreaDevelopmentState AreaDevelopmentState { get => m_areaDevelopmentState; set => m_areaDevelopmentState = value; }

        // 地域種タイプ
        public eAreaGroupType m_areaGroupType = eAreaGroupType.None;
        public eAreaGroupType AreaGroupType { get => m_areaGroupType; set => m_areaGroupType = value; }

        // 地域タイプ
        public eAreaType m_areaType = eAreaType.None;
        public eAreaType AreaType { get => m_areaType; set => m_areaType = value; }

        // 地域に存在する地形
        public eExistingTerrain m_existingTerrain = 0;
        public eExistingTerrain ExistingTerrain { get => m_existingTerrain; set => m_existingTerrain = value; }

        // 地域人口
        public int m_population = 0;
        public int Puplation { get => m_population; set => m_population = value; }

        // 最大区域数(設定可能な区域の最大数)
        // 区域解放数は現状の地域人口に比例
        public int m_maxZoneCount = -1;
        public int MaxZoneCount { get => m_maxZoneCount; set => m_maxZoneCount = value; }

        // 設定されている区域タイプリスト<セルインデックス、区域タイプ>
        public Dictionary<uint, eZoneType> m_zoneTypeDict = new Dictionary<uint, eZoneType>();
        public Dictionary<uint, eZoneType> ZoneTypeDict { get => m_zoneTypeDict; set => m_zoneTypeDict = value; }

        // 設定されている区域の拡張数<セルインデックス、拡張数>
        public Dictionary<uint, int> m_zoneExpantionDict = new Dictionary<uint, int>();
        public Dictionary<uint, int> ZoneExpantionDict = new Dictionary<uint, int>();
        
    }

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

            // 地域種タイプの設定
            record.AreaGroupType = SettingRandomAreaGroupType();

            // 地域タイプの設定
            record.AreaType = RandomSettingAreaType();

            // 隣接地形タイプの設定
            record.ExistingTerrain = SettingExistingTerrain(SfDominionRecordTableManager.Instance.Get(dominionId));

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

        // 地域種タイプを設定
        protected abstract eAreaGroupType SettingRandomAreaGroupType();

        // ランダムに地域タイプを設定
        protected abstract eAreaType RandomSettingAreaType();

        // 隣接地形タイプの設定
        protected abstract eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion);

        // 最大区域数の計算
        protected abstract int CulcMaxZoneCount();
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
    /// 地域 平地 生成 工場
    /// </summary>
    public class SfAreaFactoryPlane : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test_plane"; }

        // 地域種タイプを設定
        protected override eAreaGroupType SettingRandomAreaGroupType() { return eAreaGroupType.Plane; }

        // 地域タイプをランダムに設定
        protected override eAreaType RandomSettingAreaType() { return eAreaType.Town; }

        // 存在する地形の設定
        protected override eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion)
        {
            eExistingTerrain terrain = 0;

            // 平原、山、森、海、は確率分布だが、重複も可能なのでそれぞれをそれぞれだけの割合で計算


            // 平原
            float rate = UnityEngine.Random.value * 100.0f;
            if (ConfigController.Instance.DistributionRatioPlane > rate)
            {
                terrain |= eExistingTerrain.Plane;
            }

            // 山
            rate = UnityEngine.Random.value * 100.0f;
            if (ConfigController.Instance.DistributionRatioMountain > rate)
            {
                terrain |= eExistingTerrain.Mountain;
            }


            // 森
            rate = UnityEngine.Random.value * 100.0f;
            if (ConfigController.Instance.DistributionRatioForest > rate)
            {
                terrain |= eExistingTerrain.Forest;
            }

            // 川
            rate = UnityEngine.Random.value * 100.0f;
            if (ConfigController.Instance.DistributionRatioRiver > rate)
            {
                terrain |= eExistingTerrain.River;
            }

            // 海のみ領域が海に面しているかどうかをチェックしてフラグを立てる
            if (dominion.NeighboursOceanFlag == true)
            {
                rate = UnityEngine.Random.value * 100.0f;
                if (ConfigController.Instance.DistributionRatioOcean > rate)
                {
                    terrain |= eExistingTerrain.Ocean;
                }
            }

            // 何も地形が無かった場合は平原を設定
            if (terrain == 0)
                terrain |= eExistingTerrain.Plane;

            return terrain;
        }


        // 区域最大数の計算
        protected override int CulcMaxZoneCount() { return UnityEngine.Random.Range(ConfigController.Instance.MinZoneValue, ConfigController.Instance.MaxZoneValue + 1); }
    }

    /// <summary>
    /// 地域 遺跡 生成 工場
    /// </summary>
    public class SfAreaFactoryRemains : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test_reamins"; }

        // 地域種タイプを設定
        protected override eAreaGroupType SettingRandomAreaGroupType() { return eAreaGroupType.Remains; }

        // 地域タイプをランダムに設定
        protected override eAreaType RandomSettingAreaType() { return eAreaType.Remains; }

        // 存在する地形の設定 (遺跡は地形効果はきにしない)
        protected override eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion)
        {
            return 0;
        }

        protected override int CulcMaxZoneCount() { return -1; }
    }

    /// <summary>
    /// 地域 洞窟 生成 工場
    /// </summary>
    public class SfAreaFactoryCave : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test_cave"; }

        // 地域種タイプを設定
        protected override eAreaGroupType SettingRandomAreaGroupType() { return eAreaGroupType.Cave; }

        // 地域タイプをランダムに設定
        protected override eAreaType RandomSettingAreaType() { return eAreaType.Cave; }

        // 存在する地形の設定 (洞窟は地形効果はきにしない)
        protected override eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion)
        {
            return 0;
        }

        protected override int CulcMaxZoneCount() { return -1; }
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
                new SfAreaFactoryPlane(),
                new SfAreaFactoryRemains(),
                new SfAreaFactoryCave(),
            };
        }

        /// <summary>
        /// 地域の作成
        /// </summary>
        /// <param name="areaIndex">地域インデックス(セル番号)</param>
        /// <param name="dominionId">属している領域 ID</param>
        /// <returns></returns>
        public SfAreaRecord RandomCreate(int areaIndex, uint dominionId, eAreaGroupType factoryType = eAreaGroupType.None)
        {
            // 町か遺跡か洞窟かをランダム
            // 割合は設定できるようにする
            float rate = UnityEngine.Random.value * 100.0f;

            if (factoryType == eAreaGroupType.None)
            {
                // 町 (rate が 80 以下なら町)
                if (ConfigController.Instance.AreaTownRate > rate)
                {
                    // 町
                    factoryType = eAreaGroupType.Plane;
                }
                // 遺跡 (rate が 80 から 90 なら遺跡)
                else if (ConfigController.Instance.AreaTownRate <= rate && (ConfigController.Instance.AreaTownRate + ConfigController.Instance.AreaRemainsRate) > rate)
                {
                    // 遺跡
                    factoryType = eAreaGroupType.Remains;
                }
                // 洞窟 (rate が 90 から 100 なら遺跡)
                else if ((ConfigController.Instance.AreaTownRate + ConfigController.Instance.AreaRemainsRate) <= rate && (ConfigController.Instance.AreaTownRate + ConfigController.Instance.AreaRemainsRate + ConfigController.Instance.AreaCaveRate) > rate)
                {
                    // 洞窟
                    factoryType = eAreaGroupType.Cave;
                }
            }

            if (factoryType == eAreaGroupType.None)
            {
                Debug.LogError("factoryType == -1 !!!");
                return null;
            }

            // ユニーク ID の作成
            uint uniqueId = SfConstant.CreateUniqueId(ref SfAreaRecordTableManager.Instance.m_uniqueIdList);

            // 地域レコードを作成
            var record = factoryList[(int)factoryType].Create(uniqueId, areaIndex, dominionId);

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

        /// <summary>
        /// 拠点の変更
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name=""></param>
        public void ChangeBaseFlag(uint areaId, bool baseFlag) {
            Get(areaId).BaseFlag = baseFlag;
        }

        /// <summary>
        /// 開拓状態の変更
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="state"></param>
        public void ChangeDevelopmentState(uint areaId, eAreaDevelopmentState state) {
            Get(areaId).AreaDevelopmentState = state;
        }

        /// <summary>
        /// 地域の区域に指定の区域タイプを変更する
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="cellIndex"></param>
        /// <param name="zoneType"></param>
        public void ChangeZoneType(uint areaId, int cellIndex, eZoneType zoneType) {

            var record = Get(areaId);

            if (cellIndex >= record.MaxZoneCount)
            {
                Debug.LogWarning("index >= record.MaxZoneCount !!!");
                return;
            }

            record.ZoneTypeDict.Add((uint)cellIndex, zoneType);
        }

        /// <summary>
        /// 区域の拡張
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="cellIndex"></param>
        public void ExpantionZone(uint areaId, int cellIndex) 
        {
            var record = Get(areaId);

            if (cellIndex >= record.MaxZoneCount)
            {
                Debug.LogWarning("index >= record.MaxZoneCount !!!");
                return;
            }

            int expCt = record.ZoneExpantionDict[(uint)cellIndex];
            expCt++;
            record.ZoneExpantionDict[(uint)cellIndex] = expCt;
        }
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