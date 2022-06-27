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
        // ���� ID
        public uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // ������
        public string m_name = "";
        public string Name { get => m_name; set => m_name = value; }

        // �����̐F
        public Color m_color = Color.white;
        public Color Color { get => m_color; set => m_color = value; }

        // �̈� ID ���X�g
        public List<uint> m_sfDominionIdList = new List<uint>();
        public List<uint> DominionIdList { get => m_sfDominionIdList; set => m_sfDominionIdList = value; }

        // �����̐�

        // 
    }

    public abstract class SfKingdomFactoryBase
    {
        public SfKingdomRecord Create(uint uniqueId)
        {
            var record = CreateRecord();

            record.Id = uniqueId;

            record.Name = CreateName();

            return record;
        }

        protected abstract SfKingdomRecord CreateRecord();

        protected abstract string CreateName();
    }

    public abstract class SfKingdomFactory : SfKingdomFactoryBase
    {
        protected override SfKingdomRecord CreateRecord()
        {
            return new SfKingdomRecord();
        }
    }

    // �����̐���


    // ���̑��̍��̃����_������
    // ������x�̍������O�ɍ쐬���Ă����Ċ���U�邾����
    // �Ƃǂ߂邩�A���ׂĂO����쐬���邩�E�E�E

    /// <summary>
    /// �����H��Ǘ�
    /// </summary>
    public class SfKingdomFactoryManager : Singleton<SfKingdomFactoryManager>
    { 
    }

    /// <summary>
    /// �������R�[�h�Ǘ�
    /// �v���C���ɐ�������Ă��邷�ׂĂ� SfKingdomRecord
    /// </summary>
    public class SfKingdomRecordTable : RecordTable<SfKingdomRecord>
    {
        // ���j�[�N ID ���X�g
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

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