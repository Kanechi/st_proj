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
        Citadel     = 6000,
    }

    /// <summary>
    /// ���f�[�^
    /// �n��ɐݒ肷����
    /// ���̏����ׂ����ݒ肷�邱�Ƃł��̒n�悩�瓾���鎑�����ω�����
    /// </summary>
    public class ZoneData
    {
        private uint m_id = 0;

        private eZoneType m_zoneType = eZoneType.None;

        // �]�[���^�C�v�̐ݒ�
        public void SetZone(eZoneType zoneType) => m_zoneType = zoneType;
    }






    /// <summary>
    /// �n��f�[�^
    /// </summary>
    public class AreaData
    {
        private uint m_id = 0;

        // �ő��搔
        private int m_maxZoneCount = 0;

        // ���f�[�^���X�g
        private List<ZoneData> m_zoneDataList = new List<ZoneData>();

        // true...�����Q�g���|���X
        private bool m_lemegetonPorisFlag = false;
    }

    /// <summary>
    /// �̈�f�[�^
    /// </summary>
    public class DominionData
    {
        // �n�� ID
        private uint m_id = 0;

        // �n�於
        private string m_regionName = "";

        // true...��s
        private bool m_capitalFlag = false;

        // true...���n�ɗא�
        private bool m_adjacentPlaneFlag = false;

        // true...�X�ɗא�
        private bool m_adjacentForestFlag = false;

        // true...�C�ɗא�
        private bool m_adjacentSeaFlag = false;

        // true...�R�ɗא�
        private bool m_adjacentMountainFlag = false;

        // true...��ɗא�
        private bool m_adjacentRiverFlag = false;

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

        //public 
    }
}