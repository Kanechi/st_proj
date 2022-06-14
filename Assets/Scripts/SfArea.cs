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
    /// �n��^�C�v
    /// </summary>
    public enum eAreaType
    {
        None = -1,

        /// <summary>
        /// �s�s
        /// �ʏ�̒�
        /// ��p�I�Ɏ����𓾂�̂��ړI
        /// �̈�̖h�q�͂��グ��̂��ړI
        /// </summary>
        Town = 1000,

        /// <summary>
        /// ��s
        /// �ʏ�̒��Ƃ��܂�ς��Ȃ����A
        /// ��s�̂���̈悪�U�ߍ��܂ꂽ��Q�[���I�[�o�[�ƂȂ�̂ŁA
        /// ��s�̂���̈�̒n��Q�͖h�q�͂�傫���グ�Ă����K�v������
        /// </summary>
        Capital = 1100,

        /// <summary>
        /// �v��
        /// �h�q�͂ɓ��������n��
        /// ���������͂قڂȂ����v�ǂ�����邾���ŁA���̗̈�̖h�q�͂����Ȃ�オ��
        /// </summary>
        StrongHold = 1200,

        /// <summary>
        /// �����Q�g���|���X
        /// �������ɂ߂���
        /// ����ȏ����Ń����Q�g���|���X�ɕω������邱�Ƃ��\
        /// �����������i�i�ɂ�����A�h�q�͂����Ȃ肠����
        /// </summary>
        LemegetonPoris = 1300,



        /// <summary>
        /// ���
        /// ������i�߂邱�Ƃŋ��͂ȗ̈�o�t�𓾂���
        /// �����͉��i�K�����݂��Ă���A�������邽�т�
        /// ���ԁA�܂��͉i���I�ȗ̈�o�t�𓾂���
        /// </summary>
        Remains = 2000,

        /// <summary>
        /// ���A
        /// �T����i�߂邱�ƂŎ�����A�C�e���𓾂���
        /// �T���͉��x���s�������\�A
        /// �T���ɂ͏��R�P�l�ƕ��m���K�v
        /// �T���ɐ�������Ən���x���オ��A������A�C�e���𓾂���
        /// </summary>
        Cave = 3000,
    }

    /// <summary>
    /// ���^�C�v
    /// </summary>
    public enum eZoneType
    {
        None = -1,

        /// <summary>
        /// �_�Ƌ�
        /// ���n�ɗאڂ��Ă���Ό��݉\
        /// �H������������
        /// </summary>
        Agriculture = 1000,

        /// <summary>
        /// ���Ƌ�
        /// ����������
        /// ���܂��܂ȃA�C�e�����g���[�h�\�ɂȂ�
        /// ���̓��Y�i�Ȃǂ�������
        /// </summary>
        Commercial = 2000,

        /// <summary>
        /// �z�Ƌ�
        /// �R�ɗאڂ��Ă���Ό��݉\
        /// �z��������������
        /// </summary>
        MiningIndustry = 3000,

        /// <summary>
        /// ���̒n
        /// �X�ɗאڂ��Ă���Ό��݉\
        /// �ޖ؎���������
        /// </summary>
        LoggingArea = 4000,

        /// <summary>
        /// ������
        /// ���͎���������
        /// �����n�̃A�C�e���N���t�g�𐶐��\�ɂȂ�
        /// ���@�h��͂����グ�鎖���\
        /// </summary>
        Witchcrafty = 5000,

        /// <summary>
        /// ��ǋ�
        /// ���̈���̖h��͂��グ�邱�Ƃ��\
        /// �ǂƂ͈Ⴄ
        /// �ǂ͂܂��ʓr��ǂƂ��Č��݉\
        /// </summary>
        Citadel = 6000,
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

        // �n��J����
        public eAreaDevelopmentState m_areaDevelopmentState = eAreaDevelopmentState.Not;
        public eAreaDevelopmentState AreaDevelopmentState { get => m_areaDevelopmentState; set => m_areaDevelopmentState = value; }

        // �n��^�C�v
        public eAreaType m_areaType = eAreaType.None;
        public eAreaType AreaType { get => m_areaType; set => m_areaType = value; }


        // �ő��搔(�ݒ�\�ȋ��̍ő吔)
        public int m_maxZoneCount = -1;
        public int MaxZoneCount { get => m_maxZoneCount; set => m_maxZoneCount = value; }

        // �ݒ肳��Ă�����^�C�v���X�g
        public List<eZoneType> m_zoneTypeList = new List<eZoneType>();
        public List<eZoneType> ZoneTypeList { get => m_zoneTypeList; set => m_zoneTypeList = value; }
    }

