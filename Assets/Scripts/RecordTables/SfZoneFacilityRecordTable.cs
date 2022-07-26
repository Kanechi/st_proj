using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Sirenix.OdinInspector;

namespace sfproj
{


    /// <summary>
    /// 区域タイプ = ID
    /// stellaris は電気、工業、農業、産業区域を別枠で振り分けれるようになっているが
    /// それをすべて専門枠として扱うような感じ
    /// 利用できる土地があってそこに区域を振り分けるという考えかた
    /// </summary>
    public enum eZoneFacilityType
    {
        /// <summary>
        /// 何も設定されていない
        /// 破壊した際もこれに設定
        /// </summary>
        None = -1,

 
        /// <summary>
        /// 農園(生産)
        /// 平地に隣接していれば建設可能
        /// 定期的に食料が増加
        /// </summary>
        Production_Farm = 1000,

        /// <summary>
        /// 果樹園(生産)
        /// 森に隣接していれば建設可能
        /// 定期的に食料が増加
        /// </summary>
        Production_Orchard = 1010,

        /// <summary>
        /// 漁業(生成)
        /// 川か海に隣接していれば建設可能
        /// 定期的に食料が増加
        /// </summary>
        Production_Fishery = 1020,

        /// <summary>
        /// 採掘所(生産)
        /// 山に隣接していれば建設可能
        /// 定期的に鉱物資源が増加
        /// 増加する資源はその山に埋蔵されている鉱物によって変化
        /// 石、銅、鉄、ミスリル、オリハルコン
        /// 拡張する際にどの鉱物を重点的に採掘するかを選択できるようにする？
        /// </summary>
        Production_Mining = 1100,

        /// <summary>
        /// 伐採所(生産)
        /// 森に隣接していれば建設可能
        /// 定期的に材木資源が増加
        /// 増加する資源はその森に生えている木によって変化
        /// オーク杉、松、レッドウッド
        /// </summary>
        Production_LoggingArea = 1200,




        /// <summary>
        /// 商売(商業)
        /// どこでも建設可能
        /// マーケットボタンが利用可能になる
        /// 地域で手に入る資源を換金(即時or定期)できる
        /// </summary>
        Commercial_MarketPlace = 2000,

        /// <summary>
        /// 交易(商業、陸)
        /// 平地に隣接していれば建設可能
        /// 輸送ボタンが利用可能になる(地域で手に入る資源を自国の別の地域に輸送(即時or定期)が可能になる)
        /// トレードボタンが利用可能になる(地域で手に入る資源を隣接する他国の別の地域と物々交換(即時or定期)可能になる)
        /// 地域で手に入る資源を
        /// </summary>
        Commercial_TradingPost = 2010,

        /// <summary>
        /// 港(商業、海)
        /// 川か海に隣接していれば建設可能
        /// 輸入ボタンが利用可能になる(地域で手に入る資源を自国の別の地域に輸送(即時or定期)が可能になる)
        /// トレードボタンが利用可能になる(地域で手に入る資源を港のある他国の別の地域と物々交換(即時or定期)可能になる)
        /// </summary>
        Commercial_Harbor = 2020,



        /// <summary>
        /// 住宅(居住区)
        /// 最大人口が増加
        /// </summary>
        Residential_House = 3000,


        /// <summary>
        /// ギルド(軍事区域)
        /// 遺跡や洞窟の調査が可能な、調査ボタンが利用可能
        /// 同じ地域であれば調査が可能
        /// </summary>
        Military_Guild = 4000,

        /// <summary>
        /// 城壁
        /// 防衛時、防御力が上昇
        /// </summary>
        Military_CityWall = 4100,




        /// <summary>
        /// ルーン炉(魔導区)
        /// ルーンが刻印された石を融解してマナを得る為の炉
        /// 定期的にマナを増加
        /// </summary>
        Witchcrafty_RuneFurnace = 5000,

        /// <summary>
        /// 魔法壁(魔導区)
        /// 防衛時、魔法防御力が上昇
        /// </summary>
        Witchcrafty_MagicBarrier = 5100,
    }

    /// <summary>
    /// 施設アイテム生成カテゴリ
    /// 生産タイプは設定されているアイテム ID が生産品の ID となる
    /// 加工品タイプは設定されているアイテム ID が加工品の ID となる
    /// 加工品である場合は、定期的に時間が来たら生産品を消費して加工品が生成される
    /// 消費される生産品の数はそれぞれの加工品によって定められていて
    /// 時間が来た際に使用すべき生産品があったら加工品が生成される
    /// </summary>
    public enum eFacilityItemGenCategory
    {
        None = -1,
        Production,     // 生産
        Processed,      // 加工
    }

    /// <summary>
    /// コスト
    /// </summary>
    [Serializable]
    public class SfCost
    {
        [SerializeField]
        private uint m_id = 0;
        public uint Id => m_id;

        [SerializeField]
        private int m_count = 0;
        public int Count => m_count;
    }

    /// <summary>
    /// 区域施設レコード
    /// </summary>
    [Serializable]
    public class SfZoneFacilityRecord
    {
        // アイコン(施設画像)
        [SerializeField, HideLabel, PreviewField(55), HorizontalGroup(55, LabelWidth = 67)]
        private Sprite m_icon = null;
        public Sprite FacilitySprite => m_icon;

        // 名前
        [SerializeField]
        private string m_name;

        // 区域施設タイプ
        [SerializeField]
        private eZoneFacilityType m_zoneFacilityType = eZoneFacilityType.None;
        public eZoneFacilityType Type => m_zoneFacilityType;
        

        // コスト(生産資源ID,必要数)
        [SerializeField]
        private List<SfCost> m_costs = new List<SfCost>();
        public List<SfCost> Costs => m_costs;

        // 説明文
        [SerializeField, Multiline(3)]
        private string m_desc = "";
        public string Description => m_desc;

        // 施設のアイテム生成カテゴリ
        [SerializeField]
        private eFacilityItemGenCategory m_itemGenCategory = eFacilityItemGenCategory.None;
        public eFacilityItemGenCategory ItemGenCategory => m_itemGenCategory;

        // 生成するアイテムの ID
        [SerializeField]
        private uint m_genItemId = 0;
        public uint GenItemId => m_genItemId;
    }

    [CreateAssetMenu(menuName = "RecordTables/Create SfZoneFacilityRecordTable", fileName = "SfZoneFacilityRecordTable", order = 10000)]
    public class SfZoneFacilityRecordTable : EditorRecordTable<SfZoneFacilityRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/SfZoneFacilityRecordTable";
        // singleton instance
        protected static SfZoneFacilityRecordTable s_instance = null;
        // singleton getter 
        public static SfZoneFacilityRecordTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfZoneFacilityRecordTable);
        // get record
        public override SfZoneFacilityRecord Get(uint id) => m_recordList.Find(r => r.Type == (eZoneFacilityType)id);
        public SfZoneFacilityRecord Get(eZoneFacilityType id) => m_recordList.Find(r => r.Type == id);
    }
}