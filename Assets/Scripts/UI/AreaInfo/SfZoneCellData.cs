using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace sfproj
{
    // 区域セルデータ
    [Serializable]
    public class SfZoneCellData
    {
        // この区域セルの存在する地域 ID
        private uint m_areaId = 0;
        public uint AreaId => m_areaId;

        // セルインデックス
        [ShowInInspector, ReadOnly]
        private int m_cellIndex = 0;
        public int CellIndex { get => m_cellIndex; set => m_cellIndex = value; }

        // 区域タイプ(None じゃない場合は Add アイコン解除)
        [ShowInInspector, ReadOnly]
        private eZoneFacilityType m_zoneType = eZoneFacilityType.None;
        public eZoneFacilityType ZoneType { get => m_zoneType; set => m_zoneType = value; }

        // 拡張数
        [ShowInInspector, ReadOnly]
        private int m_expansionCount = 0;
        public int ExpansionCount { get => m_expansionCount; set => m_expansionCount = value; }

        // true...解放されている(鍵アイコン解除)
        private bool m_unlockFlag = false;
        public bool UnlockFlag { get => m_unlockFlag; set => m_unlockFlag = value; }

        public SfZoneCell Cell { get; set; } = null;

        public SfZoneCellData(uint areaId, int index, eZoneFacilityType zoneType, int expansionCt) {
            m_areaId = areaId;
            m_cellIndex = index;
            m_zoneType = zoneType;
            m_expansionCount = expansionCt;
        }
    }
}