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
    /// ê∂éYéëåπ(êŒ)
    /// </summary>
    [CreateAssetMenu(menuName = "RecordTables/Create SfProductionResourceStoneRecordTable", fileName = "SfProductionResourceStoneRecordTable", order = 11000)]
    public class SfProductionResourceStoneRecordTable : EditorRecordTable<SfProductionResourceRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/SfProductionResourceStoneRecordTable";
        // singleton instance
        protected static SfProductionResourceStoneRecordTable s_instance = null;
        // singleton getter
        public static SfProductionResourceStoneRecordTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProductionResourceStoneRecordTable);
        // get record
        public override SfProductionResourceRecord Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
}