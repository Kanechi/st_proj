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
        // ���{�݌��ݑI���X�N���[���ɐݒ肳��Ă���{�݃Z���̏��
        public SfZoneFacilityRecord ZoneFacilityRecord { get; private set; } = null;

        // �^�b�`�������Z���̃f�[�^
        public SfZoneCellData ZoneCellData { get; private set; } = null;

        // �{�݉摜
        public Sprite FacilitySprite { get; private set; } = null;



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
        public void CheckButtonEnable() 
        {
            var area = SfAreaTableManager.Instance.Get(ZoneCellData.AreaId);

            if (ZoneCellData.ZoneFacilityType == ZoneFacilityRecord.Type)
            {
                // �n��Ɍ��݂���Ă���{�݂ƃZ���̎{�݂������ꍇ�͊g���{�^��

                if (ZoneCellData.ExpansionCount < SfConfigController.ZONE_MAX_EXPANTION_COUNT)
                {
                    if (SfAreaTableManager.Instance.CheckCostForBuildingFacility(area, ZoneFacilityRecord))
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
                        EnableExpationBtn = true;
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
            else if (ZoneCellData.ZoneFacilityType == eZoneFacilityType.None)
            {
                // �������݂���Ă��Ȃ��ꍇ
                if (SfAreaTableManager.Instance.CheckCostForBuildingFacility(area, ZoneFacilityRecord))
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
                if (SfAreaTableManager.Instance.CheckCostForBuildingFacility(area, ZoneFacilityRecord))
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record">�\��������{��</param>
        /// <param name="zoneCellData">�^�b�`�������Z���̃f�[�^</param>
        public SfZoneFacilityScrollCellData(SfZoneFacilityRecord record, SfZoneCellData zoneCellData)
        {
            ZoneFacilityRecord = record;

            ZoneCellData = zoneCellData;

            FacilitySprite = ZoneFacilityRecord.FacilitySprite;

            CheckButtonEnable();
        }
    }
}