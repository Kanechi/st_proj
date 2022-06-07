using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stproj
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

        // ��
        Town,

        // ��
        Castle,

        // �v��
        StrongHold,

        // �����Q�g���|���X
        LemegetonPoris,
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

    public enum eAdjacentType : uint {
        // ���n�ɗא�
        Plane       = 1u << 0,
        // �X�ɗא�
        Forest      = 1u << 1,
        // �C�ɗא�
        Ocean       = 1u << 2,
        // �R�ɗא�
        Mountain    = 1u << 3,
        // ��ɗא�
        River       = 1u << 4,
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

        // true...�T���ς�  false...���T��
        private bool m_exploredFlag = false;

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
        private uint m_id = 0;

        // �����̒n���X�g
        private List<DominionData> m_kingdomDominionDataList = new List<DominionData>();
    }

    public class KingdomDataManager : Singleton<KingdomDataManager>
    {
        private List<KingdomData> m_kingdomDataList = new List<KingdomData>();
        public List<KingdomData> KingdomDataList => m_kingdomDataList;

    }
}