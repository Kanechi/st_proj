using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stproj
{
    /// <summary>
    /// �]�[���^�C�v
    /// </summary>
    public enum eZoneType {
        None,
        // ��
        Castle          = 1000,
        // �̎�̏�
        CastleOfLord    = 1001,

        // ���̏�
        LoggingArea     = 2000,

        // �̌@��
        Quarry          = 3000,

        // �n���^�[����
        HunterHut       = 4000,

        // ���`
        FishingPort     = 5000,
        // �f�Ս`
        TradePort       = 5001,

        // �}�[�P�b�g
        Market          = 6000,
    }

    /// <summary>
    /// ���f�[�^
    /// </summary>
    public class ZoneData
    {
        private uint m_id = 0;

        private eZoneType m_zoneType = eZoneType.None;
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