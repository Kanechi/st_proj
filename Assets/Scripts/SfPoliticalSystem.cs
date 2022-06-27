using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// ­¡Œ`‘Ô
    /// </summary>
    public enum ePoliticalForm
    {
        // ‰¤­
        Monarchy,
    }

    /// <summary>
    /// ­¡‘ÌŒn
    /// stellaris ‚ÅŒ¾‚¤Š‚Ì‹NŒ¹‚â‚ç­ô‚â‚ç‘¥‚â‚ç
    /// </summary>
    [Serializable]
    public class SfPoliticalSystem
    {
        public uint m_id;
        public uint Id { get => m_id; set => m_id = value; }
    }
}