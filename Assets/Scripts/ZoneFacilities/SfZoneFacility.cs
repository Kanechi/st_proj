using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{
    /// <summary>
    /// ���{��
    /// ���{�݂݂̂��Ǘ�
    /// ���ɑ��݂���ǉ��{�^���⃍�b�N�{�^���Ȃǂ͊Ǘ����Ă��Ȃ�
    /// </summary>
    public class SfZoneFacility : IJsonParser
    {
        // ���j�[�N ID
        public uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // �n�� ID
        public uint m_areaId = 0;
        public uint AreaId { get => m_areaId; set => m_areaId = value; }

        // ���Z���C���f�b�N�X
        public int m_cellIndex = -1;
        public int CellIndex { get => m_cellIndex; set => m_cellIndex = value; }

        public uint m_facilityTypeId = 0;
        public uint FacilityTypeId { get => m_facilityTypeId; set => m_facilityTypeId = value; }

        // ���{�݂̃^�C�v
        public eZoneFacilityCategory m_facilityType = eZoneFacilityCategory.None;
        public eZoneFacilityCategory FacilityType { get => m_facilityType; set => m_facilityType = value; }

        // �g����
        public int m_expantionCount = 0;
        public int ExpantionCount { get => m_expantionCount; set => m_expantionCount = value; }

        public void Parse(IDictionary<string, object> data)
        {
            data.Get(nameof(m_id), out m_id);
            data.Get(nameof(m_areaId), out m_areaId);
            data.Get(nameof(m_cellIndex), out m_cellIndex);
            data.Get(nameof(m_facilityTypeId), out m_facilityTypeId);
            data.GetEnum(nameof(m_facilityType), out m_facilityType);
            data.Get(nameof(m_expantionCount), out m_expantionCount);
        }
    }

    /// <summary>
    /// ���e�[�u��
    /// �{�݂��������炱���Ɏ��t����
    /// ���t�����Ă���{�݂͒���I�Ɏ����𐶎Y����
    /// </summary>
    public class SfZoneFacilityTable : RecordTable<SfZoneFacility>
    {
        // �o�^
        public void Regist(SfZoneFacility record) => RecordList.Add(record);

        // ���{�݃��R�[�h�̎擾
        public override SfZoneFacility Get(uint id) => RecordList.Find(r => r.Id == id);
        public SfZoneFacility Get(uint areaId, int cellIndex) => RecordList.Find(r => r.AreaId == areaId && r.CellIndex == cellIndex);

        // ���{�݂̎��O��
        public void Remove(SfZoneFacility record) => RecordList.Remove(record);
        public void Remove(uint areaId, int cellIndex) => Remove(Get(areaId, cellIndex));


        /// <summary>
        /// ���{�݂̏��񌚐�
        /// </summary>
        /// <param name="areaId">�n�� ID</param>
        /// <param name="cellIndex">���Z���C���f�b�N�X�ԍ�</param>
        /// <param name="type">���݂�����{�݃^�C�v</param>
        /// <param name="exp">�g����</param>
        public void BuildZoneFacilityType(uint areaId, int cellIndex, uint typeId, eZoneFacilityCategory category, int exp)
        {
            var zoneFacility = new SfZoneFacility();
            zoneFacility.Id = SfConstant.CreateUniqueId(ref SfZoneFacilityTableManager.Instance.m_uniqueIdList);
            zoneFacility.AreaId = areaId;
            zoneFacility.CellIndex = cellIndex;
            zoneFacility.m_facilityTypeId = typeId;
            zoneFacility.FacilityType = category;
            zoneFacility.ExpantionCount = exp;

            Regist(zoneFacility);
        }

        /// <summary>
        /// ���{�݂̕ύX
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="cellIndex"></param>
        /// <param name="type"></param>
        public void ChangeZoneFacilityType(uint areaId, int cellIndex, uint typeId, eZoneFacilityCategory category)
        {
            var facility = Get(areaId, cellIndex);
            facility.m_facilityTypeId = typeId;
            facility.FacilityType = category;
        }

        /// <summary>
        /// ���{�݂�j��
        /// None �ɖ߂�
        /// ��ʏ�ł̓v���X�{�^���ɕς��
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="cellIndex"></param>
        public void DestroyZonefacilityType(uint areaId, int cellIndex)
        {
            Get(areaId, cellIndex).FacilityType = eZoneFacilityCategory.None;
        }

        /// <summary>
        /// ���{�݂̊g�������グ��
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="cellIndex"></param>
        /// <param name="exp"></param>
        public void IncreaseZoneFacilityExpantion(uint areaId, int cellIndex, int exp)
        {
            var zoneFacility = Get(areaId, cellIndex);
            if (zoneFacility.ExpantionCount >= SfConfigController.ZONE_MAX_EXPANTION_COUNT)
                return;
            zoneFacility.ExpantionCount += exp;
        }

        /// <summary>
        /// ���{�݂̊g�����̐ݒ�
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="cellIndex"></param>
        /// <param name="exp"></param>
        public void SetZoneFacilityExpantion(uint areaId, int cellIndex, int exp)
        {
            var zoneFacility = Get(areaId, cellIndex);
            if (zoneFacility.ExpantionCount >= SfConfigController.ZONE_MAX_EXPANTION_COUNT)
                return;
            if (exp > SfConfigController.ZONE_MAX_EXPANTION_COUNT || exp < 0)
                return;
            zoneFacility.ExpantionCount = exp;
        }

        /// <summary>
        /// �X�V����
        /// ���������̎{�݂����t�����Ă����炱���Ŏ����̑������s��
        /// </summary>
        public void Update() { 
        
            // �{�݂��Ƃ̏������s���A�ǂ̒n��ɑ΂��ĉ����s����

            // ���Y�����{�݂ł���Βn��ɂ��鐶�Y�����𑝉����A�{�݂����݂���Ă���n��̑q�ɂɕۊǂ���
            
            // �K�v�Ȃ���C���f�b�N�X�ɐ��Y�J�E���g�_�E����\������
        }
    }

    /// <summary>
    /// ���e�[�u���Ǘ�
    /// </summary>
    public class SfZoneFacilityTableManager : Singleton<SfZoneFacilityTableManager>
    {
        // ���j�[�N ID ���X�g
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        private SfZoneFacilityTable m_table = new SfZoneFacilityTable();
        public SfZoneFacilityTable Table => m_table;

        // ���R�[�h���X�g���N���A
        public void Clear() => m_table.RecordList.Clear();

        // �ǂݍ���
        public void Load()
        {
            var director = new RecordTableESDirector<SfZoneFacility>(new ESLoadBuilder<SfZoneFacility, SfZoneFacilityTable>("SfZoneDataTable"));
            director.Construct();
            if (director.GetResult() != null && director.GetResult().RecordList.Count > 0)
                m_table.RecordList.AddRange(director.GetResult().RecordList);
        }

        /// <summary>
        /// �ۑ�����
        /// </summary>
        public void Save()
        {
            var director = new RecordTableESDirector<SfZoneFacility>(new ESSaveBuilder<SfZoneFacility>("SfZoneDataTable", m_table));
            director.Construct();
        }
    }
}