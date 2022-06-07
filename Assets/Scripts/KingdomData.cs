using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
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

        // 都市
        Town            = 1000,

        // 首都
        Capital         = 1100,

        // 要塞
        StrongHold      = 1200,

        // レメゲトンポリス
        LemegetonPoris  = 1300,


        // 遺跡
        Remains         = 2000,

        // 洞窟
        Cave            = 3000,
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
        public uint m_id = 0;

        // 王国領地リスト
        public List<DominionData> m_kingdomDominionDataList = new List<DominionData>();
    }

    public class KingdomDataManager : Singleton<KingdomDataManager>
    {
        private List<KingdomData> m_kingdomDataList = new List<KingdomData>();
        public List<KingdomData> KingdomDataList => m_kingdomDataList;

    }

    /// <summary>
    /// 王国データ工場
    /// </summary>
    public class KingdomDataFactory {

        public KingdomData Create(int territoryIndex, Color color) {

            var kingdomData = new KingdomData();

            // 名前の設定

            // 

            return kingdomData;
        }
    }
}