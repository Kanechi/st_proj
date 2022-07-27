using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sfproj
{
    public class SfItem : IJsonParser
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
    public class SfItemTable : RecordTable<SfItem>
    {
        // �o�^
        public void Regist(SfItem record) => RecordList.Add(record);

        // �A�C�e�����R�[�h�̎擾
        public override SfItem Get(uint id) => RecordList.Find(r => r.Id == id);

        /// <summary>
        /// �V�K�ɒn����L�̐V�������Y�A�C�e����o�^
        /// �E�E�E����Â炢
        /// �E�E�Efactory �ł�邱�Ƃ���
        /// </summary>
        /// <param name="area"></param>
        /// <param name="productionRecource"></param>
        public void CreateNewItem(SfArea area, SfProductionResourceRecord productionRecource)
        { 
            // �J�e�S���𐶎Y�A�C�e���ɐݒ�
            

            // ���̂�����

        }
    }

    /// <summary>
    /// �n�惌�R�[�h�e�[�u���Ǘ�
    /// </summary>
    public class SfItemTableManager : Singleton<SfItemTableManager>
    {
        private SfItemTable m_table = new SfItemTable();
        public SfItemTable Table => m_table;

        // ���j�[�N ID ���X�g
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        /// <summary>
        /// �ǂݍ��ݏ���
        /// </summary>
        public void Load()
        {
            var director = new RecordTableESDirector<SfItem>(new ESLoadBuilder<SfItem, SfItemTable>("SfItemDataTable"));

            director.Construct();

            if (director.GetResult() != null && director.GetResult().RecordList.Count > 0)
            {
                m_table.RecordList.AddRange(director.GetResult().RecordList);
            }
        }

        /// <summary>
        /// �ۑ�����
        /// </summary>
        public void Save()
        {
            var director = new RecordTableESDirector<SfItem>(new ESSaveBuilder<SfItem>("SfItemDataTable", m_table));

            director.Construct();
        }
    }
}