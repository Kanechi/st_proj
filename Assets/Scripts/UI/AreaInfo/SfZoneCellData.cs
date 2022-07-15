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

        // ���^�C�v(None ����Ȃ��ꍇ�� Add �A�C�R������)
        private eZoneType m_zoneType = eZoneType.None;
        public eZoneType ZoneType { get => m_zoneType; set => m_zoneType = value; }

        // �g����
        private int m_expansionCount = 0;
        public int ExpansionCount { get => m_expansionCount; set => m_expansionCount = value; }

        // true...�������Ă���(���A�C�R������)
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