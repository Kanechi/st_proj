#define DEBUG_COSTCHECK

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using System.Linq;

namespace sfproj
{
    /// <summary>
    /// �n��J����
    /// </summary>
    public enum eAreaDevelopmentState
    {
        // ���J��
        Not,
        // �J��
        During,
        // �J��ς�
        Completed
    }

    /// <summary>
    /// �n��ɑ��݂���n�`
    /// �n�`�ɂ���ċ��ɐݒu�ł�����̂�g���[�h�Ŕ̔����Ă�����̂Ȃǂ��낢��Ȃ��̂��ω�����
    /// �C�̂ݗ̈悪�C�ɖʂ��Ă��邩�𔻒肵�Đݒ肷��
    /// </summary>
    public enum eExistingTerrain : uint
    {
        // ����
        Plane = 1u << 0,
        // �X
        Forest = 1u << 1,
        // �R
        Mountain = 1u << 2,
        // ��
        River = 1u << 3,

        // �C(�אڂ���e���C���ɔ�\�������݂���Ȃ�C�ɗאڂ��Ă���)
        Ocean = 1u << 4,
    }

    /// <summary>
    /// �n��̃^�C�v
    /// ���n�� 1000 �ԑ�
    /// ��Ռn�� 2000 �ԑ�
    /// ���A�n�� 3000 �ԑ�
    /// </summary>
    public enum eAreaType
    {
        [EnumString("")]
        None = -1,

        /// <summary>
        /// �s�s
        /// �ʏ�̒�
        /// ��p�I�Ɏ����𓾂�̂��ړI
        /// �̈�̖h�q�͂��グ��̂��ړI
        /// </summary>
        [EnumString("debug_town")]
        Town = 1000,

        /// <summary>
        /// ��s
        /// �ʏ�̒��Ƃ��܂�ς��Ȃ����A
        /// ��s�̂���̈悪�U�ߍ��܂ꂽ��Q�[���I�[�o�[�ƂȂ�̂ŁA
        /// ��s�̂���̈�̒n��Q�͖h�q�͂�傫���グ�Ă����K�v������
        /// </summary>
        [EnumString("debug_castle")]
        Capital = 1100,

        /// <summary>
        /// �v��
        /// �h�q�͂ɓ��������n��
        /// ���������͂قڂȂ����v�ǂ�����邾���ŁA���̗̈�̖h�q�͂����Ȃ�オ��
        /// </summary>
        [EnumString("debug_twin_town")]
        StrongHold = 1200,

        /// <summary>
        /// �����Q�g���|���X
        /// �������ɂ߂���
        /// ����ȏ����Ń����Q�g���|���X�ɕω������邱�Ƃ��\
        /// �����������i�i�ɂ�����A�h�q�͂����Ȃ肠����
        /// </summary>
        [EnumString("debug_castle")]
        LemegetonPoris = 1300,



        /// <summary>
        /// ���
        /// ������i�߂邱�Ƃŋ��͂ȗ̈�o�t�𓾂���
        /// �����͉��i�K�����݂��Ă���A�������邽�т�
        /// ���ԁA�܂��͉i���I�ȗ̈�o�t�𓾂���
        /// </summary>
        [EnumString("debug_remain")]
        Remains = 2000,

        /// <summary>
        /// ���A
        /// �T����i�߂邱�ƂŎ�����A�C�e���𓾂���
        /// �T���͉��x���s�������\�A
        /// �T���ɂ͏��R�P�l�ƕ��m���K�v
        /// �T���ɐ�������Ən���x���オ��A������A�C�e���𓾂���
        /// </summary>
        [EnumString("debug_cave")]
        Cave = 3000,
    }

    static class eAreaTypeExtention
    {
        static readonly private Dictionary<eAreaType, string> s_dic_ = new Dictionary<eAreaType, string>();
        static eAreaTypeExtention() => EnumStringUtility.ForeachEnumAttribute<eAreaType, EnumStringAttribute>((e, attr) => { s_dic_.Add(e, attr.Value); });
        static public string ToEnumString(this eAreaType e) => s_dic_[e];
    }

    /// <summary>
    /// �n���̃^�C�v
    /// ���n�A��ՁA���A��3�^�C�v
    /// </summary>
    public enum eAreaGroupType {

        [EnumString("")]
        None = -1,

        // �ʏ�
        [EnumString("area_town_bg_image")]
        Normal,

        // ���
        [EnumString("area_remain_bg_image")]
        Remains,

        // ���A
        [EnumString("area_cave_bg_image")]
        Cave
    }

    static class eAreaGroupTypeExtention
    {
        static readonly private Dictionary<eAreaGroupType, string> s_dic_ = new Dictionary<eAreaGroupType, string>();
        static eAreaGroupTypeExtention() => EnumStringUtility.ForeachEnumAttribute<eAreaGroupType, EnumStringAttribute>((e, attr) => { s_dic_.Add(e, attr.Value); });
        static public string ToEnumString(this eAreaGroupType e) => s_dic_[e];
    }





