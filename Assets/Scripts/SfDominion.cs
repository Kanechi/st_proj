using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace sfproj
{

    public enum eAdjacentTerrainType : uint
    {
        // 平地に隣接
        Plane = 1u << 0,
        // 森に隣接
        Forest = 1u << 1,
        // 海に隣接
        Ocean = 1u << 2,
        // 山に隣接
        Mountain = 1u << 3,
        // 川に隣接
        River = 1u << 4,
    }

    /// <summary>
    /// 領域詳細レコード
    /// 領域の開拓システムは無し、開拓システムは地域にたいして
    /// </summary>
    [Serializable]
    public class SfDominionRecord
    {
        /// <summary>
        /// 領域詳細
        /// </summary>
        [Serializable]
        public class SfKingomDetail
        {
            // 領域 ID
            public uint m_id = 0;
            // 領域名
            public string m_name = "";
            // 地域 ID リスト
            public List<uint> m_sfAreaIdList = new List<uint>();
            // true...統治済み
            public bool m_ruleFlag = false;
            // true...

            // 隣接地形フラグ
            public eAdjacentTerrainType m_adjacentTerrainType = 0;
        }
    }

    /// <summary>
    /// 領域
    /// </summary>
    public class SfDominion : MonoBehaviour
    {
        
    }
}