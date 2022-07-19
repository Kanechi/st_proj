using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{
    [Serializable]
    public class SfSaveRecord
    {
        // �ۑ��ԍ�
        public uint m_saveNo = 0;
        public uint SaveNo { get => m_saveNo; set => m_saveNo = value; }

        // �ۑ�����
        public long m_saveTime = 0;
        public long SaveTime { get => m_saveTime; set => m_saveTime = value; }
    }

    /// <summary>
    /// ����
    /// �ۑ����
    /// �Q�[���J�n���Ɉ�x�����쐬
    /// </summary>
    [Serializable]
    public class SfKingdomRecord
    {
        // ���� ID (�����l�b�g���[�N���쐬����ꍇ�̓T�[�o�[ID�ɏ���)
        public int m_id = 0;
        public int Id { get => m_id; set => m_id = value; }

        // �����̐F
        public Color m_color = Color.white;
        public Color Color { get => m_color; set => m_color = value; }

        // true...�����̍�
        public bool m_selfFlag = false;
        public bool SelfFlag { get => m_selfFlag; set => m_selfFlag = value; }

        // �̈� ID ���X�g
        public List<uint> m_sfDominionIdList = new List<uint>();
        public List<uint> DominionIdList { get => m_sfDominionIdList; set => m_sfDominionIdList = value; }

        // ���ݍ����ɂȂ��Ă���l����ID
        // �l�� ID �͌��݂̃Z�[�u ID ���猟��
        public uint m_personId = 0;
        public uint PersonId { get => m_personId; set => m_personId = value; }

        // �������x��
        public uint m_kingdomLv = 0;
        public uint KngdomLv { get => m_kingdomLv; set => m_kingdomLv = value; }

#if false
        // �ő�l��(���Ă�����͒n�悲�Ƃɐݒ�H)
        // �������ɕ\������ۂ͍��v�l��\�����ȁH
        public uint m_maxPopulation = 0;
        public uint MaxPopulation { get => m_maxPopulation; set => m_maxPopulation = value; }

        // ���ݐl��(������n�悲�Ƃɐݒ�H)
        public uint m_population = 0;
        public uint Population { get => m_population; set => m_population = value; }
#endif
        // �����o���l
        public uint m_kingdomExp = 0;
        public uint KingdomExp { get => m_kingdomExp; set => m_kingdomExp = value; }

        // ������
        public string m_name = "";
        public string Name { get => m_name; set => m_name = value; }
    }

    /// <summary>
    /// ���������H����
    /// </summary>
    public abstract class SfKingdomFactoryBase
    {
        public SfKingdomRecord Create(int uniqueId)
        {
            var record = CreateRecord();

            // ���j�[�N ID �ݒ�
            record.Id = uniqueId;

            // �������ݒ�
            CreateName(record);

            // �����J���[�ݒ�
            SettingColor(record);

            // �����̍����ǂ����̃t���O�̐ݒ�
            SettingSelfFlag(record);

            return record;
        }

        // ���R�[�h����
        protected abstract SfKingdomRecord CreateRecord();

        // ����������
        protected abstract void CreateName(SfKingdomRecord record);
        // �����J���[�̐ݒ�
        protected abstract void SettingColor(SfKingdomRecord record);

        protected abstract void SettingSelfFlag(SfKingdomRecord record);
    }

    /// <summary>
    /// ���������H��
    /// </summary>
    public abstract class SfKingdomFactory : SfKingdomFactoryBase
    {
        protected override SfKingdomRecord CreateRecord()
        {
            return new SfKingdomRecord();
        }
    }

    /// <summary>
    /// �����̐���
    /// �����̐����̓Q�[���J�n�O�ɐݒ肵�����ڂ�ݒ�
    /// </summary>
    public class SfSelfKingdomFactory : SfKingdomFactory
    {
        // ����������
        protected override void CreateName(SfKingdomRecord record) {
            record.Name = SfConfigController.Instance.KingdomName;
        }

        // �����J���[�̐ݒ�
        protected override void SettingColor(SfKingdomRecord record) {
            record.Color = SfConfigController.Instance.KingdomColor;
        }

        // �����̍����ǂ����̃t���O
        protected override void SettingSelfFlag(SfKingdomRecord record)
        {
            record.SelfFlag = true;
        }
    }


    // ���̑��̍��̃����_������
    // ������x�̍������O�ɍ쐬���Ă����Ċ���U�邾����
    // �Ƃǂ߂邩�A���ׂĂO����쐬���邩�E�E�E
    public class SfOtherKingdomFactory : SfKingdomFactory
    {
        // ����������
        protected override void CreateName(SfKingdomRecord record)
        {
            // �����_������
            record.Name = "test";
        }

        // �����J���[�̐ݒ�
        protected override void SettingColor(SfKingdomRecord record)
        {
            record.Color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 0.6f);
        }

        // �����̍����ǂ����̃t���O
        protected override void SettingSelfFlag(SfKingdomRecord record)
        {
            record.SelfFlag = false;
        }
    }

    /// <summary>
    /// �����H��Ǘ�
    /// </summary>
    public class SfKingdomFactoryManager : Singleton<SfKingdomFactoryManager>
    {
        public SfKingdomRecord Create(bool selfKingdom)
        {
            SfKingdomFactoryBase factory = null;

            if (selfKingdom)
            {
                factory = new SfSelfKingdomFactory();
            }
            else
            {
                factory = new SfOtherKingdomFactory();
            }

            // ���j�[�N ID
            //uint uniqueId = SfConstant.CreateUniqueId(ref SfKingdomRecordTableManager.Instance.m_uniqueIdList);

            // �̈惌�R�[�h
            var record = factory.Create(SfKingdomRecordTableManager.Instance.m_uniqueId);

            SfKingdomRecordTableManager.Instance.m_uniqueId++;

            return record;
        }
    }

    /// <summary>
    /// �������R�[�h�Ǘ�
    /// �v���C���ɐ�������Ă��邷�ׂĂ� SfKingdomRecord
    /// </summary>
    public class SfKingdomRecordTable : RecordTable<SfKingdomRecord>
    {
        // ���j�[�N ID ���X�g
        public int m_uniqueId = 0;

        // �o�^
        public void Regist(SfKingdomRecord record) => RecordList.Add(record);

        // �̈惌�R�[�h�̎擾
        public override SfKingdomRecord Get(uint id) => RecordList.Find(r => r.Id == id);

        // ���g�̉������擾
        public SfKingdomRecord GetSelfKingdom() => RecordList.Find(r => r.m_selfFlag == true);

        /// <summary>
        /// �����̕ύX
        /// </summary>
        /// <param name="kingdomId">���� ID</param>
        /// <param name="personId">���̍����̐l�� ID</param>
        public void ChangeKing(uint kingdomId, uint nextPersonId) => Get(kingdomId).PersonId = nextPersonId;

        /// <summary>
        /// true...���Ɏw��̗̈���̂��Ă���
        /// </summary>
        public bool CheckDominionId(uint kingdomId, uint dominionId) => Get(kingdomId).DominionIdList.Contains(dominionId);

        /// <summary>
        /// �w��̗̈���̂���
        /// </summary>
        public void AddDominionId(uint kingdomId, uint dominionId)
        {
            if (CheckDominionId(kingdomId, dominionId))
                return;
            Get(kingdomId).DominionIdList.Add(dominionId);
        }

        /// <summary>
        /// �������x���̑���
        /// </summary>
        /// <param name="kingdomId"></param>
        public void IncKingdomLv(uint kingdomId) => Get(kingdomId).m_kingdomLv++;

#if false
        /// <summary>
        /// �ő�l���̕ύX
        /// 0 �ȉ��ɂ͂Ȃ�Ȃ�
        /// </summary>
        /// <param name="kingdomId"></param>
        /// <param name="pop"></param>
        public void ChangeMaxPop(uint kingdomId, uint pop)
        {
            uint p = Get(kingdomId).m_maxPopulation;
            p += pop;
            if (p < 0)
                p = 0;
            Get(kingdomId).m_maxPopulation = p;
        }

        /// <summary>
        /// �l���̕ύX
        /// 0 �ȉ��ɂ͂Ȃ�Ȃ�
        /// �ő�l���ȏ�ɂ͂Ȃ�Ȃ�
        /// </summary>
        /// <param name="kingdomId"></param>
        /// <param name="pop"></param>
        public void ChangePop(uint kingdomId, uint pop)
        {
            var record = Get(kingdomId);
            uint p = record.m_population;
            p += pop;
            if (p < 0)
                p = 0;
            if (p > record.MaxPopulation)
                p = record.MaxPopulation;
            record.m_population = p;
        }
#endif
    }

    public class SfKingdomRecordTableManager : SfKingdomRecordTable
    {
        private static SfKingdomRecordTableManager s_instance = null;

        public static SfKingdomRecordTableManager Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;

                s_instance = new SfKingdomRecordTableManager();
                s_instance.Load();
                return s_instance;
            }
        }

        /// <summary>
        /// �ǂݍ��ݏ���
        /// </summary>
        public void Load()
        {
            RecordTableESDirector<SfKingdomRecord> director = new RecordTableESDirector<SfKingdomRecord>(new ESLoadBuilder<SfKingdomRecord, SfKingdomRecordTable>("SfKingdomRecordTable"));
            director.Construct();
            if (director.GetResult() != null && director.GetResult().RecordList.Count > 0)
            {
                m_recordList.AddRange(director.GetResult().RecordList);
            }
        }

        /// <summary>
        /// �ۑ�����
        /// </summary>
        public void Save()
        {
            var director = new RecordTableESDirector<SfKingdomRecord>(new ESSaveBuilder<SfKingdomRecord>("SfKingdomRecordTable", this));
            director.Construct();
        }
    }
}