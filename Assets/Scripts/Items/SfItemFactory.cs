using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// �A�C�e�����񐶐��r���_�[ (���)
    /// </summary>
    public abstract class SfItemFactoryBuilderBase
    {
        // �쐬���ꂽ�A�C�e��
        protected SfItem m_createdItem = null;
        public SfItem GetCreatedItem() => m_createdItem;

        public abstract void CreateItem(uint itemId);

        /// <summary>
        /// �A�C�e���̃��A���e�B�������_���ɐݒ�
        /// </summary>
        public void RandomSettingItemRarity()
        {
            // �R�����A�A���R�����Ȃ炻�̂܂܂̖��O���g���
            // ���A�A�G�s�b�N�Ȃ�̈於���g���
            // ���W�F���_���[�Ȃ�n�於���g����
            int[] m_rarityList = new int[] {
                SfConfigController.Instance.ItemRarityRateCommon,
                SfConfigController.Instance.ItemRarityRateUncommmon,
                SfConfigController.Instance.ItemRarityRateRare,
                SfConfigController.Instance.ItemRarityRateEpic,
                SfConfigController.Instance.ItemRarityRateLegendary
            };

            int rarity = SfConstant.WeightedPick(m_rarityList);

            m_createdItem.Rarity = (eRarity)rarity;
        }

        // ��{�A�C�e���̐ݒ�
        public abstract void SettingBaseItemID();

        // ��{�A�C�e���̊�{�����擾
        public abstract string GetBaseName();

        // �A�C�e���̖��̂̐ݒ�
        // �A�C�e���̖��̂̐ݒ�
        public void SettingItemName(SfZoneFacility zoneFacility)
        {
            string uniqueName = "";

            if (m_createdItem.Rarity == eRarity.Common || m_createdItem.Rarity == eRarity.Uncommon)
            {
                // ���E�̖��O����
                uniqueName = SfConfigController.Instance.WorldName;
            }
            else if (m_createdItem.Rarity == eRarity.Rare || m_createdItem.Rarity == eRarity.Epic)
            {
                // �̈悩��
                var area = SfAreaTableManager.Instance.Table.Get(zoneFacility.AreaId);
                var dominion = SfDominionTableManager.Instance.Table.Get(area.DominionId);

                uniqueName = dominion.Name;
            }
            else if (m_createdItem.Rarity == eRarity.Legendary)
            {
                // �n�悩��
                var area = SfAreaTableManager.Instance.Table.Get(zoneFacility.AreaId);

                uniqueName = area.Name;
            }

            m_createdItem.Name = uniqueName + GetBaseName();
        }
    }

    /// <summary>
    /// ���Y�����A�C�e������쐬�p�r���_�[ (���)
    /// </summary>
    public class SfProductionResourceItemFactoryBuilder : SfItemFactoryBuilderBase
    {
        // �A�C�e���̊�ƂȂ鐶�Y�������R�[�h
        private SfProductionResourceRecord m_record = null;

        // constructor
        public SfProductionResourceItemFactoryBuilder(SfProductionResourceRecord record) => m_record = record;

        public override void CreateItem(uint itemId) {
            m_createdItem = new SfItem();
            m_createdItem.Id = itemId;
            m_createdItem.BaseItemCategory = eFacilityItemGenCategory.Production;
        }

        // ��{�A�C�e���̐ݒ�
        public override void SettingBaseItemID() => m_createdItem.BaseItemId = m_record.Id;

        // ��{�A�C�e���̊�{���������_���Ɏ擾
        public override string GetBaseName()
        {
            int index = UnityEngine.Random.Range(0, m_record.BaseNameList.Count);
            return m_record.BaseNameList[index];
        }
    }

    /// <summary>
    /// ���H�i�A�C�e������쐬�p�r���_�[ (���)
    /// </summary>
    public class SfProcessGoodsItemFactoryBuilder : SfItemFactoryBuilderBase
    {
        // �A�C�e���̊�ƂȂ���H�i���R�[�h
        private SfProcessedGoodsRecord m_record = null;

        // constructor
        public SfProcessGoodsItemFactoryBuilder(SfProcessedGoodsRecord record) => m_record = record;

        public override void CreateItem(uint itemId)
        {
            m_createdItem = new SfItem();
            m_createdItem.Id = itemId;
            m_createdItem.BaseItemCategory = eFacilityItemGenCategory.Processed;
        }

        // ��{�A�C�e���̐ݒ�
        public override void SettingBaseItemID() => m_createdItem.BaseItemId = m_record.Id;

        // ��{�A�C�e���̊�{�����擾
        public override string GetBaseName() => m_record.BaseName;
    }

    /// <summary>
    /// �A�C�e������쐬�p�f�B���N�^
    /// �A�C�e��������쐬�����Ƃ��ɂ̂݌Ăяo��
    /// </summary>
    public class SfItemFactoryDirector
    {
        private SfItemFactoryBuilderBase m_builder = null;

        public SfItemFactoryDirector(SfItemFactoryBuilderBase builder) => m_builder = builder;

        public SfItem GetCreatedItem() => m_builder.GetCreatedItem();

        /// <summary>
        /// ����쐬�̂�
        /// </summary>
        /// <param name="zoneFacility">�A�C�e���̏���쐬���s����{�݃f�[�^</param>
        public void Construct(SfZoneFacility zoneFacility)
        {
            m_builder.CreateItem(SfConstant.CreateUniqueId(ref SfItemTableManager.Instance.m_uniqueIdList));

            // ��ƂȂ�A�C�e���̃A�C�e�� ID ��ݒ�
            m_builder.SettingBaseItemID();

            // �A�C�e���̃��A���e�B�������_���ɐݒ肷��
            m_builder.RandomSettingItemRarity();

            // ���̂̓��A���e�B�Ō��肳���̂Ń��A���e�B�̌�ɖ��̂�ݒ肷��

            // �A�C�e���̖��̂�ݒ肷��
            m_builder.SettingItemName(zoneFacility);
        }
    }
}