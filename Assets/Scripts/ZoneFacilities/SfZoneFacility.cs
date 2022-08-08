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
        public uint FacilityTypeId { get => m_facilityTypeId; }

        // ���{�݂̃^�C�v
        public eZoneFacilityCategory m_facilityCategory = eZoneFacilityCategory.None;
        public eZoneFacilityCategory FacilityCategory { get => m_facilityCategory; }

        // �g����
        public int m_expantionCount = 0;
        public int ExpantionCount { get => m_expantionCount; set => m_expantionCount = value; }

        public void Parse(IDictionary<string, object> data)
        {
            data.Get(nameof(m_id), out m_id);
            data.Get(nameof(m_areaId), out m_areaId);
            data.Get(nameof(m_cellIndex), out m_cellIndex);
            data.Get(nameof(m_facilityTypeId), out m_facilityTypeId);
            data.GetEnum(nameof(m_facilityCategory), out m_facilityCategory);
            data.Get(nameof(m_expantionCount), out m_expantionCount);


            // �{�݂̋@�\�������Ń^�C�v�𒲂ׂĐ�������
            SetFacilityData(m_facilityTypeId, m_facilityCategory);
        }

        // �{�݂̋@�\
        public SfZoneFacilityAbility Ability { get; private set; } = null;
        public SfZoneFacilityAbility NextAbility { get; private set; } = null;

        /// <summary>
        /// �{�݃f�[�^�̐ݒ�
        /// �ݒ�Ɠ����Ɏ{�݋@�\���ݒ�
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        public void SetFacilityData(uint id, eZoneFacilityCategory category)
        {
            m_facilityTypeId = id;
            m_facilityCategory = category;

            NextAbility = SfZoneFacilityActiveAbilityTableManager.Instance.Table.Get(m_facilityTypeId, m_facilityCategory);

            if (NextAbility == null)
            {
                // �{�݂�j�󂵂��ۂɂ����ɓ����Ă���̂ŃG���[�ł͂Ȃ�
                // ID ������ɂ��ւ�炸�����ɓ����Ă�����G���[
                if (m_facilityTypeId != 0)
                {
                    Debug.LogWarning("id != 0. Ability == null !!!");
                }
                else
                {
                    // �j�󏈗�
                    Ability = null;
                }
            }
        }

        public void Update()
        {
            if (NextAbility != null)
            {
                Ability?.OnRemove(this);

                Ability = NextAbility;

                NextAbility = null;

                Ability?.OnSetup(this);
            }

            Ability?.OnUpdate(0.0f, this);
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
        /// 
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
            zoneFacility.SetFacilityData(typeId, category);
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
            facility.SetFacilityData(typeId, category);
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
            Get(areaId, cellIndex).SetFacilityData(0, eZoneFacilityCategory.None);
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

            // ��U switch �ł���Ă݂邩

            if (RecordList.Count == 0)
                return;

            foreach (var facility in RecordList)
            {
                if (facility.FacilityTypeId != 0)
                {

                    // ���Ԑݒ肪�K�v
                    facility.Update();
                }
            }

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