    /// <summary>
    /// �n�惌�R�[�h
    /// �ۑ����
    /// 2022/06/08
    ///     �J��V�X�e������
    ///     �Q�[���J�n���Ɉ�x�����������ė̈惌�R�[�h�ɕR�Â�
    ///     ������1�ڂ̃Z���ɑ΂��Ă܂��͊J����s��
    ///     �J��ƒ����ƒT���͕ʂ̂�
    ///     
    ///     �u���J��v�̒n��ɑ΂��čs�����Łu�J��ς݁v�ƂȂ�A�s�s�����݂ł���悤�ɂȂ�B
    ///     �J�񂵂��ۂɁA�܂�Ɉ�Ղ����A�����������B
    ///     ��Ղ����A�ɂȂ����n��͔j�󂷂邱�Ƃ͂ł��Ȃ�
    ///     ��Ղ̏ꍇ�͒������邱�ƂŃo�t�𓾂���
    ///     ���A�̏ꍇ�͒T�����邱�ƂŎ�����A�C�e���𓾂���
    ///     
    /// 2022/06/14
    ///     ES3 �ŕۑ�����Ȃ�����ŏڍ׃f�[�^�ɂ���ƕۊǂł��Ȃ��ȁE�E�E
    ///     OdinInspector �Ń��X�g�����ĕ\������ۂ̓N���X������܂��������₷������ Serialized ����ۂ͉����l���Ȃ��Ƃ���
    /// 
    /// </summary>
    [Serializable]
    public class SfArea : IJsonParser
    {
        // �n�� ID (���j�[�N ID)
        public uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // �n�於
        public string m_name = "";
        public string Name { get => m_name; set => m_name = value; }

        // �����Ă���̈� ID
        public uint m_dominionId = 0;
        public uint DominionId { get => m_dominionId; set => m_dominionId = value; }

        // �n��C���f�b�N�X(�X�N���[���̃Z���ԍ�)
        public int m_areaIndex = -1;
        public int AreaIndex { get => m_areaIndex; set => m_areaIndex = value; }

        /// <summary>
        /// true...���_
        /// ���_���푈���ɍU�ߍ��܂ꂽ�ۂ̖h�q�̒l�ɒ�������
        /// ���_�ɂ������̃o�t���푈���ɑ傫���e��
        /// ���_�ɂ��邱�Ƃł��̒����L�̃o�t�����낢��Ɖe������
        /// </summary>
        public bool m_baseFlag = false;
        public bool BaseFlag { get => m_baseFlag; set => m_baseFlag = value; }

        // �n��J����
        public eAreaDevelopmentState m_areaDevelopmentState = eAreaDevelopmentState.Not;
        public eAreaDevelopmentState AreaDevelopmentState { get => m_areaDevelopmentState; set => m_areaDevelopmentState = value; }

        // �n���^�C�v
        public eAreaGroupType m_areaGroupType = eAreaGroupType.None;
        public eAreaGroupType AreaGroupType { get => m_areaGroupType; set => m_areaGroupType = value; }

        // �n��^�C�v
        public eAreaType m_areaType = eAreaType.None;
        public eAreaType AreaType { get => m_areaType; set => m_areaType = value; }

        // �n�` (�n��ɑ��݂���n�`)
        public eExistingTerrain m_existingTerrain = 0;
        public eExistingTerrain ExistingTerrain { get => m_existingTerrain; set => m_existingTerrain = value; }

        // �n��l��
        public int m_population = 0;
        public int Population { get => m_population; set => m_population = value; }

        // �ő��搔(�ݒ�\�ȋ��̍ő吔)
        // ��������͌���̒n��l���ɔ��
        public int m_maxZoneCount = -1;
        public int MaxZoneCount { get => m_maxZoneCount; set => m_maxZoneCount = value; }

        public List<uint> m_productionResourceItemIdList = new List<uint>();
        public List<uint> ProductionResourceItemIdList { get => m_productionResourceItemIdList; set => m_productionResourceItemIdList = value; }

#if false
        // �ۊǂ���Ă���A�C�e�� ID �Ƒ���
        public class StragedProductSet : IJsonParser {

            public uint m_itemId = 0;
            public uint ItemId { get => m_itemId; set => m_itemId = value; }

            public int m_count = 0;
            public int Count { get => m_count; set => m_count = value; }

            public StragedProductSet() { }
            public StragedProductSet(uint itemId, int ct) { m_itemId = itemId; m_count = ct; }

            public void Parse(IDictionary<string, object> data)
            {

            }
        }

        public List<StragedProductSet> m_storagedProductList = new List<StragedProductSet>();
        public List<StragedProductSet> StoragedProductList => m_storagedProductList;
#endif

        // ���蓖�Ă��Ă��镐�� ID
        public List<uint> m_troopList = new List<uint>();
        public List<uint> TroopList => m_troopList;

