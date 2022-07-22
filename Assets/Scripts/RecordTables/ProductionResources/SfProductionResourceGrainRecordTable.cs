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
    /// ê∂éYéëåπ(çíï®)
    /// </summary>
    [CreateAssetMenu(menuName = "RecordTables/Create SfProductionResourceGrainRecordTable", fileName = "SfProductionResourceGrainRecordTable", order = 11000)]
    public class SfProductionResourceGrainRecordTable : EditorRecordTable<SfProductionResourceRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/SfProductionResourceGrainRecordTable";
        // singleton instance
        protected static SfProductionResourceGrainRecordTable s_instance = null;
        // singleton getter
        public static SfProductionResourceGrainRecordTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProductionResourceGrainRecordTable);
        // get record
        public override SfProductionResourceRecord Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
}