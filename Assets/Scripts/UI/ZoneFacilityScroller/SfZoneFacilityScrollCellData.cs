using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// ���{�݌��ݑI���X�N���[���Z���f�[�^
    /// </summary>
    public class SfZoneFacilityScrollCellData
    {
        public SfZoneFacilityRecord m_record = null;

        // �^�b�`�������Z���̃f�[�^
        public SfZoneCellData m_zoneCellData = null;

        // �{�݉摜
        public Sprite m_facilitySprite = null;

        // true...Build �{�^���\��
        public bool EnableBuildBtn { get; set; } = false;

        // true...NotBuild �摜�\��
        public bool EnableNotBuildImage { get; set; } = false;

        // true...Expantion �{�^���\��
        public bool EnableExpationBtn { get; set; } = false;

        // true...�g���s��(�g�����\�ȏ�Ԃŏ���������Ȃ����)
        public bool DisableExpantionImage { get; set; } = false;

        // true...�g���ő�
        public bool MaxExpantionImage { get; set; } = false;

        public SfZoneFacilityScrollCell Cell { get; set; } = null;
        public bool IsSelected { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record">�\��������{��</param>
        /// <param name="zoneCellData">�^�b�`�������Z���̃f�[�^</param>
        public SfZoneFacilityScrollCellData(SfZoneFacilityRecord record, SfZoneCellData zoneCellData)
        {
            m_record = record;

            m_zoneCellData = zoneCellData;

            m_facilitySprite = m_record.FacilitySprite;

            var areaRecord = SfAreaRecordTableManager.Instance.Get(zoneCellData.AreaId);

            if (zoneCellData.ZoneType == record.Type)
            {
                // �n��Ɍ��݂���Ă���{�݂ƃZ���̎{�݂������ꍇ�͊g���{�^��

                if (zoneCellData.ExpansionCount < SfConfigController.ZONE_MAX_EXPANTION_COUNT)
                {
                    if (SfAreaRecordTableManager.Instance.CheckCostForBuildingFacility(areaRecord, record))
                    {
                        // �g���\
                        EnableBuildBtn = false;
                        EnableNotBuildImage = false;
                        EnableExpationBtn = true;
                        DisableExpantionImage = false;
                        MaxExpantionImage = false;
                    }
                    else
                    {
                        // �g���s��
                        EnableBuildBtn = false;
                        EnableNotBuildImage = false;
                        EnableExpationBtn = false;
                        DisableExpantionImage = true;
                        MaxExpantionImage = false;
                    }
                }
                else
                {
                    // �g���ő�
                    EnableBuildBtn = false;
                    EnableNotBuildImage = false;
                    EnableExpationBtn = false;
                    DisableExpantionImage = false;
                    MaxExpantionImage = true;
                }
            }
            else if (zoneCellData.ZoneType == eZoneFacilityType.None)
            {
                // �������݂���Ă��Ȃ��ꍇ
                if (SfAreaRecordTableManager.Instance.CheckCostForBuildingFacility(areaRecord, record))
                {
                    // ���݉\
                    EnableBuildBtn = true;
                    EnableNotBuildImage = false;
                    EnableExpationBtn = false;
                    DisableExpantionImage = false;
                    MaxExpantionImage = false;
                }
                else
                {
                    // ���ݕs��
                    EnableBuildBtn = false;
                    EnableNotBuildImage = true;
                    EnableExpationBtn = false;
                    DisableExpantionImage = false;
                    MaxExpantionImage = false;
                }
            }
            else
            {
                // �������݂���Ă��邪�����{�݂ł͂Ȃ��ꍇ�͎{�݂̕ύX�ɂȂ�
                if (SfAreaRecordTableManager.Instance.CheckCostForBuildingFacility(areaRecord, record))
                {
                    // ���݉\
                    EnableBuildBtn = true;
                    EnableNotBuildImage = false;
                    EnableExpationBtn = false;
                    DisableExpantionImage = false;
                    MaxExpantionImage = false;
                }
                else
                {
                    // ���ݕs��
                    EnableBuildBtn = false;
                    EnableNotBuildImage = true;
                    EnableExpationBtn = false;
                    DisableExpantionImage = false;
                    MaxExpantionImage = false;
                }
            }
        }
    }
}