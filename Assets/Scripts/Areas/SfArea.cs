#define DEBUG_COSTCHECK

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using System.Linq;

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

        // 通常
        [EnumString("area_town_bg_image")]
        Normal,

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
    public class SfArea : IJsonParser
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

        // 地形 (地域に存在する地形)
        public eExistingTerrain m_existingTerrain = 0;
        public eExistingTerrain ExistingTerrain { get => m_existingTerrain; set => m_existingTerrain = value; }

        // 地域人口
        public int m_population = 0;
        public int Population { get => m_population; set => m_population = value; }

        // 最大区域数(設定可能な区域の最大数)
        // 区域解放数は現状の地域人口に比例
        public int m_maxZoneCount = -1;
        public int MaxZoneCount { get => m_maxZoneCount; set => m_maxZoneCount = value; }

        public List<uint> m_productionResourceItemIdList = new List<uint>();
        public List<uint> ProductionResourceItemIdList { get => m_productionResourceItemIdList; set => m_productionResourceItemIdList = value; }

#if false
        // 保管されているアイテム ID と総数
        public class StragedProductSet : IJsonParser {

            public uint m_itemId = 0;
            public uint ItemId { get => m_itemId; set => m_itemId = value; }

            public int m_count = 0;
            public int Count { get => m_count; set => m_count = value; }

            public StragedProductSet() { }
            public StragedProductSet(uint itemId, int ct) { m_itemId = itemId; m_count = ct; }

            public void Parse(IDictionary<string, object> data)
            {

            }
        }

        public List<StragedProductSet> m_storagedProductList = new List<StragedProductSet>();
        public List<StragedProductSet> StoragedProductList => m_storagedProductList;
#endif

        // 割り当てられている武将 ID
        public List<uint> m_troopList = new List<uint>();
        public List<uint> TroopList => m_troopList;

        public void Parse(IDictionary<string, object> data)
        {
            
        }
    }


    /// <summary>
    /// 地域 管理
    /// プレイ中に生成されているすべての SfAreaData
    /// 保存時は別ファイルの別クラスに実装
    /// </summary>
    public class SfAreaTable : RecordTable<SfArea>
    {


        // 登録
        public void Regist(SfArea record) => RecordList.Add(record);

        // 地域レコードの取得
        public override SfArea Get(uint id) => RecordList.Find(r => r.Id == id);

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

        // 地形の追加
        public void AddTerrain(uint areaId, eExistingTerrain terrain) => Get(areaId).ExistingTerrain |= terrain;
        // 地形の削除
        public void RemoveTerrain(uint areaId, eExistingTerrain terrain) => Get(areaId).ExistingTerrain &= ~terrain;


#if false
        /// <summary>
        /// 生産アイテムの追加
        /// </summary>
        /// <param name="areaData"></param>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        public void AddProductItem(SfArea areaData, uint itemId, int count)
        {
            var set = areaData.StoragedProductList.Find(s => s.m_itemId == itemId);

            if (set != null)
            {
                set.m_count += count;
            }
            else
            {
                areaData.StoragedProductList.Add(new SfArea.StragedProductSet(itemId, count));
            }
        }
#endif

        /// <summary>
        /// 区域施設の建設に必要なコストのチェック
        /// </summary>
        /// <param name="areaRecord"></param>
        /// <param name="facilityRecord"></param>
        /// <returns>true...建設可能</returns>
        public bool CheckCostForBuildingFacility(SfArea areaData, SfZoneFacilityRecord facilityRecord)
        {
#if DEBUG_COSTCHECK
            // 後々保管生産リストを起動時に加算できるように実装

            return true;
#else
            // 外部で取得して引数に渡した方が良いかも
            //var areaRecord = Get(areaId);
            //var facilityRecord = SfZoneFacilityRecordTable.Instance.Get(type);

            foreach (var cost in facilityRecord.Costs)
            {
                var set = areaData.StoragedProductList.Find(s => s.m_itemId == cost.Id);

                // 必要コストの資源が生産物に１つでも無い場合は false
                if (set == null)
                {
                    return false;
                }

                // 生産物がコストより少ない場合は false
                if (set.Count < cost.Count)
                {
                    return false;
                }
            }

            return true;
#endif
        }


#if false
        /// <summary>
        /// 区域施設の建設に必要なコストを支払う
        /// 
        /// 区域施設に必要なコストは Warehouse である倉庫から
        /// 倉庫は地域とは別にクラスを作成
        /// </summary>
        /// <param name="areaRecord"></param>
        /// <param name="facilityRecord"></param>
        public void PayCostForBuildingFacility(SfArea areaData, SfZoneFacilityRecord facilityRecord)
        {
            foreach (var cost in facilityRecord.Costs)
            {
                var set = areaData.StoragedProductList.Find(s => s.m_itemId == cost.Id);
                set.Count -= cost.Count;
            }
        }
#endif
    }

    /// <summary>
    /// 地域レコードテーブル管理
    /// </summary>
    public class SfAreaTableManager : Singleton<SfAreaTableManager>
    {
        // ユニーク ID リスト
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        private SfAreaTable m_table = new SfAreaTable();
        public SfAreaTable Table => m_table;

        /// <summary>
        /// 読み込み処理
        /// </summary>
        public void Load() {
            var director = new RecordTableESDirector<SfArea>(new ESLoadBuilder<SfArea, SfAreaTable>("SfAreaDataTable"));
            director.Construct();
            if (director.GetResult() != null && director.GetResult().RecordList.Count > 0)
                m_table.RecordList.AddRange(director.GetResult().RecordList);
        }

        /// <summary>
        /// 保存処理
        /// </summary>
        public void Save() {
            var director = new RecordTableESDirector<SfArea>(new ESSaveBuilder<SfArea>("SfAreaDataTable", m_table));
            director.Construct();
        }
    }
}