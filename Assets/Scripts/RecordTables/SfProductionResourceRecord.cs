using UnityEngine;
using System;

using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace sfproj
{ 

    public enum eProductionResouceCategory
    {
        None        = 0,
        // ��
        Wood        = 100,
        // ��
        Stone       = 200,
        // �z��
        Mineral     = 300,
        // ����
        Grain       = 400,
        // �ʎ�
        Fruit       = 500,
        // ���
        Vegetable   = 600,
        // ��
        Meat        = 700,
        // ��A�畆�A���A�\��A�є�
        Skin        = 800,
    }



    /// <summary>
    /// ���Y����
    /// </summary>
    [Serializable]
    public class SfProductionResourceRecord
    {
        // ���Y�����摜
        [SerializeField, HideLabel, PreviewField(55), HorizontalGroup(55, LabelWidth = 67)]
        protected Sprite m_icon = null;
        public Sprite Sprite => m_icon;


        // ���Y���� ID
        [SerializeField]
        private uint m_id = 0;
        public uint Id => m_id;

        [SerializeField]
        private eProductionResouceCategory m_category = eProductionResouceCategory.None;
        public eProductionResouceCategory Category => m_category;

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
        [SerializeField, Multiline(3)]
        private string m_desc = null;
        public string Desc => m_desc;

    }



#if false
    /// <summary>
    /// ���̕����𐶎Y�����ƂɃt�@�C���������ʎY����� Unity �ŊǗ����y�ɂȂ�
    /// </summary>
    [CreateAssetMenu(menuName = "RecordTables/Create SfProductionResourceTable", fileName = "SfProductionResourceTable", order = 10001)]
    public class SfProductionResourceTable : EditorRecordTable<SfProductionResource>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/SfProductionResourceTable";
        // singleton instance
        protected static SfProductionResourceTable s_instance = null;
        // singleton getter
        public static SfProductionResourceTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProductionResourceTable);
        // get record
        public override SfProductionResource Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
#endif



    public class SfProductionResourceTableManager : Singleton<SfProductionResourceTableManager>
    {
        
    }
}