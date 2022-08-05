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
    /// ���{�݃J�e�S��
    /// </summary>
    public enum eZoneFacilityCategory
    {
        None = 0,
        ProductionResource,
        ProcessedGoods,
#if false
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
#endif
    }

    /// <summary>
    /// �R�X�g
    /// </summary>
    [Serializable]
    public class SfCost
    {
        [SerializeField]
        private uint m_id = 0;
        public uint Id => m_id;

        [SerializeField]
        private int m_count = 0;
        public int Count => m_count;
    }

    /// <summary>
    /// ���{�݃��R�[�h
    /// </summary>
    [Serializable]
    public class SfZoneFacilityRecord
    {
        // �A�C�R��(�{�݉摜)
        [SerializeField, HideLabel, PreviewField(55), HorizontalGroup(55, LabelWidth = 67)]
        private Sprite m_icon = null;
        public Sprite FacilitySprite => m_icon;

        // ���O
        [SerializeField]
        private string m_name;

        // ���{�� ID (���Y�����{�݃^�C�v�A���H�i�{�݃^�C�v)
        [SerializeField]
        private uint m_zoneFacilityId = 0;
        public uint TypeId => m_zoneFacilityId;

        // ���{�݃^�C�v
        [SerializeField]
        private eZoneFacilityCategory m_zoneFacilityCategory = eZoneFacilityCategory.None;
        public eZoneFacilityCategory Category => m_zoneFacilityCategory;
        
        // ���݃R�X�g(���Y����ID,�K�v��)
        [SerializeField]
        private List<SfCost> m_costs = new List<SfCost>();
        public List<SfCost> Costs => m_costs;

        // ������
        [SerializeField, Multiline(3)]
        private string m_desc = "";
        public string Description => m_desc;

        // �����ł����{�A�C�e���̃J�e�S��
        [SerializeField]
        private uint m_genBaseItemCategory = 0;
        public uint GenBaseItemCategory => m_genBaseItemCategory;
    }
}