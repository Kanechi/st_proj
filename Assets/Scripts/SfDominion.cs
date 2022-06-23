using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{
    /// <summary>
    /// �̈�ɑ��݂���n�`
    /// �n�`�ɂ���ċ��ɐݒu�ł�����̂�g���[�h�Ŕ̔����Ă�����̂Ȃǂ��낢��Ȃ��̂��ω�����
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

        // �̈於
        public string m_name = "";
        public string Name { get => m_name; set => m_name = value; }

        // �e���g���C���f�b�N�X
        public int m_territoryIndex = -1;
        public int TerritoryIndex { get => m_territoryIndex; set => m_territoryIndex = value; }

        // true...�����ς�
        public bool m_ruleFlag = false;
        public bool RuleFlag { get => m_ruleFlag; set => m_ruleFlag = value; }


        // �������Ă��鉤�� ID (0...��������Ă��Ȃ�)
        public uint m_kingdomId = 0;
        public uint GovernKingdomId { get => m_kingdomId; set => m_kingdomId = value; }

        // true...��s
        public bool m_capitalFlag = false;
        public bool CapitalFlag { get => m_capitalFlag; set => m_capitalFlag = value; }

        // �̈�ɑ��݂���n�`
        public eExistingTerrain m_existingTerrain = 0;
        public eExistingTerrain ExistingTerrain { get => m_existingTerrain; set => m_existingTerrain = value; }
        // �n�� ID ���X�g
        public List<uint> m_sfAreaIdList = new List<uint>();
        public List<uint> AreaIdList { get => m_sfAreaIdList; set => m_sfAreaIdList = value; }
    }

    /// <summary>
    /// �̈�
    /// �X�N���[���r���[�̏��
    /// </summary>
    public class SfDominion : MonoBehaviour
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

            // �אڒn�`�^�C�v�̐ݒ�
            record.ExistingTerrain = SettingExistingTerrain(territoryIndex);

            return record;
        }

        // �̈惌�R�[�h�̍쐬
        protected abstract SfDominionRecord CreateRecord();

        // �̈於�̍쐬
        protected abstract string CreateName();

        // �אڒn�`�^�C�v�̐ݒ�
        protected abstract eExistingTerrain SettingExistingTerrain(int territoryIndex);
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

        // ���݂���n�`�̐ݒ�
        protected override eExistingTerrain SettingExistingTerrain(int territoryIndex)
        {
            eExistingTerrain terrain = 0;

            // ���̕����͊m��
            terrain |= eExistingTerrain.Plane;
            terrain |= eExistingTerrain.Mountain;
            terrain |= eExistingTerrain.Forest;
            terrain |= eExistingTerrain.River;

            if (CheckAdjastingOceanTerrain(territoryIndex))
            {
                terrain |= eExistingTerrain.Ocean;
            }

            return terrain;
        }

        /// <summary>
        /// �C�ɗאڂ��Ă��邩�`�F�b�N
        /// �אڂ��Ă���e���g���ɂP�ł���\��������ΊC
        /// </summary>
        /// <returns>true...�C�ɗאڂ��Ă���</returns>
        private bool CheckAdjastingOceanTerrain(int territoryIndex)
        {
            var territory = TGS.TerrainGridSystem.instance.territories[territoryIndex];

            var neighbours = territory.neighbours;

            foreach (var t in neighbours) {
                if (t.visible == false)
                    return true;
            }

            return false;
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