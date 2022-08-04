using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace sfproj {
    public class SfConfigController : SingletonMonoBehaviour<SfConfigController> {

        /// <summary>
        /// ���\���ő吔
        /// ���̕\����̍ő吔
        /// </summary>
        static public readonly int ZONE_MAX_DISPLAY_COUNT = 9;

        /// <summary>
        /// ���ő�g����
        /// </summary>
        static public readonly int ZONE_MAX_EXPANTION_COUNT = 3;

        [Title("�Q�[����Őݒ�\", horizontalLine: false)]

        [Title("�v���C�ݒ�")]

        // �嗤�̃^�C�v
        [SerializeField]
        private eLandType m_landType = eLandType.OneLand;
        public eLandType LandType => m_landType;

        // �嗤�̑傫��(�̈搔)
        // �}�b�v�T�C�Y������������ƃR���W���������������Ȃ���΂Ȃ�Ȃ��̂�
        // �\�����Ă���̈�̐��̑����Ɨ̈掩�̂̑傫���Œ���
        // 1...���Ȃ�(�̈掩�̂̕\���������Ȃ��A�̈掩�̂��傫��)
        // 2...����
        // 3...����(�̈掩�̂̕\�����������A�̈掩�̂�������)
        [SerializeField, Range(1, 3)]
        private int m_landSize = 1;
        public int LandSize => m_landSize;

        // ���̐�
        // �����̍����܂߂����E�ɏ����z�u����Ă��鍑�̐�
        // �g�D�̐��̕����ǂ�����
        // �ő吔�͑嗤�̑傫���ŕω�������K�v������
        [SerializeField, Range(2, 20)]
        private int m_kingdomCount = 2;
        public int KingdomCount => m_kingdomCount;

        // �����̗̈�̐�
        [SerializeField, Range(1, 3)]
        private int m_occupiedTerritoryCount = 1;
        public int OccupiedTerritoryCount => m_occupiedTerritoryCount;

        // ���̖��O
        [SerializeField]
        private string m_kingdomName = "";
        public string KingdomName => m_kingdomName;

        // �����̐F
        [SerializeField]
        private Color m_kingdomColor;
        public Color KingdomColor => m_kingdomColor;

        /// <summary>
        /// �������牺�� SPUM �ō쐬�\��
        /// SPUM �Ń����_���쐬�����ۂɂ��Ԃ� ID �����邾�낤���炻���ۑ�
        /// </summary>

        // ���[�_�[�̔��^
        // ���[�_�[�̔��F
        // ���[�_�[�̑̑傫��

        // ���[�_�[��

        // ���m�̔��^
        // ���m�̔��F
        // ���m�̑̑傫��

        // �I���N��
        [SerializeField]
        private int m_gameFinishYear = 100;
        public int GameFinishYear => m_gameFinishYear;

        [Title("�Q�[����Őݒ�\", horizontalLine: false)]

        [Title("�o�����X�ݒ�")]

        // �̈�ɏo������n��̑�����(�Z���̐�+-)
        [SerializeField, Range(0, 2)]
        private int m_areaIncDecValue = 1;
        public int AreaIncDecValue => m_areaIncDecValue;

        [Title("���ƈ�ՂƓ��A�A���킹��100%", horizontalLine: false)]
        // �̈�̒n��ɐݒ肳��钬�̊���
        [SerializeField, Range(0, 100)]
        private int m_areaTownRate = 70;
        public int AreaTownRate => m_areaTownRate;

        // �̈�̒n��ɐݒ肳����Ղ̊���
        [SerializeField, Range(0, 100)]
        private int m_areaRemainsRate = 10;
        public int AreaRemainsRate => m_areaRemainsRate;

        // �̈�̒n��ɐݒ肳��铴�A�̊���
        [SerializeField, Range(0, 100)]
        private int m_areaCaveRate = 10;
        public int AreaCaveRate => m_areaCaveRate;

        // �n��ɐݒ肳��鐶�Y�����A�C�e���̍ő�l
        [SerializeField, Range(1, 5)]
        private int m_maxAreaProductionResourceItem = 3;
        public int MaxAreaProductionResourceItem => m_maxAreaProductionResourceItem;

        // �n��ɐݒ肳�����̍Œ�l
        [SerializeField, Range(1, 9)]
        private int m_minZoneValue = 5;
        public int MinZoneValue => m_minZoneValue;

        // �n��ɐݒ肳�����̍ő�l
        [SerializeField, Range(1, 9)]
        private int m_maxZoneValue = 9;
        public int MaxZoneValue => m_maxZoneValue;

        [Title("�n�`�̕��z")]

        // ���������z����銄��
        [SerializeField, Range(0, 100)]
        private int m_distributionRatioPlane = 80;
        public int DistributionRatioPlane => m_distributionRatioPlane;

        // �R�����z����銄��
        [SerializeField, Range(0, 100)]
        private int m_distributionRatioMountain = 20;
        public int DistributionRatioMountain => m_distributionRatioMountain;

        // �X�����z����銄��
        [SerializeField, Range(0, 100)]
        private int m_distributionRatiofForest = 30;
        public int DistributionRatioForest => m_distributionRatiofForest;

        // �삪���z����銄��
        [SerializeField, Range(0, 100)]
        private int m_distributionRatioRiver = 5;
        public int DistributionRatioRiver => m_distributionRatioRiver;

        // �C�ɖʂ��Ă��邤���ŊC���ʂ��Ă���ł��낤����
        [SerializeField, Range(0, 100)]
        private int m_distributionRatioOcean = 75;
        public int DistributionRatioOcean => m_distributionRatioOcean;

        [Title("�A�C�e�����A���e�B")]

        // �R����
        [SerializeField, Range(0, 100)]
        private int m_itemRarityRateCommon = 57;
        public int ItemRarityRateCommon => m_itemRarityRateCommon;

        // �A���R����
        [SerializeField, Range(0, 100)]
        private int m_itemRarityRateUncommon = 28;
        public int ItemRarityRateUncommmon => m_itemRarityRateUncommon;

        // ���A
        [SerializeField, Range(0, 100)]
        private int m_itemRarityRateRare = 10;
        public int ItemRarityRateRare => m_itemRarityRateRare;

        // �G�s�b�N
        [SerializeField, Range(0, 100)]
        private int m_itemRarityRateEpic = 4;
        public int ItemRarityRateEpic => m_itemRarityRateEpic;

        // ���W�F���_���[
        [SerializeField, Range(0, 100)]
        private int m_itemRarityRateLegendary = 1;
        public int ItemRarityRateLegendary => m_itemRarityRateLegendary;

        [Title("�Q�[����Őݒ�s��")]

        // ���E�̖��O
        // �Q�[���J�n���ɖ��񌈒�(����̂�)
        [SerializeField]
        private string m_worldName;
        public string WorldName => m_worldName;

        // ���ȊO�̓y�n�̐F
        [SerializeField]
        private Color m_landColor;
        public Color LandColor => m_landColor;

    }
}