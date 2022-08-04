using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Sirenix.OdinInspector;

namespace sfproj
{
    public enum eProductionResoucePlantCategory
    {
        None = 0,

        // ƒxƒŠ[
        Berry = 140000,
        // ƒŠƒ“ƒS
        Apple,

        // ˆğ
        Potato = 141000,
    }

    /// <summary>
    /// ¶Y‘Œ¹(A•¨)
    /// </summary>
    [CreateAssetMenu(menuName = "RecordTables/Create SfProductionResourcePlantRecordTable", fileName = "SfProductionResourceRecord", order = 11004)]
    public class SfProductionResourcePlantRecordTable : EditorRecordTable<SfProductionResourceRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/ProductionResources/SfProductionResourcePlantRecordTable";
        // singleton instance
        protected static SfProductionResourcePlantRecordTable s_instance = null;
        // singleton getter
        public static SfProductionResourcePlantRecordTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProductionResourcePlantRecordTable);
        // get record
        public override SfProductionResourceRecord Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
}