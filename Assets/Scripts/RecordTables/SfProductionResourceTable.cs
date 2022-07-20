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
    /// ê∂éYéëåπ
    /// </summary>
    public class SfProductionResource
    {
        // ÉAÉCÉRÉì
        [SerializeField, HideLabel, PreviewField(55), HorizontalGroup(55, LabelWidth = 67)]
        private Texture m_icon = null;

        // ñºëO
        [SerializeField]
        private string m_name;

        // ê∂éYéëåπ ID
        [SerializeField]
        private uint m_id = 0;
        public uint Id => m_id;

        // ê∂éYéëåπâÊëú
        [SerializeField]
        private Sprite m_sprite = null;
        public Sprite Sprite => m_sprite;

        // ê‡ñæï∂
        private string m_desc = null;
        public string Desc => m_desc;

    }

    [CreateAssetMenu(menuName = "RecordTables/Create SfProductionResourceTable", fileName = "SfProductionResourceTable", order = 10000)]
    public class SfProductionResourceTable : EditorRecordTable<SfProductionResource>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/TerrainTileRecordTable";
        // singleton instance
        protected static SfProductionResourceTable s_instance = null;
        // singleton getter 
        public static SfProductionResourceTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProductionResourceTable);
        // get record
        public override SfProductionResource Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
}