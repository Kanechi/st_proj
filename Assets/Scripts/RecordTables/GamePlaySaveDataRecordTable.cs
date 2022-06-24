using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Sirenix.OdinInspector;

namespace sfproj {

    /// <summary>
    /// �Q�[���v���C�Z�[�u�f�[�^
    /// </summary>
    [Serializable]
    public class GamePlaySaveDataRecord : IJsonParser
    {
        // ���� ID
        [SerializeField]
        private uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // �ۑ��ԍ�
        [SerializeField]
        private uint m_saveNo = 0;
        public uint SaveNo { get => m_saveNo; set => m_saveNo = value; }

        

        public void Parse(IDictionary<string, object> data)
        {
        }
    }
}