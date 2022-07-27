using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// 種族タイプ
    /// </summary>
    public enum eRaceType
    {
        None = 0,

        Human,  // 人種
        Elf,    // エルフ
        Dowarf, // ドワーフ
    }



    /// <summary>
    /// 種族レコード
    /// stellaris で言う所の起源やら政策やら国是やら
    /// ファンタジーなので政策や国是やらをスキル形式で上昇させていこうかなと
    /// スキルの取得はルーンとかスキル取得が可能なアイテムがいくつか用意
    /// </summary>
    [Serializable]
    public class SfRaceRecord
    {
        // ID
        [SerializeField]
        private uint m_id;
        public uint Id => m_id;

        // 種族タイプ
        [SerializeField]
        private eRaceType m_raceType = eRaceType.None;
        public eRaceType RaceType => m_raceType;
    }

    [CreateAssetMenu(menuName = "RecordTables/Create SfRaceRecordTable", fileName = "SfRaceRecordTable", order = 30000)]
    public class SfRaceRecordTable : EditorRecordTable<SfRaceRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/SfRaceRecordTable";
        // singleton instance
        protected static SfRaceRecordTable s_instance = null;
        // singleton getter 
        public static SfRaceRecordTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfRaceRecordTable);
        // get record
        public override SfRaceRecord Get(uint id) => m_recordList.Find(r => r.Id == id);
        public SfRaceRecord Get(eRaceType type) => m_recordList.Find(r => r.RaceType == type);
    }
}