        public void Parse(IDictionary<string, object> data)
        {
            
        }
    }


    /// <summary>
    /// �n�� �Ǘ�
    /// �v���C���ɐ�������Ă��邷�ׂĂ� SfAreaData
    /// �ۑ����͕ʃt�@�C���̕ʃN���X�Ɏ���
    /// </summary>
    public class SfAreaTable : RecordTable<SfArea>
    {


        // �o�^
        public void Regist(SfArea record) => RecordList.Add(record);

        // �n�惌�R�[�h�̎擾
        public override SfArea Get(uint id) => RecordList.Find(r => r.Id == id);

        /// <summary>
        /// ���_�̕ύX
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name=""></param>
        public void ChangeBaseFlag(uint areaId, bool baseFlag) {
            Get(areaId).BaseFlag = baseFlag;
        }

        /// <summary>
        /// �J���Ԃ̕ύX
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="state"></param>
        public void ChangeDevelopmentState(uint areaId, eAreaDevelopmentState state) {
            Get(areaId).AreaDevelopmentState = state;
        }

        // �n�`�̒ǉ�
        public void AddTerrain(uint areaId, eExistingTerrain terrain) => Get(areaId).ExistingTerrain |= terrain;
        // �n�`�̍폜
        public void RemoveTerrain(uint areaId, eExistingTerrain terrain) => Get(areaId).ExistingTerrain &= ~terrain;


#if false
        /// <summary>
        /// ���Y�A�C�e���̒ǉ�
        /// </summary>
        /// <param name="areaData"></param>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        public void AddProductItem(SfArea areaData, uint itemId, int count)
        {
            var set = areaData.StoragedProductList.Find(s => s.m_itemId == itemId);

            if (set != null)
            {
                set.m_count += count;
            }
            else
            {
                areaData.StoragedProductList.Add(new SfArea.StragedProductSet(itemId, count));
            }
        }
#endif

        /// <summary>
        /// ���{�݂̌��݂ɕK�v�ȃR�X�g�̃`�F�b�N
        /// </summary>
        /// <param name="areaRecord"></param>
        /// <param name="facilityRecord"></param>
        /// <returns>true...���݉\</returns>
        public bool CheckCostForBuildingFacility(SfArea areaData, SfZoneFacilityRecord facilityRecord)
        {
#if DEBUG_COSTCHECK
            // ��X�ۊǐ��Y���X�g���N�����ɉ��Z�ł���悤�Ɏ���

            return true;
#else
            // �O���Ŏ擾���Ĉ����ɓn���������ǂ�����
            //var areaRecord = Get(areaId);
            //var facilityRecord = SfZoneFacilityRecordTable.Instance.Get(type);

            foreach (var cost in facilityRecord.Costs)
            {
                var set = areaData.StoragedProductList.Find(s => s.m_itemId == cost.Id);

                // �K�v�R�X�g�̎��������Y���ɂP�ł������ꍇ�� false
                if (set == null)
                {
                    return false;
                }

                // ���Y�����R�X�g��菭�Ȃ��ꍇ�� false
                if (set.Count < cost.Count)
                {
                    return false;
                }
            }

            return true;
#endif
        }


#if false
        /// <summary>
        /// ���{�݂̌��݂ɕK�v�ȃR�X�g���x����
        /// 
        /// ���{�݂ɕK�v�ȃR�X�g�� Warehouse �ł���q�ɂ���
        /// �q�ɂ͒n��Ƃ͕ʂɃN���X���쐬
        /// </summary>
        /// <param name="areaRecord"></param>
        /// <param name="facilityRecord"></param>
        public void PayCostForBuildingFacility(SfArea areaData, SfZoneFacilityRecord facilityRecord)
        {
            foreach (var cost in facilityRecord.Costs)
            {
                var set = areaData.StoragedProductList.Find(s => s.m_itemId == cost.Id);
                set.Count -= cost.Count;
            }
        }
#endif
    }

    /// <summary>
    /// �n�惌�R�[�h�e�[�u���Ǘ�
    /// </summary>
    public class SfAreaTableManager : Singleton<SfAreaTableManager>
    {
        // ���j�[�N ID ���X�g
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        private SfAreaTable m_table = new SfAreaTable();
        public SfAreaTable Table => m_table;

        /// <summary>
        /// �ǂݍ��ݏ���
        /// </summary>
        public void Load() {
            var director = new RecordTableESDirector<SfArea>(new ESLoadBuilder<SfArea, SfAreaTable>("SfAreaDataTable"));
            director.Construct();
            if (director.GetResult() != null && director.GetResult().RecordList.Count > 0)
                m_table.RecordList.AddRange(director.GetResult().RecordList);
        }

        /// <summary>
        /// �ۑ�����
        /// </summary>
        public void Save() {
            var director = new RecordTableESDirector<SfArea>(new ESSaveBuilder<SfArea>("SfAreaDataTable", m_table));
            director.Construct();
        }
    }
}