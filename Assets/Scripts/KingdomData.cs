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
        Citadel     = 6000,
    }

    /// <summary>
    /// 区域データ
    /// 地域に設定する情報
    /// この情報を細かく設定することでその地域から得られる資源が変化する
    /// </summary>
    public class ZoneData
    {
        private uint m_id = 0;

        private eZoneType m_zoneType = eZoneType.None;

        // ゾーンタイプの設定
        public void SetZone(eZoneType zoneType) => m_zoneType = zoneType;
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

        // true...レメゲトンポリス
        private bool m_lemegetonPorisFlag = false;
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

        // true...平地に隣接
        private bool m_adjacentPlaneFlag = false;

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