using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{


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

        // ������
        public string m_name = "";
        public string Name { get => m_name; set => m_name = value; }

        // �����̐F
        public Color m_color = Color.white;
        public Color Color { get => m_color; set => m_color = value; }

        // true...�����̍�
        public bool m_selfFlag = false;
        public bool SelfFlag { get => m_selfFlag; set => m_selfFlag = value; }

        // �̈� ID ���X�g
        public List<uint> m_sfDominionIdList = new List<uint>();
        public List<uint> DominionIdList { get => m_sfDominionIdList; set => m_sfDominionIdList = value; }

        // �����̐�
        public uint m_population = 0;
        public uint Population { get => m_population; set => m_population = value; }
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
            record.Name = ConfigController.Instance.KingdomName;
        }

        // �����J���[�̐ݒ�
        protected override void SettingColor(SfKingdomRecord record) {
            record.Color = ConfigController.Instance.KingdomColor;
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