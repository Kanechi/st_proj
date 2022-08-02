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
    public class SfKingdom
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
    /// �������R�[�h�Ǘ�
    /// �v���C���ɐ�������Ă��邷�ׂĂ� SfKingdomRecord
    /// </summary>
    public class SfKingdomTable : RecordTable<SfKingdom>
    {

        // �o�^
        public void Regist(SfKingdom record) => RecordList.Add(record);

        // �̈惌�R�[�h�̎擾
        public override SfKingdom Get(uint id) => RecordList.Find(r => r.Id == id);

        // ���g�̉������擾
        public SfKingdom GetSelfKingdom() => RecordList.Find(r => r.m_selfFlag == true);

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

    public class SfKingdomTableManager : Singleton<SfKingdomTableManager>
    {
        private SfKingdomTable m_table = new SfKingdomTable();
        public SfKingdomTable Table => m_table;

        // ���j�[�N ID ���X�g
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        /// <summary>
        /// �ǂݍ��ݏ���
        /// </summary>
        public void Load()
        {
            RecordTableESDirector<SfKingdom> director = new RecordTableESDirector<SfKingdom>(new ESLoadBuilder<SfKingdom, SfKingdomTable>("SfKingdomRecordTable"));
            director.Construct();
            if (director.GetResult() != null && director.GetResult().RecordList.Count > 0)
            {
                m_table.RecordList.AddRange(director.GetResult().RecordList);
            }
        }

        /// <summary>
        /// �ۑ�����
        /// </summary>
        public void Save()
        {
            var director = new RecordTableESDirector<SfKingdom>(new ESSaveBuilder<SfKingdom>("SfKingdomRecordTable", m_table));
            director.Construct();
        }
    }
}