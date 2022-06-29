using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using TGS;

namespace sfproj
{
    /// <summary>
    /// �̈�ڍ׃��R�[�h
    /// �̈�̊J��V�X�e���͖���
    /// �ۑ����
    /// �Q�[���J�n���Ɉ�x��������
    /// �אڂ��Ă���Γ������邩���Ȃ�����I��
    /// ����������ē������\
    /// </summary>
    [Serializable]
    public class SfDominionRecord
    {
        // �̈� ID
        public uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // ������݂ĉ��ԖڂɎ�ɓ��ꂽ�̈�Ȃ̂��̔ԍ�
        // �D��ꂽ����Ԃ����肷��Ɣԍ��͕ς��
        public int m_dominionIndex = 0;
        public int DominionIndex { get => m_dominionIndex; set => m_dominionIndex = value; }

        // �̈於
        public string m_name = "";
        public string Name { get => m_name; set => m_name = value; }

        // �e���g���C���f�b�N�X
        public int m_territoryIndex = -1;
        public int TerritoryIndex { get => m_territoryIndex; set => m_territoryIndex = value; }

        // true...�����ς�
        public bool m_ruleFlag = false;
        public bool RuleFlag { get => m_ruleFlag; set => m_ruleFlag = value; }


        // �������Ă��鉤�� ID (-1...��������Ă��Ȃ�)
        public int m_kingdomId = -1;
        public int GovernKingdomId { get => m_kingdomId; set => m_kingdomId = value; }

        // true...��s
        public bool m_capitalFlag = false;
        public bool CapitalFlag { get => m_capitalFlag; set => m_capitalFlag = value; }

        // true...�C�ɗאڂ��Ă���
        public bool m_neighboursOceanFlag = false;
        public bool NeighboursOceanFlag { get => m_neighboursOceanFlag; set => m_neighboursOceanFlag = value; }

        // �n�� ID ���X�g
        public List<uint> m_sfAreaIdList = new List<uint>();
        public List<uint> AreaIdList { get => m_sfAreaIdList; set => m_sfAreaIdList = value; }
    }

    /// <summary>
    /// �̈搶���H��  ���
    /// 2022/06/08
    ///     �����ނ����݂���킯�ł͂Ȃ��̂ŕ�����K�v�͂Ȃ�����
    ///     �ꉞ�e���v���[�g�����Ă���
    /// </summary>
    public abstract class SfDominionFactoryBase
    {
        public SfDominionRecord Create(uint uniqueId, int territoryIndex)
        {
            var record = CreateRecord();

            // ���j�[�N ID �̐ݒ�
            record.Id = uniqueId;

            // �̈於�̐���
            record.Name = CreateName();

            // �e���g���C���f�b�N�X�̐ݒ�
            record.TerritoryIndex = territoryIndex;

            // �C�ɗאڂ��Ă��邩�ǂ����̃t���O��ݒ�
            record.m_neighboursOceanFlag = CheckAdjastingOceanTerrain(territoryIndex);

            return record;
        }

        // �̈惌�R�[�h�̍쐬
        protected abstract SfDominionRecord CreateRecord();

        // �̈於�̍쐬
        protected abstract string CreateName();

        /// <summary>
        /// �C�ɗאڂ��Ă��邩�`�F�b�N
        /// �אڂ��Ă���e���g���ɂP�ł���\��������ΊC
        /// </summary>
        /// <returns>true...�C�ɗאڂ��Ă���</returns>
        private bool CheckAdjastingOceanTerrain(int territoryIndex)
        {
            var territory = TGS.TerrainGridSystem.instance.territories[territoryIndex];

            var neighbours = territory.neighbours;

            foreach (var t in neighbours)
            {
                if (t.visible == false)
                    return true;
            }

            return false;
        }
    }

    /// <summary>
    /// �̈惌�R�[�h�����H��
    /// </summary>
    public abstract class SfDominionCreateFactory : SfDominionFactoryBase
    {
        protected override SfDominionRecord CreateRecord()
        {
            return new SfDominionRecord();
        }
    }

    /// <summary>
    /// �̈搶���H��
    /// </summary>
    public class SfDominionFactory : SfDominionCreateFactory
    {
        // ���̕�����n��ɂ���ĕω�������H
        protected override string CreateName()
        {
            return "test";
        }


    }

