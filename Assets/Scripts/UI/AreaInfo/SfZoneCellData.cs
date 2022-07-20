using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace sfproj
{
    // ���Z���f�[�^
    [Serializable]
    public class SfZoneCellData
    {
        // ���̋��Z���̑��݂���n�� ID
        private uint m_areaId = 0;
        public uint AreaId => m_areaId;

        // �Z���C���f�b�N�X
        [ShowInInspector, ReadOnly]
        private int m_cellIndex = 0;
        public int CellIndex { get => m_cellIndex; set => m_cellIndex = value; }

        // ���^�C�v(None ����Ȃ��ꍇ�� Add �A�C�R������)
        [ShowInInspector, ReadOnly]
        private eZoneFacilityType m_zoneType = eZoneFacilityType.None;
        public eZoneFacilityType ZoneType { get => m_zoneType; set => m_zoneType = value; }

        // �g����
        [ShowInInspector, ReadOnly]
        private int m_expansionCount = 0;
        public int ExpansionCount { get => m_expansionCount; set => m_expansionCount = value; }

        // true...�������Ă���(���A�C�R������)
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