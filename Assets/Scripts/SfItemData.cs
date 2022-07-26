using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sfproj
{
    public class SfItemData : IJsonParser
    {
        // �A�C�e�� ID (���j�[�N ID)
        public uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // ����
        public string m_name = "";
        public string Name => m_name;

        // ��{�ƂȂ�A�C�e���J�e�S��(���Y�����H��)
        public eFacilityItemGenCategory m_baseItemCategory = eFacilityItemGenCategory.None;
        public eFacilityItemGenCategory BaseItemCategory => m_baseItemCategory;

        // ��{�ƂȂ�A�C�e�� ID
        public uint m_baseItemId = 0;
        public uint BaseItemId => m_baseItemId;

        public void Parse(IDictionary<string, object> data)
        {
            data.Get(nameof(m_id), out m_id);

            data.Get(nameof(m_name), out m_name);

            data.GetEnum(nameof(m_baseItemCategory), out m_baseItemCategory);

            data.Get(nameof(m_baseItemId), out m_baseItemId);
        }
    }

    /// <summary>
    /// �n�� �Ǘ�
    /// �v���C���ɐ�������Ă��邷�ׂĂ� SfAreaData
    /// �ۑ����͕ʃt�@�C���̕ʃN���X�Ɏ���
    /// </summary>
    public class SfItemDataTable : RecordTable<SfItemData>
    {
        // ���j�[�N ID ���X�g
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        // �o�^
        public void Regist(SfItemData record) => RecordList.Add(record);

        // �A�C�e�����R�[�h�̎擾
        public override SfItemData Get(uint id) => RecordList.Find(r => r.Id == id);
    }

    /// <summary>
    /// �n�惌�R�[�h�e�[�u���Ǘ�
    /// </summary>
    public class SfItemDataTableManager : SfItemDataTable
    {
        private static SfItemDataTableManager s_instance = null;

        public static SfItemDataTableManager Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;

                s_instance = new SfItemDataTableManager();

                s_instance.Load();

                return s_instance;
            }
        }

        /// <summary>
        /// �ǂݍ��ݏ���
        /// </summary>
        public void Load()
        {

            var builder = new ESLoadBuilder<SfItemData, SfItemDataTable>("SfItemDataTable");

            var director = new RecordTableESDirector<SfItemData>(builder);

            director.Construct();

            if (director.GetResult() != null && director.GetResult().RecordList.Count > 0)
            {
                m_recordList.AddRange(director.GetResult().RecordList);
            }
        }

        /// <summary>
        /// �ۑ�����
        /// </summary>
        public void Save()
        {

            var builder = new ESSaveBuilder<SfItemData>("SfItemDataTable", this);

            var director = new RecordTableESDirector<SfItemData>(builder);

            director.Construct();
        }
    }
}