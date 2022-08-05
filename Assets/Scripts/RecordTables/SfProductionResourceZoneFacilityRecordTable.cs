using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    // ê∂éYéëåπãÊàÊé{ê›É^ÉCÉv
    public enum eProductionResourceZoneFacilityType 
    { 
        None = 0,

        Farm                = 10000,
        Mining,
        DismantlingPlace,
        Orchard,
        LoggingArea,
    }

    [CreateAssetMenu(menuName = "RecordTables/Create SfProductionResourceZoneFacilityRecordTable", fileName = "SfZoneFacilityRecordTable", order = 20000)]
    public class SfProductionResourceZoneFacilityRecordTable : EditorRecordTable<SfZoneFacilityRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/SfProductionResourceZoneFacilityRecordTable";
        // singleton instance
        protected static SfProductionResourceZoneFacilityRecordTable s_instance = null;
        // singleton getter 
        public static SfProductionResourceZoneFacilityRecordTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProductionResourceZoneFacilityRecordTable);
        // get record
        public override SfZoneFacilityRecord Get(uint id) => m_recordList.Find(r => r.TypeId == id);
    }
}
