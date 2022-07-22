using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Sirenix.OdinInspector;

namespace sfproj
{
    /// <summary>
    /// ���^�C�v
    /// stellaris �͓d�C�A�H�ƁA�_�ƁA�Y�Ƌ���ʘg�ŐU�蕪�����悤�ɂȂ��Ă��邪
    /// ��������ׂĐ��g�Ƃ��Ĉ����悤�Ȋ���
    /// ���p�ł���y�n�������Ă����ɋ���U�蕪����Ƃ����l������
    /// </summary>
    public enum eZoneFacilityType
    {
        /// <summary>
        /// �����ݒ肳��Ă��Ȃ�
        /// �j�󂵂��ۂ�����ɐݒ�
        /// </summary>
        None = -1,

 
        /// <summary>
        /// �_��(���Y)
        /// ���n�ɗאڂ��Ă���Ό��݉\
        /// ����I�ɐH��������
        /// </summary>
        Production_Farm = 1000,

        /// <summary>
        /// �ʎ���(���Y)
        /// �X�ɗאڂ��Ă���Ό��݉\
        /// ����I�ɐH��������
        /// </summary>
        Production_Orchard = 1010,

        /// <summary>
        /// ����(����)
        /// �삩�C�ɗאڂ��Ă���Ό��݉\
        /// ����I�ɐH��������
        /// </summary>
        Production_Fishery = 1020,

        /// <summary>
        /// �̌@��(���Y)
        /// �R�ɗאڂ��Ă���Ό��݉\
        /// ����I�ɍz������������
        /// �������鎑���͂��̎R�ɖ�������Ă���z���ɂ���ĕω�
        /// �΁A���A�S�A�~�X�����A�I���n���R��
        /// �g������ۂɂǂ̍z�����d�_�I�ɍ̌@���邩��I���ł���悤�ɂ���H
        /// </summary>
        Production_Mining = 1100,

        /// <summary>
        /// ���̏�(���Y)
        /// �X�ɗאڂ��Ă���Ό��݉\
        /// ����I�ɍޖ؎���������
        /// �������鎑���͂��̐X�ɐ����Ă���؂ɂ���ĕω�
        /// �I�[�N���A���A���b�h�E�b�h
        /// </summary>
        Production_LoggingArea = 1200,




        /// <summary>
        /// ����(����)
        /// �ǂ��ł����݉\
        /// �}�[�P�b�g�{�^�������p�\�ɂȂ�
        /// �n��Ŏ�ɓ��鎑��������(����or���)�ł���
        /// </summary>
        Commercial_MarketPlace = 2000,

        /// <summary>
        /// ����(���ƁA��)
        /// ���n�ɗאڂ��Ă���Ό��݉\
        /// �A���{�^�������p�\�ɂȂ�(�n��Ŏ�ɓ��鎑���������̕ʂ̒n��ɗA��(����or���)���\�ɂȂ�)
        /// �g���[�h�{�^�������p�\�ɂȂ�(�n��Ŏ�ɓ��鎑����אڂ��鑼���̕ʂ̒n��ƕ��X����(����or���)�\�ɂȂ�)
        /// �n��Ŏ�ɓ��鎑����
        /// </summary>
        Commercial_TradingPost = 2010,

        /// <summary>
        /// �`(���ƁA�C)
        /// �삩�C�ɗאڂ��Ă���Ό��݉\
        /// �A���{�^�������p�\�ɂȂ�(�n��Ŏ�ɓ��鎑���������̕ʂ̒n��ɗA��(����or���)���\�ɂȂ�)
        /// �g���[�h�{�^�������p�\�ɂȂ�(�n��Ŏ�ɓ��鎑�����`�̂��鑼���̕ʂ̒n��ƕ��X����(����or���)�\�ɂȂ�)
        /// </summary>
        Commercial_Harbor = 2020,



        /// <summary>
        /// �Z��(���Z��)
        /// �ő�l��������
        /// </summary>
        Residential_House = 3000,


        /// <summary>
        /// �M���h(�R�����)
        /// ��Ղ⓴�A�̒������\�ȁA�����{�^�������p�\
        /// �����n��ł���Β������\
        /// </summary>
        Military_Guild = 4000,

        /// <summary>
        /// ���
        /// �h�q���A�h��͂��㏸
        /// </summary>
        Military_CityWall = 4100,




        /// <summary>
        /// ���[���F(������)
        /// ���[�������󂳂ꂽ�΂�Z�����ă}�i�𓾂�ׂ̘F
        /// ����I�Ƀ}�i�𑝉�
        /// </summary>
        Witchcrafty_RuneFurnace = 5000,

        /// <summary>
        /// ���@��(������)
        /// �h�q���A���@�h��͂��㏸
        /// </summary>
        Witchcrafty_MagicBarrier = 5100,
    }

    /// <summary>
    /// ���{�݃��R�[�h
    /// </summary>
    [Serializable]
    public class SfZoneFacilityRecord
    {
        // �A�C�R��
        [SerializeField, HideLabel, PreviewField(55), HorizontalGroup(55, LabelWidth = 67)]
        private Texture m_icon = null;

        // ���O
        [SerializeField]
        private string m_name;

        // ���{�݃^�C�v
        [SerializeField]
        private eZoneFacilityType m_zoneFacilityType = eZoneFacilityType.None;
        public eZoneFacilityType Type => m_zoneFacilityType;

        // �{�݉摜
        [SerializeField]
        private Sprite m_facilitySprite = null;
        public Sprite FacilitySprite => m_facilitySprite;

        // �R�X�g(���Y����ID,�K�v��)
        [SerializeField]
        private Dictionary<uint, int> m_costs;
        public Dictionary<uint, int> Costs => m_costs;

        // ������
        [SerializeField, Multiline(3)]
        private string m_desc = "";
        public string Description => m_desc;
    }

    [CreateAssetMenu(menuName = "RecordTables/Create SfZoneFacilityRecordTable", fileName = "SfZoneFacilityRecordTable", order = 10000)]
    public class SfZoneFacilityRecordTable : EditorRecordTable<SfZoneFacilityRecord>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/SfZoneFacilityRecordTable";
        // singleton instance
        protected static SfZoneFacilityRecordTable s_instance = null;
        // singleton getter 
        public static SfZoneFacilityRecordTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfZoneFacilityRecordTable);
        // get record
        public override SfZoneFacilityRecord Get(uint id) => m_recordList.Find(r => r.Type == (eZoneFacilityType)id);
        public SfZoneFacilityRecord Get(eZoneFacilityType id) => m_recordList.Find(r => r.Type == id);
    }
}