using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    // 区域セルデータ
    public class SfZoneCellData
    {
        // セルインデックス
        private int m_cellIndex = 0;
        public int CellIndex { get => m_cellIndex; set => m_cellIndex = value; }

        // 区域タイプ(None じゃない場合は Add アイコン解除)
        private eZoneType m_zoneType = eZoneType.None;
        public eZoneType ZoneType { get => m_zoneType; set => m_zoneType = value; }

        // 拡張数
        private int m_expansionCount = 0;
        public int ExpansionCount { get => m_expansionCount; set => m_expansionCount = value; }

        // true...解放されている(鍵アイコン解除)
        private bool m_unlockFlag = false;
        public bool UnlockFlag { get => m_unlockFlag; set => m_unlockFlag = value; }

        public SfZoneCell Cell { get; set; } = null;

        public SfZoneCellData(int index, eZoneType zoneType, int expansionCt) {
            m_cellIndex = index;
            m_zoneType = zoneType;
            m_expansionCount = expansionCt;
        }
    }
}