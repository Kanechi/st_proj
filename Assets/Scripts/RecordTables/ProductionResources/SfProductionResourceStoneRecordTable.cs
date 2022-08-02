using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Sirenix.OdinInspector;

namespace sfproj
{
    public enum eProductionResouceStoneCategory
    {
        None = 0,

        // ïÅí ÇÃêŒ
        Stone = 30000,
    }

    /// <summary>
    /// ê∂éYéëåπ(êŒ)
    /// </summary>
    [CreateAssetMenu(menuName = "RecordTables/Create SfProductionResourceStoneRecordTable", fileName = "SfProductionResourceRecord", order = 11003)]
    public class SfProductionResourceStoneRecordTable : EditorRecordTable<SfProductionResourceRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/ProductionResources/SfProductionResourceStoneRecordTable";
        // singleton instance
        protected static SfProductionResourceStoneRecordTable s_instance = null;
        // singleton getter
        public static SfProductionResourceStoneRecordTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProductionResourceStoneRecordTable);
        // get record
        public override SfProductionResourceRecord Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
}