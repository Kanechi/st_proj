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
    /// ê∂éYéëåπ(êHçﬁ)
    /// </summary>
    [CreateAssetMenu(menuName = "RecordTables/Create SfProcessedGoodsFoodTable", fileName = "SfProcessedGoodsFoodTable", order = 11000)]
    public class SfProcessedGoodsFoodTable : EditorRecordTable<SfProcessdGoodsRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/SfProcessedGoodsFoodTable";
        // singleton instance
        protected static SfProcessedGoodsFoodTable s_instance = null;
        // singleton getter
        public static SfProcessedGoodsFoodTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProcessedGoodsFoodTable);
        // get record
        public override SfProcessdGoodsRecord Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
}