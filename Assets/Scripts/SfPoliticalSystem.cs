using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// �����`��
    /// </summary>
    public enum ePoliticalForm
    {
        // ����
        Monarchy,
    }

    /// <summary>
    /// �����̌n
    /// stellaris �Ō������̋N����琭���獑�����
    /// </summary>
    [Serializable]
    public class SfPoliticalSystem
    {
        public uint m_id;
        public uint Id { get => m_id; set => m_id = value; }
    }
}