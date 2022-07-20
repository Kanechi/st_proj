using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// 区域施設建設選択スクロールセルデータ
    /// </summary>
    public class SfZoneFacilityScrollCellData
    {
        public SfZoneFacilityRecord m_record = null;

        // タッチした区域セルのデータ
        public SfZoneCellData m_zoneCellData = null;

        // 施設画像
        public Sprite m_facilitySprite = null;

        // true...Build ボタン表示
        public bool EnableBuildBtn { get; set; } = false;

        // true...NotBuild 画像表示
        public bool EnableNotBuildImage { get; set; } = false;

        // true...Expantion ボタン表示
        public bool EnableExpationBtn { get; set; } = false;

        // true...拡張不可(拡張が可能な状態で条件が合わない状態)
        public bool DisableExpantionImage { get; set; } = false;

        // true...拡張最大
        public bool MaxExpantionImage { get; set; } = false;

        public SfZoneFacilityScrollCell Cell { get; set; } = null;
        public bool IsSelected { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record">表示する区域施設</param>
        /// <param name="zoneCellData">タッチした区域セルのデータ</param>
        public SfZoneFacilityScrollCellData(SfZoneFacilityRecord record, SfZoneCellData zoneCellData)
        {
            m_record = record;

            m_zoneCellData = zoneCellData;

            m_facilitySprite = m_record.FacilitySprite;

            var areaRecord = SfAreaRecordTableManager.Instance.Get(zoneCellData.AreaId);

            if (zoneCellData.ZoneType == record.Type)
            {
                // 地域に建設されている施設とセルの施設が同じ場合は拡張ボタン

                if (zoneCellData.ExpansionCount < SfConfigController.ZONE_MAX_EXPANTION_COUNT)
                {
                    if (SfAreaRecordTableManager.Instance.CheckCostForBuildingFacility(areaRecord, record))
                    {
                        // 拡張可能
                        EnableBuildBtn = false;
                        EnableNotBuildImage = false;
                        EnableExpationBtn = true;
                        DisableExpantionImage = false;
                        MaxExpantionImage = false;
                    }
                    else
                    {
                        // 拡張不可
                        EnableBuildBtn = false;
                        EnableNotBuildImage = false;
                        EnableExpationBtn = false;
                        DisableExpantionImage = true;
                        MaxExpantionImage = false;
                    }
                }
                else
                {
                    // 拡張最大
                    EnableBuildBtn = false;
                    EnableNotBuildImage = false;
                    EnableExpationBtn = false;
                    DisableExpantionImage = false;
                    MaxExpantionImage = true;
                }
            }
            else if (zoneCellData.ZoneType == eZoneFacilityType.None)
            {
                // 何も建設されていない場合
                if (SfAreaRecordTableManager.Instance.CheckCostForBuildingFacility(areaRecord, record))
                {
                    // 建設可能
                    EnableBuildBtn = true;
                    EnableNotBuildImage = false;
                    EnableExpationBtn = false;
                    DisableExpantionImage = false;
                    MaxExpantionImage = false;
                }
                else
                {
                    // 建設不可
                    EnableBuildBtn = false;
                    EnableNotBuildImage = true;
                    EnableExpationBtn = false;
                    DisableExpantionImage = false;
                    MaxExpantionImage = false;
                }
            }
            else
            {
                // 何か建設されているが同じ施設ではない場合は施設の変更になる
                if (SfAreaRecordTableManager.Instance.CheckCostForBuildingFacility(areaRecord, record))
                {
                    // 建設可能
                    EnableBuildBtn = true;
                    EnableNotBuildImage = false;
                    EnableExpationBtn = false;
                    DisableExpantionImage = false;
                    MaxExpantionImage = false;
                }
                else
                {
                    // 建設不可
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