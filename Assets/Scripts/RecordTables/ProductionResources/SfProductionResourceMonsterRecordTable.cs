using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Sirenix.OdinInspector;

namespace sfproj
{
    public enum eProductionResouceMonsterCategory
    {
        None = 0,

        // îÁ
        Leather = 130000,
    }

    /// <summary>
    /// ê∂éYéëåπ(çíï®)
    /// </summary>
    [CreateAssetMenu(menuName = "RecordTables/Create SfProductionResourceMonsterRecordTable", fileName = "SfProductionResourceRecord", order = 11003)]
    public class SfProductionResourceMonsterRecordTable : EditorRecordTable<SfProductionResourceRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/ProductionResources/SfProductionResourceMonsterRecordTable";
        // singleton instance
        protected static SfProductionResourceMonsterRecordTable s_instance = null;
        // singleton getter
        public static SfProductionResourceMonsterRecordTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProductionResourceMonsterRecordTable);
        // get record
        public override SfProductionResourceRecord Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
}