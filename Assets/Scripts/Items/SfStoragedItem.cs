using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{
    /// <summary>
    /// �n��ɕۊǂ���Ă���A�C�e���P��ޕ�
    /// </summary>
    public class SfStoragedItem : IJsonParser
    {
        // �ۊǌɂ̃C���f�b�N�X(�オ�O)
        // �ۊǐ����Ȃ��Ȃ�����ۊǌɂ��������A�C���f�b�N�X���J�艺����
        public int m_index = 0;
        public int Index { get => m_index; set => m_index = value; }

        // ���Y���ꂽ�A�C�e����ID
        public uint m_itemId = 0;
        public uint ItemId { get => m_itemId; set => m_itemId = value; }

        // �ۊǐ�
        public int m_count = 0;
        public int Count { get => m_count; set => m_count = value; }

        // �ۊǂ���Ă���n��� ID
        public uint m_areaId = 0;
        public uint AreaId { get => m_areaId; set => m_areaId = value; }

        public SfStoragedItem() { }
        public SfStoragedItem(int index, uint itemId, int count, uint areaId)
        {
            m_index = index;
            m_itemId = itemId;
            m_count = count;
            m_areaId = areaId;
        }

        public void Parse(IDictionary<string, object> data)
        {
            
        }
    }

    /// <summary>
    /// �ۊǃA�C�e���e�[�u��
    /// </summary>
    public class SfStoragedItemTable : RecordTable<SfStoragedItem>
    {
        // ���g�p
        public override SfStoragedItem Get(uint id) => null;

        // �o�^
        public void Regist(SfStoragedItem record) => RecordList.Add(record);

        // �w��n��̈�ԑ傫���C���f�b�N�X�̐���Ԃ�
        public int GetMaxIndexByArea(uint areaId) {
            if (RecordList.Count == 0)
                return 0;

            var list = RecordList.Where(r => r.AreaId == areaId);

            if (list.Count() == 0)
                return 0;

            return list.Max(r => r.Index);
        }

        /// <summary>
        /// �A�C�e���𑝂₷
        /// </summary>
        /// <param name="areaId">�ǂ̒n���</param>
        /// <param name="itemId">�ǂ̃A�C�e����</param>
        /// <param name="value">������</param>
        public void IncreaseItem(uint areaId, uint itemId, int value)
        {
            SfStoragedItem item = null;
            if (RecordList.Count != 0)
            {
                item = RecordList.Find(r => r.AreaId == areaId && r.ItemId == itemId);
            }

            if (item == null)
            {
                int index = GetMaxIndexByArea(areaId);

                item = new SfStoragedItem(index, itemId, value, areaId);

                Regist(item);
            }
            else
            {
                item.Count += value;
                // �X�^�b�N(��ލő�l)�ȏ�ɂȂ�����؂�̂�
                // stellaris �Ȃ�A�C�e�����Ƃɍő�l������������
                // ���̃Q�[�����A�C�e���̎�ނ���������E�E�E
                // ���ǂ����ς���ɓ��ꂽ�����ЂƂ܂��ꗥ�� 999 ��
                if (item.Count > 999)
                    item.Count = 999;
            }
        }

        /// <summary>
        /// �����̓`�F�b�N���K�v����
        /// �A������ۂƂ����E�E�E�O�ȉ��͂O�ɂȂ�Œ�~���Ă����Ζ��Ȃ��H
        /// �A�������̑��̎{�݂ƈ����Ƃ��Ă͓������ȁH
        /// �g�p�����ۂɂO�ȉ��ɂȂ邱�ƂŋN����f�����b�g����������悤�ȍs�����N����Ȃ���Ζ��Ȃ�����
        /// �Ⴆ�Ύ{�݌��݂Ȃ񂩂͌��݃{�^�����������Ƃ��ĉ����Ȃ�����Ƃ��őΏ�
        /// 
        /// �ЂƂ܂����u
        /// </summary>
        /// <param name="areaid"></param>
        /// <param name="itemId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckDecreaseItem(uint areaId, uint itemId, int value)
        {
            return true;
        }

        /// <summary>
        /// �ЂƂ܂����邱�Ƃ͌�񂵂�
        /// 
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="itemId"></param>
        /// <param name="value"></param>
        public void DecreaseItem(uint areaId, uint itemId, int value)
        {
            
        }
    }

    public class SfStoragedItemTableManager : Singleton<SfStoragedItemTableManager>
    {
        private SfStoragedItemTable m_table = new SfStoragedItemTable();
        public SfStoragedItemTable Table => m_table;
    }
}