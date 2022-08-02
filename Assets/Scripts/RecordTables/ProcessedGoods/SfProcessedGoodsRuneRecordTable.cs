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
    [CreateAssetMenu(menuName = "RecordTables/Create SfProcessedGoodsRuneRecordTable", fileName = "SfProcessedGoodsRecord", order = 12004)]
    public class SfProcessedGoodsRuneRecordTable : EditorRecordTable<SfProcessedGoodsRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/ProcessedGoods/SfProcessedGoodsRuneRecordTable";
        // singleton instance
        protected static SfProcessedGoodsRuneRecordTable s_instance = null;
        // singleton getter
        public static SfProcessedGoodsRuneRecordTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProcessedGoodsRuneRecordTable);
        // get record
        public override SfProcessedGoodsRecord Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
}