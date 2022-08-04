using UnityEngine;
using System;

using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace sfproj
{ 

    /// <summary>
    /// ���Y�����J�e�S��
    /// </summary>
    public enum eProductionResouceCategory : uint
    {
        None        = 0,
        // ����(�C�l�A�g�E�����R�V�A���Ȃ�)
        Grain       = 110000,
        // �z��(�΁A���A�S�Ȃ�)
        Mineral     = 120000,
        // �����X�^�[�f��(��A�畆�A���A�\��A�є�A���A�܂Ȃ�)
        Monster     = 130000,
        // �A���f��(��؁A�ʎ��Ȃ�)
        Plant       = 140000,
        // ��(�X�M�̖؁A�~�̖؂Ȃ�)
        Wood        = 150000,
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

        // ���Y�����J�e�S��
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
        private List<string> m_baseNameList;
        public List<string> BaseNameList => m_baseNameList;
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
        public SfProductionResourceRecord Get(uint id) {

            if (id >= (uint)eProductionResouceCategory.Grain && id < (uint)eProductionResouceCategory.Mineral)
            {
                return SfProductionResourceGrainRecordTable.Instance.Get(id);
            }
            else if (id >= (uint)eProductionResouceCategory.Mineral && id < (uint)eProductionResouceCategory.Monster)
            {
                return SfProductionResourceMineralRecordTable.Instance.Get(id);
            }
            else if (id >= (uint)eProductionResouceCategory.Monster && id < (uint)eProductionResouceCategory.Plant)
            {
                return SfProductionResourceMonsterRecordTable.Instance.Get(id);
            }
            else if (id >= (uint)eProductionResouceCategory.Plant && id < (uint)eProductionResouceCategory.Wood)
            {
                return SfProductionResourcePlantRecordTable.Instance.Get(id);
            }
            else if (id >= (uint)eProductionResouceCategory.Wood)
            {
                return SfProductionResourceWoodRecordTable.Instance.Get(id);
            }

            return null;
        }
    }
}