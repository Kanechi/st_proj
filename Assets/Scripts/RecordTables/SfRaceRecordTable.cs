using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// �푰�^�C�v
    /// </summary>
    public enum eRaceType
    {
        None = 0,

        Human,  // �l��
        Elf,    // �G���t
        Dowarf, // �h���[�t
    }



    /// <summary>
    /// �푰���R�[�h
    /// stellaris �Ō������̋N����琭���獑�����
    /// �t�@���^�W�[�Ȃ̂Ő���⍑�������X�L���`���ŏ㏸�����Ă��������Ȃ�
    /// �X�L���̎擾�̓��[���Ƃ��X�L���擾���\�ȃA�C�e�����������p��
    /// </summary>
    [Serializable]
    public class SfRaceRecord
    {
        // ID
        [SerializeField]
        private uint m_id;
        public uint Id => m_id;

        // �푰�^�C�v
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