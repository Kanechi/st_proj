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
        // �e�̋��r���[
        // SfZoneView ���琶�������Ƃ��ɂ̂ݎ��t�����s��
        public SfZoneView ZoneView { get; private set; } = null;
        public void SetZoneView(SfZoneView zoneView) => ZoneView = zoneView;

        // ���{��(null == ���݂���Ă��Ȃ�)
        private SfZoneFacility m_zoneFacility = null;
        public SfZoneFacility ZoneFacility { get => m_zoneFacility; set => m_zoneFacility = value; }

        // �n�� ID
        [ShowInInspector, ReadOnly]
        private uint m_areaId = 0;
        public uint AreaId => m_areaId;

        // ���Z���C���f�b�N�X
        [ShowInInspector, ReadOnly]
        private int m_cellIndex = -1;
        public int CellIndex => m_cellIndex;

        // ���^�C�v(None ����Ȃ��ꍇ�� Add �A�C�R������)
        [ShowInInspector, ReadOnly]
        public eZoneFacilityType ZoneFacilityType => m_zoneFacility != null ? m_zoneFacility.FacilityType : eZoneFacilityType.None;

        // �g����
        [ShowInInspector, ReadOnly]
        public int ExpansionCount => m_zoneFacility != null ? m_zoneFacility.ExpantionCount : 0;

        // true...�������Ă���(���A�C�R������)
        private bool m_unlockFlag = false;
        public bool UnlockFlag { get => m_unlockFlag; set => m_unlockFlag = value; }

        public SfZoneCell Cell { get; set; } = null;

        public SfZoneCellData(SfZoneFacility zoneFacility) {
            Set(zoneFacility);
        }

        public SfZoneCellData(uint areaId, int cellIndex)
        {
            m_areaId = areaId;
            m_cellIndex = cellIndex;
        }

        public void Set(SfZoneFacility zoneFacility)
        {
            m_areaId = zoneFacility.AreaId;
            m_cellIndex = zoneFacility.CellIndex;
            m_zoneFacility = zoneFacility;
        }

        public void Set(uint areaId, int cellIndex)
        {
            m_areaId = areaId;
            m_cellIndex = cellIndex;
        }

        /// <summary>
        /// �{�݃^�C�v�̕ύX
        /// </summary>
        /// <param name="type"></param>
        public void ChangeFacilityType(eZoneFacilityType type) {

            // ���{�݃e�[�u����ύX
            if (m_zoneFacility != null)
            {
                SfZoneFacilityTableManager.Instance.Table.ChangeZoneFacilityType(m_zoneFacility.AreaId, m_zoneFacility.CellIndex, type);
                SfZoneFacilityTableManager.Instance.Table.SetZoneFacilityExpantion(m_zoneFacility.AreaId, m_zoneFacility.CellIndex, 1);
            }
            else
            {
                SfZoneFacilityTableManager.Instance.Table.BuildZoneFacilityType(m_areaId, m_cellIndex, type, 1);
            }

            // �ύX�����������̃f�[�^�ɐݒ�
            m_zoneFacility = SfZoneFacilityTableManager.Instance.Table.Get(m_areaId, m_cellIndex);

            // �Z���̃{�^���L�����̕ύX�Ǝ{�݉摜��ύX
            Cell.SettingZoneButtonEnable();
        }
    }
}