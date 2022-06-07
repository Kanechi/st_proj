using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    /// �̈�̊J��V�X�e���͖����A�J��V�X�e���͒n��ɂ�������
    /// </summary>
    [Serializable]
    public class SfDominionRecord
    {
        /// <summary>
        /// �̈�ڍ�
        /// </summary>
        [Serializable]
        public class SfKingomDetail
        {
            // �̈� ID
            public uint m_id = 0;
            // �̈於
            public string m_name = "";
            // �n�� ID ���X�g
            public List<uint> m_sfAreaIdList = new List<uint>();
            // true...�����ς�
            public bool m_ruleFlag = false;
            // true...

            // �אڒn�`�t���O
            public eAdjacentTerrainType m_adjacentTerrainType = 0;
        }
    }

    /// <summary>
    /// �̈�
    /// </summary>
    public class SfDominion : MonoBehaviour
    {
        
    }
}