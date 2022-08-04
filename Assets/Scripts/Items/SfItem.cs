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
    /// ���Y�����A�C�e�� (�R�����A�A���R����)
    /// </summary>
    public class SfProductionResourceItem : SfItem 
    {
        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);
        }
    }

    /// <summary>
    /// �̈挴�Y�̐��Y�����A�C�e�� (���A�A�G�s�b�N)
    /// </summary>
    public class SfDominionProductionResourceItem : SfProductionResourceItem
    {
        // ���Y�n ID
        public uint m_placeOriginId = 0;
        public uint PlaceOriginId { get => m_placeOriginId; set => m_placeOriginId = value; }

        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);

            data.Get(nameof(m_placeOriginId), out m_placeOriginId);
        }
    }

    /// <summary>
    /// �n�挴�Y�̐��Y�����A�C�e�� (���W�F���_���[)
    /// </summary>
    public class SfAreaProductionResourceItem : SfProductionResourceItem
    {
        // ���Y�n ID
        public uint m_placeOriginId = 0;
        public uint PlaceOriginId { get => m_placeOriginId; set => m_placeOriginId = value; }

        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);

            data.Get(nameof(m_placeOriginId), out m_placeOriginId);
        }
    }

    /// <summary>
    /// ���H�i�A�C�e��
    /// </summary>
    public class SfProcessedGoodsItem : SfItem
    {
        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);
        }
    }

    /// <summary>
    /// �̈挴�Y�̉��H�i�A�C�e��
    /// </summary>
    public class SfDominionProcessedGoodsItem : SfProcessedGoodsItem
    {
        // ���Y�n ID
        public uint m_placeOriginId = 0;
        public uint PlaceOriginId { get => m_placeOriginId; set => m_placeOriginId = value; }

        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);

            data.Get(nameof(m_placeOriginId), out m_placeOriginId);
        }
    }

    /// <summary>
    /// �n�挴�Y�̉��H�i�A�C�e��
    /// </summary>
    public class SfAreaProcessedGoodsItem : SfProcessedGoodsItem
    {
        // ���Y�n ID
        public uint m_placeOriginId = 0;
        public uint PlaceOriginId { get => m_placeOriginId; set => m_placeOriginId = value; }

        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);

            data.Get(nameof(m_placeOriginId), out m_placeOriginId);
        }
    }

    /// <summary>
    /// ���Y�����A�C�e���e�[�u��
    /// </summary>
    public class SfProductionResourceItemTable : RecordTable<SfItem>
    {
        // �o�^
        public void Regist(SfItem record) => RecordList.Add(record);

        // �A�C�e�����R�[�h�̎擾
        public override SfItem Get(uint id) => RecordList.Find(r => r.Id == id);

        /// <summary>
        /// �R�����ƃA���R�����̃A�C�e�������łɑ��݂���
        /// </summary>
        /// <param name="baseId"></param>
        /// <param name="rarity"></param>
        /// <returns>true...�d������</returns>
        public bool CheckExistCommonAndUncommon(uint baseId, eRarity rarity)
        {
            return RecordList.Find(r => r.Rarity == rarity && r.BaseItemId == baseId) != null;
        }

        /// <summary>
        /// ���A�ƃG�s�b�N�̃A�C�e�������łɑ��݂���
        /// �̈� ID �̈�v�܂Ŋm�F
        /// </summary>
        /// <param name="baseId"></param>
        /// <param name="rarity"></param>
        /// <param name="dominion"></param>
        /// <returns>true...�d������</returns>
        public bool CheckExistRareAndEpic(uint baseId, eRarity rarity, SfDominion dominion)
        {
            var record = RecordList.Find(r => r.Rarity == rarity && r.BaseItemId == baseId);
            if (record == null)
                return true;
            var dominionItemRecord = record as SfDominionProductionResourceItem;
            return dominionItemRecord.PlaceOriginId == dominion.Id;
        }

        /// <summary>
        /// ���W�F���_���̃A�C�e�������łɑ��݂���
        /// �n�� ID �̈�v�܂Ŋm�F
        /// </summary>
        /// <param name="baseId"></param>
        /// <param name="rarity"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public bool CheckExistLegendary(uint baseId, eRarity rarity, SfArea area)
        {
            var record = RecordList.Find(r => r.Rarity == rarity && r.BaseItemId == baseId);
            if (record == null)
                return true;
            var areaItemRecord = record as SfAreaProductionResourceItem;
            return areaItemRecord.PlaceOriginId == area.Id;
        }
    }

#if false
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
#endif
}