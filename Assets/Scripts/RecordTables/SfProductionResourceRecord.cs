using UnityEngine;
using System;

using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace sfproj
{ 

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



    /// <summary>
    /// 生産資源
    /// </summary>
    [Serializable]
    public class SfProductionResourceRecord
    {
        // 生産資源画像
        [SerializeField, HideLabel, PreviewField(55), HorizontalGroup(55, LabelWidth = 67)]
        protected Sprite m_icon = null;
        public Sprite Sprite => m_icon;


        // 生産資源 ID
        [SerializeField]
        private uint m_id = 0;
        public uint Id => m_id;

        [SerializeField]
        private eProductionResouceCategory m_category = eProductionResouceCategory.None;
        public eProductionResouceCategory Category => m_category;

        /// <summary>
        /// 基準となる名称
        /// ゲーム開始時世界が生成された時点でその地域で生産される生産資源は決定される
        /// その際にこの名前が小麦であった場合、～小麦という名称に変化して保存される
        /// 例えばライラック小麦が生産される場合、
        /// マーケットを建設するとライラックパン、もしくはライラックパスタが生産される
        /// </summary>
        [SerializeField]
        private string m_baseName;
        public string BaseName => m_baseName;

        // 説明文
        [SerializeField, Multiline(3)]
        private string m_desc = null;
        public string Desc => m_desc;

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