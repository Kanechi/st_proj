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
        /// �����̈�ڍ�(�X�N���[���r���[�P��)
        /// </summary>
        [Serializable]
        public class DominionDetail
        {
            // �̈� ID
            public uint m_id = 0;

            // �̈於
            public string m_name = "";

            // �����̈悪�����Ă��� tgs �� territory index
            public int m_territoryIndex = 0;

            // �������Ă���n�� ID ���X�g
            public List<uint> m_areaIdList = new List<uint>();
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

        /// <summary>
        /// �S�̓y���X�g
        /// </summary>
        [Serializable]
        public class TerritoryDetail
        {
            //public List<int> terrainIndex
        }

        public void Parse(IDictionary<string, object> data)
        {
        }
    }
}