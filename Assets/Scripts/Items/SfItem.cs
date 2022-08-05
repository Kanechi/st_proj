using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sfproj
{
    public enum eRarity {
        None = -1,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
    }

    public class SfItem : IJsonParser
    {
        // �A�C�e�� ID (���j�[�N ID)
        public uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // ����
        public string m_name = "";
        public string Name { get => m_name; set => m_name = value; }

        // ��{�ƂȂ�A�C�e�� ID
        public uint m_baseItemId = 0;
        public uint BaseItemId { get => m_baseItemId; set => m_baseItemId = value; }

        // ���A���e�B
        public eRarity m_rarity = eRarity.None;
        public eRarity Rarity { get => m_rarity; set => m_rarity = value; }

        public virtual void Parse(IDictionary<string, object> data)
        {
            data.Get(nameof(m_id), out m_id);

            data.Get(nameof(m_name), out m_name);

            data.Get(nameof(m_baseItemId), out m_baseItemId);

            data.GetEnum(nameof(m_rarity), out m_rarity);
        }
    }

    /// <summary>
    /// ���Y�����A�C�e��
    /// </summary>
    public class SfProductionResourceItem : SfItem
    {
        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);
        }

        public SfProductionResourceItem Clone() => (SfProductionResourceItem)MemberwiseClone();
    }

    /// <summary>
    /// ���H�i�A�C�e��
    /// </summary>
    public class SfProcessedGoodsItem : SfItem
    {
        // ���

        // ���� (���ʂ͕ʓr�֐������邩������Ȃ��B���̏ꍇ�͒l�͂��̊֐����ŗ��p����邱�ƂɂȂ�)

        // �l

        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);
        }

        public SfProcessedGoodsItem Clone() => (SfProcessedGoodsItem)MemberwiseClone();
    }

    /// <summary>
    /// ���Y�����A�C�e���e�[�u��
    /// �R��������G�s�b�N�܂�
    /// ���W�F���_���͖���
    /// </summary>
    public class SfProductionResourceItemTable : RecordTable<SfProductionResourceItem>
    {
        // �o�^
        public void Regist(SfProductionResourceItem record) => RecordList.Add(record);

        // �A�C�e�����R�[�h�̎擾
        public override SfProductionResourceItem Get(uint id) => RecordList.Find(r => r.Id == id);
        public SfProductionResourceItem Get(uint baseItemid, eRarity rarity) => RecordList.Find(r => r.Rarity == rarity && r.BaseItemId == baseItemid);

        // �������A���e�B�œ�����{�A�C�e�� ID �̓����A�C�e�������łɑ��݂��Ă��邩�ǂ����`�F�b�N
        public bool CheckExist(uint baseItemid, eRarity rarity) => Get(baseItemid, rarity) != null;
    }

    /// <summary>
    /// ���Y�����A�C�e���e�[�u���Ǘ�
    /// </summary>
    public class SfProductionResourceItemTableManager : Singleton<SfProductionResourceItemTableManager>
    {
        private SfProductionResourceItemTable m_table = new SfProductionResourceItemTable();
        public SfProductionResourceItemTable Table => m_table;

        // ���j�[�N ID ���X�g
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();


        /// <summary>
        /// �ǂݍ��ݏ���
        /// </summary>
        public void Load()
        {
            var director = new RecordTableESDirector<SfProductionResourceItem>(new ESLoadBuilder<SfProductionResourceItem, SfProductionResourceItemTable>("SfProductionResourceItemTable"));

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
            var director = new RecordTableESDirector<SfProductionResourceItem>(new ESSaveBuilder<SfProductionResourceItem>("SfProductionResourceItemTable", m_table));

            director.Construct();
        }
    }
}