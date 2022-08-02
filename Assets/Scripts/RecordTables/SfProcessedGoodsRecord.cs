using UnityEngine;
using System;

using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace sfproj
{
    public enum eProcessdGoodsCategory
    {
        None = 0,

        // �H��(�����A�ʎ��A��؁A�����琶��)
        Food = 10000,

        // ����(�z���A�؁A�炩�琶��)
        Equipment = 20000,

        // ����(�z���A�؁A�炩�琶��)
        Arms = 30000,

        // ���[��(�΂��琶��)
        Rune = 40000,
    }



    /// <summary>
    /// ���H�i
    /// </summary>
    [Serializable]
    public class SfProcessedGoodsRecord
    {
        /// <summary>
        /// ���H�i���쐬����ۂɕK�v�Ȑ��Y�����Ƃ��̔䗦
        /// </summary>
        [Serializable]
        public class SfResourceRatio
        {

            // ���Y���� ID
            [SerializeField]
            private uint m_id = 0;
            public uint Id => m_id;

            // �䗦
            [SerializeField, Range(0,100)]
            private float m_ratio = 0.0f;
            public float Ratio => m_ratio;
        }

        // �A�C�R��
        [SerializeField, HideLabel, PreviewField(55), HorizontalGroup(55, LabelWidth = 67)]
        protected Sprite m_icon = null;
        public Sprite Sprite => m_icon;

        // ���Y���� ID
        [SerializeField]
        private uint m_id = 0;
        public uint Id => m_id;

        [SerializeField]
        private eProcessdGoodsCategory m_category = eProcessdGoodsCategory.None;
        public eProcessdGoodsCategory Category => m_category;

        /// <summary>
        /// ��ƂȂ閼��
        /// �Q�[���J�n�����E���������ꂽ���_�ł��̒n��Ő��Y����鐶�Y�����͌��肳���
        /// ���̍ۂɂ��̖��O�������ł������ꍇ�A�`�����Ƃ������̂ɕω����ĕۑ������
        /// �Ⴆ�΃��C���b�N���������Y�����ꍇ�A
        /// �}�[�P�b�g�����݂���ƃ��C���b�N�p���A�������̓��C���b�N�p�X�^�����Y�����
        /// </summary>
        [SerializeField]
        private string m_baseName;
        public string BaseName => m_baseName;    

        // ������
        [SerializeField]
        private string m_desc = null;
        public string Desc => m_desc;

        /// <summary>
        /// �䗦
        /// ���X�g�̂O�Ԃ���~��
        /// ������ 100% �ɂȂ�悤�ɐ݌v���邱��
        /// 
        /// </summary>
        [SerializeField]
        private List<SfResourceRatio> m_resourceRatio = null;
        public List<SfResourceRatio> ResourceRatio => m_resourceRatio;
    }
}