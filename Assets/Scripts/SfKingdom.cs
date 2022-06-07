using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// �������R�[�h
    /// </summary>
    [Serializable]
    public class SfKingdomRecord
    {
        [Serializable]
        public class SfKingdomDetail
        {
            // ���� ID
            public uint m_id = 0;
            // ������
            public string m_name = "";
            // �̈� ID ���X�g
            public List<uint> m_sfDominionIdList = new List<uint>();
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public class SfKingdom : MonoBehaviour
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
}