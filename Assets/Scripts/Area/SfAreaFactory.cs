using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj {

    /// <summary>
    /// �n�搶�� �H�� ���
    /// 2022/0608
    ///     �s�s�ƈ�ՂƓ��A�ŕ�����ׂ��H
    ///     �ő��搔 = �ő咲�����H
    /// </summary>
    public abstract class SfAreaFactoryBase
    {
        public SfAreaData Create(uint uniqueId, int areaIndex, uint dominionId)
        {
            var data = CreateAreaData();

            // �n�� ID �̐ݒ�
            data.Id = uniqueId;

            // �n�於��ݒ�
            data.Name = CreateRandomAreaName();

            // �̈� ID ��ݒ�
            data.DominionId = dominionId;

            // �n��C���f�b�N�X�̐ݒ�
            data.AreaIndex = areaIndex;

            // �n���^�C�v�̐ݒ�
            data.AreaGroupType = SettingRandomAreaGroupType();

            // �n��^�C�v�̐ݒ�
            data.AreaType = RandomSettingAreaType();

            // �אڒn�`�^�C�v�̐ݒ�
            data.ExistingTerrain = SettingExistingTerrain(SfDominionRecordTableManager.Instance.Get(dominionId));

            // �ő��搔�̐ݒ�
            int ct = data.MaxZoneCount = CulcMaxZoneCount();

            for (int i = 0; i < ct; ++i)
            {
                data.ZoneFacilityList.Add(new SfAreaData.ZoneFacilitySet(i));
            }

            return data;
        }

        /// <summary>
        /// �n�惌�R�[�h�𐶐�
        /// </summary>
        /// <returns></returns>
        protected abstract SfAreaData CreateAreaData();

        /// <summary>
        /// �n�於�������_���ɐ���
        /// </summary>
        /// <returns></returns>
        protected abstract string CreateRandomAreaName();

        // �n���^�C�v��ݒ�
        protected abstract eAreaGroupType SettingRandomAreaGroupType();

        // �����_���ɒn��^�C�v��ݒ�
        protected abstract eAreaType RandomSettingAreaType();

        // �אڒn�`�^�C�v�̐ݒ�
        protected abstract eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion);

        // �ő��搔�̌v�Z
        protected abstract int CulcMaxZoneCount();
    }

    /// <summary>
    /// �n�惌�R�[�h���� �H��
    /// </summary>
    public abstract class SfAreaCreateFactory : SfAreaFactoryBase
    {
        protected override SfAreaData CreateAreaData()
        {
            return new SfAreaData();
        }
    }

    /// <summary>
    /// �n�� ���n ���� �H��
    /// </summary>
    public class SfAreaFactoryPlane : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test_plane"; }

        // �n���^�C�v��ݒ�
        protected override eAreaGroupType SettingRandomAreaGroupType() { return eAreaGroupType.Plane; }

        // �n��^�C�v�������_���ɐݒ�
        protected override eAreaType RandomSettingAreaType() { return eAreaType.Town; }

        // ���݂���n�`�̐ݒ�
        protected override eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion)
        {
            eExistingTerrain terrain = 0;

            // �����A�R�A�X�A�C�A�͊m�����z�����A�d�����\�Ȃ̂ł��ꂼ������ꂼ�ꂾ���̊����Ōv�Z


            // ����
            float rate = UnityEngine.Random.value * 100.0f;
            if (SfConfigController.Instance.DistributionRatioPlane > rate)
            {
                terrain |= eExistingTerrain.Plane;
            }

            // �X
            rate = UnityEngine.Random.value * 100.0f;
            if (SfConfigController.Instance.DistributionRatioForest > rate)
            {
                terrain |= eExistingTerrain.Forest;
            }

            // �R
            rate = UnityEngine.Random.value * 100.0f;
            if (SfConfigController.Instance.DistributionRatioMountain > rate)
            {
                terrain |= eExistingTerrain.Mountain;
            }

            // ��
            rate = UnityEngine.Random.value * 100.0f;
            if (SfConfigController.Instance.DistributionRatioRiver > rate)
            {
                terrain |= eExistingTerrain.River;
            }

            // �C�̂ݗ̈悪�C�ɖʂ��Ă��邩�ǂ������`�F�b�N���ăt���O�𗧂Ă�
            if (dominion.NeighboursOceanFlag == true)
            {
                rate = UnityEngine.Random.value * 100.0f;
                if (SfConfigController.Instance.DistributionRatioOcean > rate)
                {
                    terrain |= eExistingTerrain.Ocean;
                }
            }

            // �����n�`�����������ꍇ�͕�����ݒ�
            if (terrain == 0)
                terrain |= eExistingTerrain.Plane;

            return terrain;
        }


        // ���ő吔�̌v�Z
        protected override int CulcMaxZoneCount() { return UnityEngine.Random.Range(SfConfigController.Instance.MinZoneValue, SfConfigController.Instance.MaxZoneValue + 1); }
    }

    /// <summary>
    /// �n�� ��� ���� �H��
    /// </summary>
    public class SfAreaFactoryRemains : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test_reamins"; }

        // �n���^�C�v��ݒ�
        protected override eAreaGroupType SettingRandomAreaGroupType() { return eAreaGroupType.Remains; }

        // �n��^�C�v�������_���ɐݒ�
        protected override eAreaType RandomSettingAreaType() { return eAreaType.Remains; }

        // ���݂���n�`�̐ݒ� (��Ղ͒n�`���ʂ͂��ɂ��Ȃ�)
        protected override eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion)
        {
            return 0;
        }

        protected override int CulcMaxZoneCount() { return -1; }
    }

    /// <summary>
    /// �n�� ���A ���� �H��
    /// </summary>
    public class SfAreaFactoryCave : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test_cave"; }

        // �n���^�C�v��ݒ�
        protected override eAreaGroupType SettingRandomAreaGroupType() { return eAreaGroupType.Cave; }

        // �n��^�C�v�������_���ɐݒ�
        protected override eAreaType RandomSettingAreaType() { return eAreaType.Cave; }

        // ���݂���n�`�̐ݒ� (���A�͒n�`���ʂ͂��ɂ��Ȃ�)
        protected override eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion)
        {
            return 0;
        }

        protected override int CulcMaxZoneCount() { return -1; }
    }

    /// <summary>
    /// �n�� ���� �H�� �Ǘ�
    /// </summary>
    public class SfAreaFactoryManager : Singleton<SfAreaFactoryManager>
    {
        private List<SfAreaFactoryBase> factoryList = null;

        public SfAreaFactoryManager()
        {

            factoryList = new List<SfAreaFactoryBase>()
            {
                new SfAreaFactoryPlane(),
                new SfAreaFactoryRemains(),
                new SfAreaFactoryCave(),
            };
        }

        /// <summary>
        /// �n��̍쐬
        /// </summary>
        /// <param name="areaIndex">�n��C���f�b�N�X(�Z���ԍ�)</param>
        /// <param name="dominionId">�����Ă���̈� ID</param>
        /// <returns></returns>
        public SfAreaData RandomCreate(int areaIndex, uint dominionId, eAreaGroupType factoryType = eAreaGroupType.None)
        {
            // ������Ղ����A���������_��
            // �����͐ݒ�ł���悤�ɂ���
            float rate = UnityEngine.Random.value * 100.0f;

            if (factoryType == eAreaGroupType.None)
            {
                // �� (rate �� 80 �ȉ��Ȃ璬)
                if (SfConfigController.Instance.AreaTownRate > rate)
                {
                    // ��
                    factoryType = eAreaGroupType.Plane;
                }
                // ��� (rate �� 80 ���� 90 �Ȃ���)
                else if (SfConfigController.Instance.AreaTownRate <= rate && (SfConfigController.Instance.AreaTownRate + SfConfigController.Instance.AreaRemainsRate) > rate)
                {
                    // ���
                    factoryType = eAreaGroupType.Remains;
                }
                // ���A (rate �� 90 ���� 100 �Ȃ���)
                else if ((SfConfigController.Instance.AreaTownRate + SfConfigController.Instance.AreaRemainsRate) <= rate && (SfConfigController.Instance.AreaTownRate + SfConfigController.Instance.AreaRemainsRate + SfConfigController.Instance.AreaCaveRate) > rate)
                {
                    // ���A
                    factoryType = eAreaGroupType.Cave;
                }
            }

            if (factoryType == eAreaGroupType.None)
            {
                Debug.LogError("factoryType == -1 !!!");
                return null;
            }

            // ���j�[�N ID �̍쐬
            uint uniqueId = SfConstant.CreateUniqueId(ref SfAreaDataTableManager.Instance.m_uniqueIdList);

            // �n��f�[�^���쐬
            var data = factoryList[(int)factoryType].Create(uniqueId, areaIndex, dominionId);

            return data;
        }
    }
}