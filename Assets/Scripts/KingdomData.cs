using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stproj
{
    /// <summary>
    /// 区域タイプ
    /// </summary>
    public enum eZoneType {
        None = -1,

        /// <summary>
        /// 農業区
        /// 平地に隣接していれば建設可能
        /// 食料が資源が増える
        /// </summary>
        Agriculture     = 1000,

        /// <summary>
        /// 商業区
        /// さまざまなアイテムをトレード可能になる
        /// 町の特産品なども増える
        /// </summary>
        Commercial      = 2000,

        /// <summary>
        /// 鉱業区
        /// 山に隣接していれば建設可能
        /// 鉱物資源が増える
        /// </summary>
        MiningIndustry  = 3000,

        /// <summary>
        /// 伐採地
        /// 森に隣接していれば建設可能
        /// 材木資源が増える
        /// </summary>
        LoggingArea     = 4000,

        /// <summary>
        /// 魔導区
        /// 魔導系のアイテムクラフトを生成可能になる
        /// 魔法防御力をを上げる事も可能
        /// </summary>
        Witchcrafty     = 5000,

        /// <summary>
        /// 城塞区
        /// 町の一区画の防御力を上げることが可能
        /// 壁とは違う
        /// 壁はまた別途城壁として建設可能
        /// </summary>
        Citadel         = 6000,
    }

    /// <summary>
    /// 地域タイプ
    /// </summary>
    public enum eAreaType
    { 
        None = -1,

        // 町
        Town,

        // 城
        Castle,

        // 要塞
        StrongHold,

        // レメゲトンポリス
        LemegetonPoris,
    }

    /// <summary>
    /// 地域データ
    /// </summary>
    public class AreaData
    {
        private uint m_id = 0;

        // 最大区域数
        private int m_maxZoneCount = 0;

        // 地域タイプ
        private eAreaType m_areaType = eAreaType.None;

        // 区域タイプリスト
        private List<eZoneType> m_zoneTypeList = new List<eZoneType>();
    }

    public enum eAdjacentType : uint {
        // 平地に隣接
        Plane       = 1u << 0,
        // 森に隣接
        Forest      = 1u << 1,
        // 海に隣接
        Ocean       = 1u << 2,
        // 山に隣接
        Mountain    = 1u << 3,
        // 川に隣接
        River       = 1u << 4,
    }

    /// <summary>
    /// 領域データ
    /// tgs の領域をタッチした際の情報
    /// </summary>
    public class DominionData
    {
        // 領域 ID
        private uint m_id = 0;

        // 領域名
        private string m_regionName = "";

        // true...探索済み  false...未探索
        private bool m_exploredFlag = false;

        // true...統治済み  false...未統治
        private bool m_ruleFlag = false;

        // true...首都(本拠地、城)
        private bool m_capitalFlag = false;

        // true...隣接
        private uint m_adjacentFlag = 0;


        // 領地地域データリスト
        private List<AreaData> m_areaDataList = new List<AreaData>();
    }


    /// <summary>
    /// 王国1つ分のデータ
    /// </summary>
    public class KingdomData
    {
        // 王国 ID
        private uint m_id = 0;

        // 王国領地リスト
        private List<DominionData> m_kingdomDominionDataList = new List<DominionData>();
    }

    public class KingdomDataManager : Singleton<KingdomDataManager>
    {
        private List<KingdomData> m_kingdomDataList = new List<KingdomData>();
        public List<KingdomData> KingdomDataList => m_kingdomDataList;

    }
}