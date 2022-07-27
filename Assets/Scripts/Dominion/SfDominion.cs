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
    public class SfDominion
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



    /// �̈� �Ǘ�
    /// �v���C���ɐ�������Ă��邷�ׂĂ� SfDominion
    /// �ۑ����͕ʃt�@�C���̕ʃN���X�Ɏ���
    /// </summary>
    public class SfDominionTable : RecordTable<SfDominion>
    {


        // �o�^
        public void Regist(SfDominion record) => RecordList.Add(record);

        // �̈惌�R�[�h�̎擾
        public override SfDominion Get(uint id) => RecordList.Find(r => r.Id == id);

        public SfDominion GetAtTerritoryIndex(int territoryIndex) => RecordList.Find(r => r.TerritoryIndex == territoryIndex);

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
                SfArea areaRecord = SfAreaTableManager.Instance.Table.Get(areaId);
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
    public class SfDominionTableManager : Singleton<SfDominionTableManager>
    {
        private SfDominionTable m_table = new SfDominionTable();
        public SfDominionTable Table => m_table;

        // ���j�[�N ID ���X�g
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        /// <summary>
        /// �ǂݍ��ݏ���
        /// </summary>
        public void Load()
        {
            RecordTableESDirector<SfDominion> director = new RecordTableESDirector<SfDominion>(new ESLoadBuilder<SfDominion, SfDominionTable>("SfDominionTable"));
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
            var director = new RecordTableESDirector<SfDominion>(new ESSaveBuilder<SfDominion>("SfDominionTable", m_table));
            director.Construct();
        }
    }
}