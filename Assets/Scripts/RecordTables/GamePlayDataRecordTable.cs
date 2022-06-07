using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Sirenix.OdinInspector;

namespace stproj {

    /// <summary>
    /// �Q�[���v���C�ڍ�
    /// </summary>
    [Serializable]
    public class GamePlayDetailRecord : IJsonParser
    {
        // ���� ID
        [SerializeField]
        private uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // �ۑ��ԍ�
        [SerializeField]
        private uint m_saveNo = 0;
        public uint SaveNo { get => m_saveNo; set => m_saveNo = value; }

        /// <summary>
        /// �y�n�ڍ�
        /// </summary>
        [Serializable]
        public class LandDetail
        {
            // tgs �̃V�[�h�l
            public int m_tgsSeedValue = 0;

            // �y�n�^�C�v
            public eLandType m_landType = eLandType.OneLand;

            // �y�n�T�C�Y
            public int m_landSize = 0;
        }

        /// <summary>
        /// �n��ڍ�(�Z���P��)
        /// </summary>
        [SerializeField]
        public class AreaDetail
        {
            // �n�� ID
            public uint m_id = 0;

            // �n�於
            public string m_name = "";

            // ���ő吔
            public int m_maxZoneCount = 0;

            // �ݒu���Ă�����^�C�v
            public List<eZoneType> m_zoneTypeList = new List<eZoneType>();
        }

        /// <summary>
        /// �̈�ڍ�(�X�N���[���r���[�P��)
        /// �ǂ����̉����ɓ�������Ă���̓y�̏��
        /// </summary>
        [Serializable]
        public class DominionDetail
        {
            // �̈� ID
            public uint m_id = 0;

            // �̈悪�����Ă��� tgs �� territory index
            public int m_territoryIndex = 0;

            // �������Ă���n�� ID ���X�g
            public List<uint> m_areaIdList = new List<uint>();
        }

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
        /// �̓y�ڍ�
        /// </summary>
        [Serializable]
        public class TerritoryDetail
        {
            // tgs �� terrainIndex
            public int m_terrainIndex = 0;

            // ���� ID(0...��������Ă��Ȃ�)
            public uint m_kingdomId = 0;

            // �̈� ID(0...��������Ă��Ȃ�)
            public uint m_dominionId = 0;

            // �̓y��
            public string m_name = "";

            // �אڒn�`�t���O
            public eAdjacentTerrainType m_adjacentTerrainTypeFlag = 0;
        }

        /// <summary>
        /// �����ڍ�
        /// </summary>
        [Serializable]
        public class KingdomDetail
        {
            // ���� ID
            public uint m_id = 0;

            // ������
            public string m_name = "";

            // �����̓y ID ���X�g
            public List<uint> m_dominionIdList = new List<uint>();
        }

        public void Parse(IDictionary<string, object> data)
        {
        }
    }
}