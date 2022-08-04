using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// ���[���h�ɑ��݂���A�C�e���̊�{�A�C�e�� ID���`�F�b�N����������A�C�e���̊�{�A�C�e�� ID �����肷��
    /// </summary>
    public class SfProductionResourceListChecker
    {
        private void CheckAndAddList(eProductionResourceCategoryFlag checkFlag, eProductionResouceCategory addCategory, ref eProductionResourceCategoryFlag flag, ref List<SfProductionResourceRecord> baseItemList)
        {
            if ((checkFlag & flag) != 0)
            {
                baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(addCategory));
                flag |= eProductionResourceCategoryFlag.Grain;
            }
        }

        public List<SfProductionResourceRecord> GetRecordByTerrain(eExistingTerrain terrain)
        {
            var baseItemList = new List<SfProductionResourceRecord>();

            eProductionResourceCategoryFlag flag = 0;

            if ((eExistingTerrain.Plane & terrain) != 0)
            {
                CheckAndAddList(eProductionResourceCategoryFlag.Grain, eProductionResouceCategory.Grain, ref flag, ref baseItemList);
            }

            if ((eExistingTerrain.Mountain & terrain) != 0)
            {
                if ((eProductionResourceCategoryFlag.Monster & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Monster));
                    flag |= eProductionResourceCategoryFlag.Monster;
                }

                if ((eProductionResourceCategoryFlag.Plant & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Plant));
                    flag |= eProductionResourceCategoryFlag.Plant;
                }

                if ((eProductionResourceCategoryFlag.Wood & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Wood));
                    flag |= eProductionResourceCategoryFlag.Wood;
                }

            }

            if ((eExistingTerrain.Forest & terrain) != 0)
            {
                if ((eProductionResourceCategoryFlag.Grain & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Grain));
                    flag |= eProductionResourceCategoryFlag.Grain;
                }
                if ((eProductionResourceCategoryFlag.Mineral & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Mineral));
                    flag |= eProductionResourceCategoryFlag.Mineral;
                }
                if ((eProductionResourceCategoryFlag.Monster & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Monster));
                    flag |= eProductionResourceCategoryFlag.Monster;
                }
            }

            if ((eExistingTerrain.River & terrain) != 0)
            {
                if ((eProductionResourceCategoryFlag.Monster & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Monster));
                    flag |= eProductionResourceCategoryFlag.Monster;
                }

                if ((eProductionResourceCategoryFlag.Plant & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Plant));
                    flag |= eProductionResourceCategoryFlag.Plant;
                }
            }

            if ((eExistingTerrain.Ocean & terrain) != 0)
            {
                baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Monster));
            }

            return baseItemList;
        }


#if false
        /// <summary>
        /// ���ɐ�������Ă����{�A�C�e�� ID ���`�F�b�N����������Ă��Ȃ����̂��珇�ɐ������s��
        /// ��������Ă��Ȃ����̂��珇�ɐ�������ƒn�`�ɑ΂��ăo���o���ɔz�u����Ă��܂����B
        /// �������̂���������Ȃ��Ȃ��Ă��܂��E�E�E
        /// ��͂蓯�����̂��������������������悢�����E�E�E
        /// </summary>
        /// <returns></returns>
        public uint CheckAndGenRandomBaseId() {

            // �S�Ă̊�{�A�C�e�����擾
            var baseItemList = SfProductionResourceTableManager.Instance.GetAllProductionResourceList();

            // ���󐶐�����Ă����{�A�C�e�� ID ���擾
            var genItemList = SfProductionResourceItemTableManager.Instance.Table.RecordList;

            // ���󐶐�����Ă����{�A�C�e�� ID �𒲂א�������Ă��Ȃ���{�A�C�e���A�C�e�� ID �����X�g��
            var noGenBaseItemList = new List<SfProductionResourceRecord>();
            for (int i = 0; i < baseItemList.Count; ++i)
            {
                // false...�g���Ă��Ȃ�
                bool isUse = false;

                for (int j = 0; j < genItemList.Count; ++j)
                {
                    // �������ꂽ�A�C�e���Ɏg�p����Ă����{�A�C�e�� ID �����R�[�h�̊�{�A�C�e�� ID 
                    if (baseItemList[i].Id == genItemList[j].BaseItemId)
                    {
                        isUse = true;
                    }
                }

                if (isUse == false)
                {
                    // �g�p����Ă��Ȃ���Ύ��t����
                    noGenBaseItemList.Add(baseItemList[i]);
                }
            }

            // �����_���Ɋ�{�A�C�e�� ID ���Z�o
            int index = UnityEngine.Random.Range(0, noGenBaseItemList.Count);
            return noGenBaseItemList[index].Id;
        }
    
