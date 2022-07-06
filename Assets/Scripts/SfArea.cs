using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        // ���n
        [EnumString("area_town_bg_image")]
        Plane,

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
    /// ���^�C�v
    /// stellaris �͓d�C�A�H�ƁA�_�ƁA�Y�Ƌ���ʘg�ŐU�蕪�����悤�ɂȂ��Ă��邪
    /// ��������ׂĐ��g�Ƃ��Ĉ����悤�Ȋ���
    /// ���p�ł���y�n�������Ă����ɋ���U�蕪����Ƃ����l������
    /// </summary>
    public enum eZoneType
    {
        /// <summary>
        /// �����ݒ肳��Ă��Ȃ�
        /// �j�󂵂��ۂ�����ɐݒ�
        /// </summary>
        None = -1,

        /// <summary>
        /// ���Y��_�c��
        /// ���n�ɗאڂ��Ă���Ό��݉\
        /// �H������������
        /// </summary>
        Production_Fields = 1000,
        Production_Fishery = 1001,

        /// <summary>
        /// ���Y��_�̌@��
        /// �R�ɗאڂ��Ă���Ό��݉\
        /// �z��������������
        /// </summary>
        Production_Mining = 1010,

        /// <summary>
        /// ���Y��_���̏�
        /// �X�ɗאڂ��Ă���Ό��݉\
        /// �ޖ؎���������
        /// </summary>
        Production_LoggingArea = 1020,

        /// <summary>
        /// ���Ƌ�
        /// ����������
        /// ���܂��܂ȃA�C�e�����g���[�h�\�ɂȂ�
        /// ���̓��Y�i�Ȃǂ�������
        /// </summary>
        Commercial_MarketPlace  = 2000,
        Commercial_TradingPost  = 2010,
        // �`�F�C�ɖʂ��Ă��Ȃ��Ɛݒu�s��
        Commercial_Harbor       = 2020,



        /// <summary>
        /// ������
        /// ���͎���������
        /// �����n�̃A�C�e���N���t�g�𐶐��\
        /// ���@�h��͂����グ�鎖���\
        /// </summary>
        Witchcrafty_MagicItemWorkshop = 3000,
        Witchcrafty_RuneEngravedStone = 3010,

        /// <summary>
        /// ��ǋ�
        /// ���̈���̖h��͂��グ�邱�Ƃ��\
        /// �ǂƂ͈Ⴄ
        /// �ǂ͂܂��ʓr��ǂƂ��Č��݉\
        /// </summary>
        Citadel = 4000,
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
    public class SfAreaRecord
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

        // �n��ɑ��݂���n�`
        public eExistingTerrain m_existingTerrain = 0;
        public eExistingTerrain ExistingTerrain { get => m_existingTerrain; set => m_existingTerrain = value; }

        // �n��l��
        public int m_population = 0;
        public int Puplation { get => m_population; set => m_population = value; }

        // �ő��搔(�ݒ�\�ȋ��̍ő吔)
        // ��������͌���̒n��l���ɔ��
        public int m_maxZoneCount = -1;
        public int MaxZoneCount { get => m_maxZoneCount; set => m_maxZoneCount = value; }

        // �ݒ肳��Ă�����^�C�v���X�g<�Z���C���f�b�N�X�A���^�C�v>
        public Dictionary<uint, eZoneType> m_zoneTypeDict = new Dictionary<uint, eZoneType>();
        public Dictionary<uint, eZoneType> ZoneTypeDict { get => m_zoneTypeDict; set => m_zoneTypeDict = value; }

        // �ݒ肳��Ă�����̊g����<�Z���C���f�b�N�X�A�g����>
        public Dictionary<uint, int> m_zoneExpantionDict = new Dictionary<uint, int>();
        public Dictionary<uint, int> ZoneExpantionDict = new Dictionary<uint, int>();
        
    }

    /// <summary>
    /// �n�搶�� �H�� ���
    /// 2022/0608
    ///     �s�s�ƈ�ՂƓ��A�ŕ�����ׂ��H
    ///     �ő��搔 = �ő咲�����H
    /// </summary>
    public abstract class SfAreaFactoryBase
    {
        public SfAreaRecord Create(uint uniqueId, int areaIndex, uint dominionId)
        {
            var record = CreateAreaRecord();

            // �n�� ID �̐ݒ�
            record.Id = uniqueId;

            // �n�於��ݒ�
            record.Name = CreateRandomAreaName();

            // �̈� ID ��ݒ�
            record.DominionId = dominionId;

            // �n��C���f�b�N�X�̐ݒ�
            record.AreaIndex = areaIndex;

            // �n���^�C�v�̐ݒ�
            record.AreaGroupType = SettingRandomAreaGroupType();

            // �n��^�C�v�̐ݒ�
            record.AreaType = RandomSettingAreaType();

            // �אڒn�`�^�C�v�̐ݒ�
            record.ExistingTerrain = SettingExistingTerrain(SfDominionRecordTableManager.Instance.Get(dominionId));

            // �ő��搔�̐ݒ�

            return record;
        }

        /// <summary>
        /// �n�惌�R�[�h�𐶐�
        /// </summary>
        /// <returns></returns>
        protected abstract SfAreaRecord CreateAreaRecord();

        /// <summary>
        /// �n�於�������_���ɐ���
        /// </summary>
        /// <returns></returns>
        protected abstract string CreateRandomAreaName();

        // �n���^�C�v��ݒ�
        protected abstract eAreaGroupType SettingRandomAreaGroupType();

        // �����_���ɒn��^�C�v��ݒ�
        protected abstract eAreaType RandomSettingAreaType();

        // �אڒn�`�^�C�v�̐ݒ�
        protected abstract eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion);

        // �ő��搔�̌v�Z
        protected abstract int CulcMaxZoneCount();
    }

    /// <summary>
    /// �n�惌�R�[�h���� �H��
    /// </summary>
    public abstract class SfAreaCreateFactory : SfAreaFactoryBase
    {
        protected override SfAreaRecord CreateAreaRecord()
        {
            return new SfAreaRecord();
        }
    }

    /// <summary>
    /// �n�� ���n ���� �H��
    /// </summary>
    public class SfAreaFactoryPlane : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test_plane"; }

        // �n���^�C�v��ݒ�
        protected override eAreaGroupType SettingRandomAreaGroupType() { return eAreaGroupType.Plane; }

        // �n��^�C�v�������_���ɐݒ�
        protected override eAreaType RandomSettingAreaType() { return eAreaType.Town; }

        // ���݂���n�`�̐ݒ�
        protected override eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion)
        {
            eExistingTerrain terrain = 0;

            // �����A�R�A�X�A�C�A�͊m�����z�����A�d�����\�Ȃ̂ł��ꂼ������ꂼ�ꂾ���̊����Ōv�Z


            // ����
            float rate = UnityEngine.Random.value * 100.0f;
            if (ConfigController.Instance.DistributionRatioPlane > rate)
            {
                terrain |= eExistingTerrain.Plane;
            }

            // �R
            rate = UnityEngine.Random.value * 100.0f;
            if (ConfigController.Instance.DistributionRatioMountain > rate)
            {
                terrain |= eExistingTerrain.Mountain;
            }


            // �X
            rate = UnityEngine.Random.value * 100.0f;
            if (ConfigController.Instance.DistributionRatioForest > rate)
            {
                terrain |= eExistingTerrain.Forest;
            }

            // ��
            rate = UnityEngine.Random.value * 100.0f;
            if (ConfigController.Instance.DistributionRatioRiver > rate)
            {
                terrain |= eExistingTerrain.River;
            }

            // �C�̂ݗ̈悪�C�ɖʂ��Ă��邩�ǂ������`�F�b�N���ăt���O�𗧂Ă�
            if (dominion.NeighboursOceanFlag == true)
            {
                rate = UnityEngine.Random.value * 100.0f;
                if (ConfigController.Instance.DistributionRatioOcean > rate)
                {
                    terrain |= eExistingTerrain.Ocean;
                }
            }

            // �����n�`�����������ꍇ�͕�����ݒ�
            if (terrain == 0)
                terrain |= eExistingTerrain.Plane;

            return terrain;
        }


        // ���ő吔�̌v�Z
        protected override int CulcMaxZoneCount() { return UnityEngine.Random.Range(ConfigController.Instance.MinZoneValue, ConfigController.Instance.MaxZoneValue + 1); }
    }

    /// <summary>
    /// �n�� ��� ���� �H��
    /// </summary>
    public class SfAreaFactoryRemains : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test_reamins"; }

        // �n���^�C�v��ݒ�
        protected override eAreaGroupType SettingRandomAreaGroupType() { return eAreaGroupType.Remains; }

        // �n��^�C�v�������_���ɐݒ�
        protected override eAreaType RandomSettingAreaType() { return eAreaType.Remains; }

        // ���݂���n�`�̐ݒ� (��Ղ͒n�`���ʂ͂��ɂ��Ȃ�)
        protected override eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion)
        {
            return 0;
        }

        protected override int CulcMaxZoneCount() { return -1; }
    }

    /// <summary>
    /// �n�� ���A ���� �H��
    /// </summary>
    public class SfAreaFactoryCave : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test_cave"; }

        // �n���^�C�v��ݒ�
        protected override eAreaGroupType SettingRandomAreaGroupType() { return eAreaGroupType.Cave; }

        // �n��^�C�v�������_���ɐݒ�
        protected override eAreaType RandomSettingAreaType() { return eAreaType.Cave; }

        // ���݂���n�`�̐ݒ� (���A�͒n�`���ʂ͂��ɂ��Ȃ�)
        protected override eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion)
        {
            return 0;
        }

        protected override int CulcMaxZoneCount() { return -1; }
    }

    /// <summary>
    /// �n�� ���� �H�� �Ǘ�
    /// </summary>
    public class SfAreaFactoryManager : Singleton<SfAreaFactoryManager>
    {
        private List<SfAreaFactoryBase> factoryList = null;

        public SfAreaFactoryManager() {

            factoryList = new List<SfAreaFactoryBase>()
            {
                new SfAreaFactoryPlane(),
                new SfAreaFactoryRemains(),
                new SfAreaFactoryCave(),
            };
        }

        /// <summary>
        /// �n��̍쐬
        /// </summary>
        /// <param name="areaIndex">�n��C���f�b�N�X(�Z���ԍ�)</param>
        /// <param name="dominionId">�����Ă���̈� ID</param>
        /// <returns></returns>
        public SfAreaRecord RandomCreate(int areaIndex, uint dominionId, eAreaGroupType factoryType = eAreaGroupType.None)
        {
            // ������Ղ����A���������_��
            // �����͐ݒ�ł���悤�ɂ���
            float rate = UnityEngine.Random.value * 100.0f;

            if (factoryType == eAreaGroupType.None)
            {
                // �� (rate �� 80 �ȉ��Ȃ璬)
                if (ConfigController.Instance.AreaTownRate > rate)
                {
                    // ��
                    factoryType = eAreaGroupType.Plane;
                }
                // ��� (rate �� 80 ���� 90 �Ȃ���)
                else if (ConfigController.Instance.AreaTownRate <= rate && (ConfigController.Instance.AreaTownRate + ConfigController.Instance.AreaRemainsRate) > rate)
                {
                    // ���
                    factoryType = eAreaGroupType.Remains;
                }
                // ���A (rate �� 90 ���� 100 �Ȃ���)
                else if ((ConfigController.Instance.AreaTownRate + ConfigController.Instance.AreaRemainsRate) <= rate && (ConfigController.Instance.AreaTownRate + ConfigController.Instance.AreaRemainsRate + ConfigController.Instance.AreaCaveRate) > rate)
                {
                    // ���A
                    factoryType = eAreaGroupType.Cave;
                }
            }

            if (factoryType == eAreaGroupType.None)
            {
                Debug.LogError("factoryType == -1 !!!");
                return null;
            }

            // ���j�[�N ID �̍쐬
            uint uniqueId = SfConstant.CreateUniqueId(ref SfAreaRecordTableManager.Instance.m_uniqueIdList);

            // �n�惌�R�[�h���쐬
            var record = factoryList[(int)factoryType].Create(uniqueId, areaIndex, dominionId);

            return record;
        }
    }

    /// <summary>
    /// �n�� �Ǘ�
    /// �v���C���ɐ�������Ă��邷�ׂĂ� SfAreaRecord
    /// �ۑ����͕ʃt�@�C���̕ʃN���X�Ɏ���
    /// </summary>
    public class SfAreaRecordTable : RecordTable<SfAreaRecord>
    {
        // ���j�[�N ID ���X�g
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        // �o�^
        public void Regist(SfAreaRecord record) => RecordList.Add(record);

        // �n�惌�R�[�h�̎擾
        public override SfAreaRecord Get(uint id) => RecordList.Find(r => r.Id == id);

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

        /// <summary>
        /// �n��̋��Ɏw��̋��^�C�v��ύX����
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="cellIndex"></param>
        /// <param name="zoneType"></param>
        public void ChangeZoneType(uint areaId, int cellIndex, eZoneType zoneType) {

            var record = Get(areaId);

            if (cellIndex >= record.MaxZoneCount)
            {
                Debug.LogWarning("index >= record.MaxZoneCount !!!");
                return;
            }

            record.ZoneTypeDict.Add((uint)cellIndex, zoneType);
        }

        /// <summary>
        /// ���̊g��
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="cellIndex"></param>
        public void ExpantionZone(uint areaId, int cellIndex) 
        {
            var record = Get(areaId);

            if (cellIndex >= record.MaxZoneCount)
            {
                Debug.LogWarning("index >= record.MaxZoneCount !!!");
                return;
            }

            int expCt = record.ZoneExpantionDict[(uint)cellIndex];
            expCt++;
            record.ZoneExpantionDict[(uint)cellIndex] = expCt;
        }
    }

    /// <summary>
    /// �n�惌�R�[�h�e�[�u���Ǘ�
    /// </summary>
    public class SfAreaRecordTableManager : SfAreaRecordTable
    {
        private static SfAreaRecordTableManager s_instance = null;

        public static SfAreaRecordTableManager Instance {

            get
            {
                if (s_instance != null)
                    return s_instance;

                s_instance = new SfAreaRecordTableManager();

                s_instance.Load();

                return s_instance;
            }
        }

        /// <summary>
        /// �ǂݍ��ݏ���
        /// </summary>
        public void Load() {

            var builder = new ESLoadBuilder<SfAreaRecord, SfAreaRecordTable>("SfAreaRecordTable");

            var director = new RecordTableESDirector<SfAreaRecord>(builder);

            director.Construct();

            if (director.GetResult() != null && director.GetResult().RecordList.Count > 0)
            {
                m_recordList.AddRange(director.GetResult().RecordList);
            }
        }

        /// <summary>
        /// �ۑ�����
        /// </summary>
        public void Save() {

            var builder = new ESSaveBuilder<SfAreaRecord>("SfAreaRecordTable", this);

            var director = new RecordTableESDirector<SfAreaRecord>(builder);

            director.Construct();
        }
    }
}