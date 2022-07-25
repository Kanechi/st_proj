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
    /// ê∂éYéëåπ(ñÿçﬁ)
    /// </summary>
    [CreateAssetMenu(menuName = "RecordTables/Create SfProductionResourceWoodRecordTable", fileName = "SfProductionResourceRecord", order = 11004)]
    public class SfProductionResourceWoodRecordTable : EditorRecordTable<SfProductionResourceRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/ProductionResources/SfProductionResourceWoodRecordTable";
        // singleton instance
        protected static SfProductionResourceWoodRecordTable s_instance = null;
        // singleton getter
        public static SfProductionResourceWoodRecordTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProductionResourceWoodRecordTable);
        // get record
        public override SfProductionResourceRecord Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
}