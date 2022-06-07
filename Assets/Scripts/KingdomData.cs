using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// ���^�C�v
    /// </summary>
    public enum eZoneType {
        None = -1,

        /// <summary>
        /// �_�Ƌ�
        /// ���n�ɗאڂ��Ă���Ό��݉\
        /// �H����������������
        /// </summary>
        Agriculture     = 1000,

        /// <summary>
        /// ���Ƌ�
        /// ���܂��܂ȃA�C�e�����g���[�h�\�ɂȂ�
        /// ���̓��Y�i�Ȃǂ�������
        /// </summary>
        Commercial      = 2000,

        /// <summary>
        /// �z�Ƌ�
        /// �R�ɗאڂ��Ă���Ό��݉\
        /// �z��������������
        /// </summary>
        MiningIndustry  = 3000,

        /// <summary>
        /// ���̒n
        /// �X�ɗאڂ��Ă���Ό��݉\
        /// �ޖ؎�����������
        /// </summary>
        LoggingArea     = 4000,

        /// <summary>
        /// ������
        /// �����n�̃A�C�e���N���t�g�𐶐��\�ɂȂ�
        /// ���@�h��͂����グ�鎖���\
        /// </summary>
        Witchcrafty     = 5000,

        /// <summary>
        /// ��ǋ�
        /// ���̈���̖h��͂��グ�邱�Ƃ��\
        /// �ǂƂ͈Ⴄ
        /// �ǂ͂܂��ʓr��ǂƂ��Č��݉\
        /// </summary>
        Citadel         = 6000,
    }

    /// <summary>
    /// �n��^�C�v
    /// </summary>
    public enum eAreaType
    { 
        None = -1,

        // �s�s
        Town            = 1000,

        // ��s
        Capital         = 1100,

        // �v��
        StrongHold      = 1200,

        // �����Q�g���|���X
        LemegetonPoris  = 1300,


        // ���
        Remains         = 2000,

        // ���A
        Cave            = 3000,
    }

    /// <summary>
    /// �n��f�[�^
    /// </summary>
    public class AreaData
    {
        private uint m_id = 0;

        // �ő��搔
        private int m_maxZoneCount = 0;

        // �n��^�C�v
        private eAreaType m_areaType = eAreaType.None;

        // ���^�C�v���X�g
        private List<eZoneType> m_zoneTypeList = new List<eZoneType>();
    }



    /// <summary>
    /// �̈�f�[�^
    /// tgs �̗̈���^�b�`�����ۂ̏��
    /// </summary>
    public class DominionData
    {
        // �̈� ID
        private uint m_id = 0;

        // �̈於
        private string m_regionName = "";

        // true...�����ς�  false...������
        private bool m_ruleFlag = false;

        // true...��s(�{���n�A��)
        private bool m_capitalFlag = false;

        // true...�א�
        private uint m_adjacentFlag = 0;


        // �̒n�n��f�[�^���X�g
        private List<AreaData> m_areaDataList = new List<AreaData>();
    }


    /// <summary>
    /// ����1���̃f�[�^
    /// </summary>
    public class KingdomData
    {
        // ���� ID
        public uint m_id = 0;

        // �����̒n���X�g
        public List<DominionData> m_kingdomDominionDataList = new List<DominionData>();
    }

    public class KingdomDataManager : Singleton<KingdomDataManager>
    {
        private List<KingdomData> m_kingdomDataList = new List<KingdomData>();
        public List<KingdomData> KingdomDataList => m_kingdomDataList;

    }

    /// <summary>
    /// �����f�[�^�H��
    /// </summary>
    public class KingdomDataFactory {

        public KingdomData Create(int territoryIndex, Color color) {

            var kingdomData = new KingdomData();

            // ���O�̐ݒ�

            // 

            return kingdomData;
        }
    }
}