    /// <summary>
    /// �̈�H��Ǘ�
    /// </summary>
    public class SfDominionFactoryManager : Singleton<SfDominionFactoryManager>{

        /// <summary>
        /// �̈惌�R�[�h�̍쐬
        /// </summary>
        /// <param name="territoryIndex"></param>
        /// <returns></returns>
        public SfDominionRecord Create(int territoryIndex) {

            // �ЂƂ܂� SfDominionFactory �����Ȃ�
            var factory = new SfDominionFactory();

            // ���j�[�N ID �̍쐬
            uint uniqueId = SfConstant.CreateUniqueId(ref SfDominionRecordTableManager.Instance.m_uniqueIdList);

            // �̈惌�R�[�h���쐬
            var record = factory.Create(uniqueId, territoryIndex);

            return record;
        }
    }



    /// �̈� �Ǘ�
    /// �v���C���ɐ�������Ă��邷�ׂĂ� SfDominionRecord
    /// �ۑ����͕ʃt�@�C���̕ʃN���X�Ɏ���
    /// </summary>
    public class SfDominionRecordTable : RecordTable<SfDominionRecord>
    {
        // ���j�[�N ID ���X�g
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        // �o�^
        public void Regist(SfDominionRecord record) => RecordList.Add(record);

        // �̈惌�R�[�h�̎擾
        public override SfDominionRecord Get(uint id) => RecordList.Find(r => r.Id == id);

        public SfDominionRecord GetAtTerritoryIndex(int territoryIndex) => RecordList.Find(r => r.TerritoryIndex == territoryIndex);

        /// <summary>
        /// �����ς݂̃t���O�̕ύX
        /// </summary>
        /// <param name="dominionId"></param>
        /// <param name="ruleFlag"></param>
        public void ChangeRuleFlag(uint dominionId, bool ruleFlag) => Get(dominionId).RuleFlag = ruleFlag;

        /// <summary>
        /// ���� ID �̕ύX
        /// </summary>
        /// <param name="dominionId"></param>
        /// <param name="kingdomId"></param>
        public void ChangeKingdomId(uint dominionId, int kingdomId) => Get(dominionId).GovernKingdomId = kingdomId;

        /// <summary>
        /// ���_�t���O�̕ύX
        /// </summary>
        /// <param name="dominionId"></param>
        /// <param name="capitalFlag"></param>
        public void ChangeCapitalFlag(uint dominionId, bool capitalFlag) => Get(dominionId).CapitalFlag = capitalFlag;

        /// <summary>
        /// �C���f�b�N�X�̈�ԏ������n�� ID ���擾
        /// </summary>
        /// <param name="dominionId"></param>
        /// <param name="groupType"></param>
        /// <returns></returns>
        public uint GetMinimumCellIndexArea(uint dominionId, eAreaGroupType groupType = eAreaGroupType.None) {
            var areaIdList = Get(dominionId).AreaIdList;
            uint minimumAreaId = 0;
            foreach (uint areaId in areaIdList) {
                SfAreaRecord areaRecord = SfAreaRecordTableManager.Instance.Get(areaId);
                if (areaRecord.AreaGroupType == groupType) {
                    minimumAreaId = areaId;
                    break;
                }
            }
            return minimumAreaId;
        }
    }

    /// <summary>
    /// �̈�Ǘ�
    /// </summary>
    public class SfDominionRecordTableManager : SfDominionRecordTable
    {
        private static SfDominionRecordTableManager s_instance = null;

        public static SfDominionRecordTableManager Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;

                s_instance = new SfDominionRecordTableManager();
                s_instance.Load();
                return s_instance;
            }
        }

        /// <summary>
        /// �ǂݍ��ݏ���
        /// </summary>
        public void Load()
        {
            RecordTableESDirector<SfDominionRecord> director = new RecordTableESDirector<SfDominionRecord>(new ESLoadBuilder<SfDominionRecord, SfDominionRecordTable>("SfDominionRecordTable"));
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
            var director = new RecordTableESDirector<SfDominionRecord>(new ESSaveBuilder<SfDominionRecord>("SfDominionRecordTable", this));
            director.Construct();
        }
    }
}