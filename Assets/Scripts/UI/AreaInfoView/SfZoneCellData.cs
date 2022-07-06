using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    // ���Z���f�[�^
    public class SfZoneCellData
    {
        // �Z���C���f�b�N�X
        private int m_cellIndex = 0;
        public int CellIndex { get => m_cellIndex; set => m_cellIndex = value; }

        // ���^�C�v
        private eZoneType m_zoneType = eZoneType.None;
        public eZoneType ZoneType { get => m_zoneType; set => m_zoneType = value; }

        // �g����
        private int m_expansionCount = 0;
        public int ExpansionCount { get => m_expansionCount = 0; set => m_expansionCount = value; }

        // true...�A�����b�N
        private bool m_unlockFlag = false;
        public bool UnlockFlag { get => m_unlockFlag; set => m_unlockFlag = value; }

        public SfZoneCell Cell { get; set; } = null;

        public SfZoneCellData(eZoneType zoneType, int expansionCt, bool unlockFlag) {
            m_zoneType = zoneType;
            m_expansionCount = expansionCt;
            m_unlockFlag = unlockFlag;
        }
    }
}