#if false
    /// <summary>
    /// �n��
    /// ����͒n��Z���ł���̈�X�N���[���Z���ɂȂ邩��K�v�Ȃ��H
    /// </summary>
    public class SfArea : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
#endif
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

            // �n��^�C�v�̐ݒ�

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

        /// <summary>
        /// �n��^�C�v��ݒ� (���A��ՁA���A)
        /// </summary>
        /// <returns></returns>
        protected abstract eAreaType SettingAreaType();

        protected abstract int SettingMaxZoneCount();
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
    /// �n�� �� ���� �H��
    /// </summary>
    public class SfAreaFactoryTown : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test"; }

        protected override eAreaType SettingAreaType() { return eAreaType.Town; }

        protected override int SettingMaxZoneCount() { return UnityEngine.Random.Range(ConfigController.Instance.MinAreaValue, ConfigController.Instance.MaxAreaValue + 1); }
    }

    /// <summary>
    /// �n�� ��� ���� �H��
    /// </summary>
    public class SfAreaFactoryRemains : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test"; }

        protected override eAreaType SettingAreaType() { return eAreaType.Remains; }

        protected override int SettingMaxZoneCount() { return UnityEngine.Random.Range(ConfigController.Instance.MinAreaValue, ConfigController.Instance.MaxAreaValue + 1); }
    }

    /// <summary>
    /// �n�� ���A ���� �H��
    /// </summary>
    public class SfAreaFactoryCave : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test"; }

        protected override eAreaType SettingAreaType() { return eAreaType.Cave; }

        protected override int SettingMaxZoneCount() { return -1; }
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
                 new SfAreaFactoryCave(),
                 new SfAreaFactoryRemains(),
                 new SfAreaFactoryTown()
            };
        }

        /// <summary>
        /// �n��̍쐬
        /// </summary>
        /// <param name="areaIndex">�n��C���f�b�N�X(�Z���ԍ�)</param>
        /// <param name="dominionId">�����Ă���̈� ID</param>
        /// <returns></returns>
        public SfAreaRecord Create(int areaIndex, uint dominionId) 
        {
            // ������Ղ����A���������_��
            // �����͐ݒ�ł���悤�ɂ���
            float rate = UnityEngine.Random.value * 100.0f;

            int factoryType = -1;

            if (100.0f > rate && (ConfigController.Instance.AreaTownRate + ConfigController.Instance.AreaRemainsRate) <= rate)
            {
                // ���A
                factoryType = 0;
            }
            else if ((ConfigController.Instance.AreaTownRate + ConfigController.Instance.AreaRemainsRate) > rate &&
                (ConfigController.Instance.AreaTownRate) <= rate)
            {
                // ���
                factoryType = 1;
            }
            else if (ConfigController.Instance.AreaTownRate < rate)
            {
                // ��
                factoryType = 2;
            }

            if (factoryType == -1)
            {
                Debug.LogError("factoryType == -1 !!!");
                return null;
            }

            // ���j�[�N ID �̍쐬
            uint uniqueId = SfConstant.CreateUniqueId(ref SfAreaRecordTableManager.Instance.m_uniqueIdList);

            // �n�惌�R�[�h���쐬
            var record = factoryList[factoryType].Create(uniqueId, areaIndex, dominionId);

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