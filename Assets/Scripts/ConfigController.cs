using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace sfproj {
    public class ConfigController : SingletonMonoBehaviour<ConfigController> {

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

        // �̈�ɏo������n��̍Œ�l
        [SerializeField, Range(1, 9)]
        private int m_minAreaValue = 2;
        public int MinAreaValue => m_minAreaValue;

        // �̈�ɏo������n��̍ő�l
        [SerializeField, Range(1, 9)]
        private int m_maxAreaValue = 6;
        public int MaxAreaValue => m_maxAreaValue;

        [Title("���ƈ�ՂƓ��A�A���킹��100%", horizontalLine: false)]
        // �̈�̒n��ɐݒ肳��钬�̊���
        [SerializeField, Range(0, 100)]
        private int m_areaTownRate = 80;
        public int AreaTownRate => m_areaTownRate;

        // �̈�̒n��ɐݒ肳����Ղ̊���
        [SerializeField, Range(0, 100)]
        private int m_areaRemainsRate = 10;
        public int AreaRemainsRate => m_areaRemainsRate;

        // �̈�̒n��ɐݒ肳��铴�A�̊���
        [SerializeField, Range(0, 100)]
        private int m_areaCaveRate = 10;
        public int AreaCaveRate => m_areaCaveRate;

        // �n��ɐݒ肳�����̍Œ�l
        [SerializeField, Range(1, 20)]
        private int m_minZoneValue = 5;
        public int MinZoneValue => m_minZoneValue;

        // �n��ɐݒ肳�����̍ő�l
        [SerializeField, Range(1, 20)]
        private int m_maxZoneValue = 10;
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

        [Title("�Q�[����Őݒ�s��")]

        // ���ȊO�̓y�n�̐F
        [SerializeField]
        private Color m_landColor;
        public Color LandColor => m_landColor;

    }
}