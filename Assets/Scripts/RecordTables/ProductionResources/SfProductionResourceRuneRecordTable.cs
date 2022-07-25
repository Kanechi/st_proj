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
    /// ê∂éYéëåπ(ÉãÅ[Éì)
    /// </summary>
    [CreateAssetMenu(menuName = "RecordTables/Create SfProductionResourceRuneRecordTable", fileName = "SfProductionResourceRecord", order = 11002)]
    public class SfProductionResourceRuneRecordTable : EditorRecordTable<SfProductionResourceRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/ProductionResources/SfProductionResourceRuneRecordTable";
        // singleton instance
        protected static SfProductionResourceRuneRecordTable s_instance = null;
        // singleton getter
        public static SfProductionResourceRuneRecordTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProductionResourceRuneRecordTable);
        // get record
        public override SfProductionResourceRecord Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
}