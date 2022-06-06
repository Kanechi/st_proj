using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stproj
{
    /// <summary>
    /// ゾーンタイプ
    /// </summary>
    public enum eZoneType {
        None,
        // 城
        Castle          = 1000,
        // 領主の城
        CastleOfLord    = 1001,

        // 伐採場
        LoggingArea     = 2000,

        // 採掘所
        Quarry          = 3000,

        // ハンター小屋
        HunterHut       = 4000,

        // 漁港
        FishingPort     = 5000,
        // 貿易港
        TradePort       = 5001,

        // マーケット
        Market          = 6000,
    }

    /// <summary>
    /// 区域データ
    /// </summary>
    public class ZoneData
    {
        private uint m_id = 0;

        private eZoneType m_zoneType = eZoneType.None;
    }

    /// <summary>
    /// 地域データ
    /// </summary>
    public class AreaData
    {
        private uint m_id = 0;

        // 最大区域数
        private int m_maxZoneCount = 0;

        // 区域データリスト
        private List<ZoneData> m_zoneDataList = new List<ZoneData>();
    }

    /// <summary>
    /// 領域データ
    /// </summary>
    public class DominionData
    {
        // 地域 ID
        private uint m_id = 0;

        // 地域名
        private string m_regionName = "";

        // true...首都
        private bool m_capitalFlag = false;

        // true...森に隣接
        private bool m_adjacentForestFlag = false;

        // true...海に隣接
        private bool m_adjacentSeaFlag = false;

        // true...山に隣接
        private bool m_adjacentMountainFlag = false;

        // true...川に隣接
        private bool m_adjacentRiverFlag = false;

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

        //public 
    }
}