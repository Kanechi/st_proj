using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace sfproj {
    public class ConfigController : SingletonMonoBehaviour<ConfigController> {

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


        /// <summary>
        /// ���ȊO�̓y�n�̐F
        /// </summary>
        [SerializeField]
        private Color m_landColor;
        public Color LandColor => m_landColor;

    }
}