#endif
    }

    /// <summary>
    /// �ŏI�I�ɐ��������A�C�e����̈悩�n��ǂ���Ɏ��t���邩�͊O���Ń��A���e�B�𒲂ׂĎ��t����
    /// </summary>
    public abstract class SfItemFactoryBase
    {
        public SfItem Create(uint id, uint baseId, eRarity rarity)
        {
            var item = CreateItem();

            item.Id = id;

            item.BaseItemId = baseId;

            item.Rarity = rarity;

            item.Name = CreateItemName(baseId, rarity);

            return item;
        }

        public abstract SfItem CreateItem();

        // �A�C�e���̖��O�𐶐�
        public abstract string CreateItemName(uint baseId, eRarity rarity);
    }

    /// <summary>
    /// ���Y�����A�C�e���̐���
    /// ���Y�����A�C�e���Ɖ��H�i�A�C�e���ŃN���X�𕪂��邩�l����
    /// </summary>
    public abstract class SfProductionResourceItemCreateFactory : SfItemFactoryBase
    {
        public override SfItem CreateItem()
        {
            var item = new SfItem();

            return item;
        }
    }

    /// <summary>
    /// ���Y�����A�C�e���H��
    /// </summary>
    public class SfProductionResourceItemFactory : SfProductionResourceItemCreateFactory {

        // �A�C�e���̖��O�𐶐�
        public override string CreateItemName(uint baseId, eRarity rarity)
        {
            // ��{�A�C�e�������X�g���擾
            var record = SfProductionResourceTableManager.Instance.Get(baseId);

            // �����_���ɂP�I��
            int index = UnityEngine.Random.Range(0, record.BaseNameList.Count);
            string baseName = record.BaseNameList[index];

            // ���A���e�B�����A���G�s�b�N�Ȃ烉���_���������t��
            if (rarity == eRarity.Rare || rarity == eRarity.Epic)
            {
                baseName = "�󏭂�" + baseName; 
            }

            return baseName;
        }
    }

    /// <summary>
    /// �A�C�e�������p�r���_�[
    /// </summary>
    public abstract class SfItemBuilderBase {

        // �������ꂽ�A�C�e���A�������͂��łɐ�������Ă���A�C�e��
        protected SfItem m_createdItem = null;
        public SfItem GetResult() => m_createdItem;

        // ��{�A�C�e�� ID ��n�`���ƂɃ����_���ɐ���
        public abstract uint GenBaseItemId();

        // �A�C�e���̃��A�x������
        public abstract eRarity GenItemRarity();

        // true...����������{�A�C�e�� ID �� ���A�x���ݒ肳��Ă���A�C�e�������łɑ��݂��Ă���
        public abstract bool CheckExist(uint baseItemId, eRarity rarity);

        // ���������A�C�e���̃��j�[�N ID �𐶐�
        protected abstract uint GenUniqueItemId();

        // �A�C�e���̐���
        public abstract void CreateItem(uint baseItemId, eRarity rarity);

        // �A�C�e���̓o�^
        public abstract void RegistItem();
    }

    /// <summary>
    /// �A�C�e�������p�r���_�[
    /// </summary>
    public class SfProductionResourceItemBuilder : SfItemBuilderBase
    {
        // ���Y�n��
        private SfArea m_placeOriginArea = null;

        // constructor
        public SfProductionResourceItemBuilder(SfArea placeOriginArea) => m_placeOriginArea = placeOriginArea;

        // ��{�A�C�e�� ID ��n�`���ƂɃ����_���ɐ���
        public override uint GenBaseItemId()
        {
            var checker = new SfProductionResourceListChecker();

            var baseItemList = checker.GetRecordByTerrain(m_placeOriginArea.ExistingTerrain);

            int index = UnityEngine.Random.Range(0, baseItemList.Count);

            return baseItemList[index].Id;
        }

        // �A�C�e���̃��A�x������
        public override eRarity GenItemRarity() {

            // �R�����A�A���R�����Ȃ炻�̂܂܂̖��O���g���
            // ���A�A�G�s�b�N�Ȃ�̈於���g���
            // ���W�F���_���[�Ȃ�n�於���g����
            int[] m_rarityList = new int[] {
                SfConfigController.Instance.ItemRarityRateCommon,
                SfConfigController.Instance.ItemRarityRateUncommmon,
                SfConfigController.Instance.ItemRarityRateRare,
                SfConfigController.Instance.ItemRarityRateEpic,
            };

            int rarity = SfConstant.WeightedPick(m_rarityList);

            return (eRarity)rarity;
        }

        // true...����������{�A�C�e�� ID �� ���A�x���ݒ肳��Ă���A�C�e�������łɑ��݂��Ă���
        public override bool CheckExist(uint baseItemId, eRarity rarity)
        {
            m_createdItem = SfProductionResourceItemTableManager.Instance.Table.Get(baseItemId, rarity);

            return m_createdItem != null;
        }

        // ���������A�C�e���̃��j�[�N ID �𐶐�
        protected override uint GenUniqueItemId() => SfConstant.CreateUniqueId(ref SfProductionResourceItemTableManager.Instance.m_uniqueIdList);

        // �A�C�e���̐���
        public override void CreateItem(uint baseItemId, eRarity rarity) {

            var factory = new SfProductionResourceItemFactory();

            uint uniqueId = GenUniqueItemId();

            m_createdItem = factory.Create(uniqueId, baseItemId, rarity);
        }

        // �A�C�e���̓o�^
        public override void RegistItem()
        {
            SfProductionResourceItemTableManager.Instance.Table.Regist(m_createdItem as SfProductionResourceItem);
        }
    }

    /// <summary>
    /// �A�C�e�������p�f�B���N�^�[
    /// </summary>
    public class SfItemGenDirector
    {
        private SfItemBuilderBase m_builder = null;

        public SfItemGenDirector(SfItemBuilderBase builder) => m_builder = builder;

        public SfItem GetResult() => m_builder.GetResult();

        public void Construct() {

            uint baseItemId = m_builder.GenBaseItemId();

            eRarity rarity = m_builder.GenItemRarity();

            // ����������{�A�C�e�� ID �� ���A�x�̃A�C�e�������łɑ��݂����炻����g��
            if (m_builder.CheckExist(baseItemId, rarity) == false)
            {
                m_builder.CreateItem(baseItemId, rarity);

                // �V�K�Ő���������A�C�e���e�[�u���ɓo�^
                m_builder.RegistItem();
            }
        }
    }
}