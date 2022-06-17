using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{

    public enum eAdjacentTerrainType : uint
    {
        // ���n�ɗא�
        Plane = 1u << 0,
        // �X�ɗא�
        Forest = 1u << 1,
        // �C�ɗא�
        Ocean = 1u << 2,
        // �R�ɗא�
        Mountain = 1u << 3,
        // ��ɗא�
        River = 1u << 4,
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
        /// <summary>
        /// �̈�ڍ�
        /// </summary>
        [Serializable]
        public class SfDominionDetail
        {
            // �̈� ID
            public uint m_id = 0;
            // �̈於
            public string m_name = "";
            // �e���g���C���f�b�N�X
            public int m_territoryIndex = -1;

            // true...�����ς�
            public bool m_ruleFlag = false;
            // �������Ă��鉤�� ID (0...��������Ă��Ȃ�)
            public uint m_kingdomId = 0;
            // true...��s
            public bool m_capitalFlag = false;

            // �אڒn�`�^�C�v
            public eAdjacentTerrainType m_adjacentTerrainType = 0;

            // �n�� ID ���X�g
            public List<uint> m_sfAreaIdList = new List<uint>();
        }

        [SerializeField]
        private SfDominionDetail m_detail = new SfDominionDetail();

        // �̈� ID
        public uint Id { get => m_detail.m_id; set => m_detail.m_id = value; }
        // �̈於
        public string Name { get => m_detail.m_name; set => m_detail.m_name = value; }
        // �e���g���C���f�b�N�X
        public int TerritoryIndex { get => m_detail.m_territoryIndex; set => m_detail.m_territoryIndex = value; }

        // �����ς݃t���O
        public bool RuleFlag { get => m_detail.m_ruleFlag; set => m_detail.m_ruleFlag = value; }
        // �������Ă��鉤�� ID (0...��������Ă��Ȃ�)
        public uint GovernKingdomId { get => m_detail.m_kingdomId; set => m_detail.m_kingdomId = value; }
        // ��s�t���O
        public bool CapitalFlag { get => m_detail.m_capitalFlag; set => m_detail.m_capitalFlag = value; }

        // �אڒn�`�^�C�v
        public eAdjacentTerrainType AdjacentTerrainType { get => m_detail.m_adjacentTerrainType; set => m_detail.m_adjacentTerrainType = value; }
        // �n�� ID ���X�g
        public List<uint> AreaIdList { get => m_detail.m_sfAreaIdList; set => m_detail.m_sfAreaIdList = value; }
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
            record.AdjacentTerrainType = CreateAdjacentTerrainType();

            return record;
        }

        // �̈惌�R�[�h�̍쐬
        protected abstract SfDominionRecord CreateRecord();

        // �̈於�̍쐬
        protected abstract string CreateName();

        // �אڒn�`�^�C�v�̐ݒ�
        protected abstract eAdjacentTerrainType CreateAdjacentTerrainType();
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

        // �אڒn�`�^�C�v�̐ݒ�
        protected override eAdjacentTerrainType CreateAdjacentTerrainType()
        {
            return eAdjacentTerrainType.Plane;
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