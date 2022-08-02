using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Sirenix.OdinInspector;

namespace sfproj
{
    public enum eProcessedGoodsFoodCategory
    {
        None = 0,

        // ÉpÉì
        Bread = 10000,
    }

    /// <summary>
    /// â¡çHïi(êHóø)
    /// </summary>
    [CreateAssetMenu(menuName = "RecordTables/Create SfProcessedGoodsFoodRecordTable", fileName = "SfProcessedGoodsRecord", order = 12001)]
    public class SfProcessedGoodsFoodRecordTable : EditorRecordTable<SfProcessedGoodsRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/ProcessedGoods/SfProcessedGoodsFoodRecordTable";
        // singleton instance
        protected static SfProcessedGoodsFoodRecordTable s_instance = null;
        // singleton getter
        public static SfProcessedGoodsFoodRecordTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProcessedGoodsFoodRecordTable);
        // get record
        public override SfProcessedGoodsRecord Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
}