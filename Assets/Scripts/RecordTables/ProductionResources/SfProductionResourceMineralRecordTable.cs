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
    /// ���Y����(�z��)
    /// </summary>
    [CreateAssetMenu(menuName = "RecordTables/Create SfProductionResourceMineralRecordTable", fileName = "SfProductionResourceRecord", order = 11001)]
    public class SfProductionResourceMineralRecordTable : EditorRecordTable<SfProductionResourceRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/ProductionResources/SfProductionResourceMineralRecordTable";
        // singleton instance
        protected static SfProductionResourceMineralRecordTable s_instance = null;
        // singleton getter
        public static SfProductionResourceMineralRecordTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProductionResourceMineralRecordTable);
        // get record
        public override SfProductionResourceRecord Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
}