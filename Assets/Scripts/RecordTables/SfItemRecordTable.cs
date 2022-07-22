using UnityEngine;
using System;

using Sirenix.OdinInspector;

namespace sfproj
{
    public enum eItemCategory
    {
        ProductionResource,
        ProcessedGoods,
    }

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

    public enum eProcessdGoodsCategory
    { 
        None        = 0,
        // �H��(�����A�ʎ��A��؁A�����琶��)
        Food        = 100,
        // ����(�΁A�z���A�؁A�炩�琶��)
        Equipment   = 200,
        // ����(�΁A�z���A�؁A�炩�琶��)
        Arms        = 300,
        // ���[��(�΂��琶��)
        Rune        = 400,
    }

    [SerializeField]
    public class SfItemRecord
    {
        // �A�C�R��
        [SerializeField, HideLabel, PreviewField(55), HorizontalGroup(55, LabelWidth = 67)]
        private Texture m_icon = null;

        /// <summary>
        /// ����
        /// ��ƂȂ閼��
        /// �Q�[���J�n�����E���������ꂽ���_�ł��̒n��Ő��Y����鐶�Y�����͌��肳���
        /// ���̍ۂɂ��̖��O�������ł������ꍇ�A�`�����Ƃ������̂ɕω����ĕۑ������
        /// �Ⴆ�΃��C���b�N���������Y�����ꍇ�A
        /// �}�[�P�b�g�����݂���ƃ��C���b�N�p���A�������̓��C���b�N�p�X�^�����Y�����
        /// </summary>
        [SerializeField]
        private string m_name;

        // ���Y���� ID
        [SerializeField]
        private uint m_id = 0;
        public uint Id => m_id;

        // ���Y�����摜
        [SerializeField]
        private Sprite m_sprite = null;
        public Sprite Sprite => m_sprite;

        // ������
        [SerializeField]
        private string m_desc = null;
        public string Desc => m_desc;
    }

    /// <summary>
    /// ���Y����
    /// </summary>
    [Serializable]
    public class SfProductionResourceRecord : SfItemRecord
    {
        [SerializeField]
        private eProductionResouceCategory m_category = eProductionResouceCategory.None;
        public eProductionResouceCategory Category => m_category;
    }

    /// <summary>
    /// ���H�i
    /// </summary>
    [SerializeField]
    public class SfProcessdGoodsRecord : SfItemRecord
    {
        [SerializeField]
        private eProcessdGoodsCategory m_category = eProcessdGoodsCategory.None;
        public eProcessdGoodsCategory Category => m_category;
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