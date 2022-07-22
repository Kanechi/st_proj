using UnityEngine;
using System;

using Sirenix.OdinInspector;

namespace sfproj
{
    public enum eItemCategory
    {
        ProductionResource,
        ProcessedGoods,
    }

    public enum eProductionResouceCategory
    {
        None        = 0,
        // 木
        Wood        = 100,
        // 石
        Stone       = 200,
        // 鉱物
        Mineral     = 300,
        // 穀物
        Grain       = 400,
        // 果実
        Fruit       = 500,
        // 野菜
        Vegetable   = 600,
        // 肉
        Meat        = 700,
        // 皮、皮膚、肌、表皮、毛皮
        Skin        = 800,
    }

    public enum eProcessdGoodsCategory
    { 
        None        = 0,
        // 食材(穀物、果実、野菜、肉から生成)
        Food        = 100,
        // 装備(石、鉱物、木、皮から生成)
        Equipment   = 200,
        // 武具(石、鉱物、木、皮から生成)
        Arms        = 300,
        // ルーン(石から生成)
        Rune        = 400,
    }

    [SerializeField]
    public class SfItemRecord
    {
        // アイコン
        [SerializeField, HideLabel, PreviewField(55), HorizontalGroup(55, LabelWidth = 67)]
        private Texture m_icon = null;

        /// <summary>
        /// 名称
        /// 基準となる名称
        /// ゲーム開始時世界が生成された時点でその地域で生産される生産資源は決定される
        /// その際にこの名前が小麦であった場合、～小麦という名称に変化して保存される
        /// 例えばライラック小麦が生産される場合、
        /// マーケットを建設するとライラックパン、もしくはライラックパスタが生産される
        /// </summary>
        [SerializeField]
        private string m_name;

        // 生産資源 ID
        [SerializeField]
        private uint m_id = 0;
        public uint Id => m_id;

        // 生産資源画像
        [SerializeField]
        private Sprite m_sprite = null;
        public Sprite Sprite => m_sprite;

        // 説明文
        [SerializeField]
        private string m_desc = null;
        public string Desc => m_desc;
    }

    /// <summary>
    /// 生産資源
    /// </summary>
    [Serializable]
    public class SfProductionResourceRecord : SfItemRecord
    {
        [SerializeField]
        private eProductionResouceCategory m_category = eProductionResouceCategory.None;
        public eProductionResouceCategory Category => m_category;
    }

    /// <summary>
    /// 加工品
    /// </summary>
    [SerializeField]
    public class SfProcessdGoodsRecord : SfItemRecord
    {
        [SerializeField]
        private eProcessdGoodsCategory m_category = eProcessdGoodsCategory.None;
        public eProcessdGoodsCategory Category => m_category;
    }

#if false
    /// <summary>
    /// この部分を生産物ごとにファイル分けし量産すれば Unity で管理が楽になる
    /// </summary>
    [CreateAssetMenu(menuName = "RecordTables/Create SfProductionResourceTable", fileName = "SfProductionResourceTable", order = 10001)]
    public class SfProductionResourceTable : EditorRecordTable<SfProductionResource>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/SfProductionResourceTable";
        // singleton instance
        protected static SfProductionResourceTable s_instance = null;
        // singleton getter
        public static SfProductionResourceTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProductionResourceTable);
        // get record
        public override SfProductionResource Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
#endif



    public class SfProductionResourceTableManager : Singleton<SfProductionResourceTableManager>
    {
        
    }
}