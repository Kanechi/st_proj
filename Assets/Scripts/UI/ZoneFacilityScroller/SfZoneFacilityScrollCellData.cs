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
        // 区域施設建設選択スクロールに設定されている施設セルの情報
        public SfZoneFacilityRecord ZoneFacilityRecord { get; private set; } = null;

        // タッチした区域セルのデータ
        public SfZoneCellData ZoneCellData { get; private set; } = null;

        // 施設画像
        public Sprite FacilitySprite { get; private set; } = null;



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
        public void CheckButtonEnable() 
        {
            var area = SfAreaTableManager.Instance.Get(ZoneCellData.AreaId);

            if (ZoneCellData.ZoneFacilityType == ZoneFacilityRecord.Type)
            {
                // 地域に建設されている施設とセルの施設が同じ場合は拡張ボタン

                if (ZoneCellData.ExpansionCount < SfConfigController.ZONE_MAX_EXPANTION_COUNT)
                {
                    if (SfAreaTableManager.Instance.CheckCostForBuildingFacility(area, ZoneFacilityRecord))
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
                        EnableExpationBtn = true;
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
            else if (ZoneCellData.ZoneFacilityType == eZoneFacilityType.None)
            {
                // 何も建設されていない場合
                if (SfAreaTableManager.Instance.CheckCostForBuildingFacility(area, ZoneFacilityRecord))
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
                if (SfAreaTableManager.Instance.CheckCostForBuildingFacility(area, ZoneFacilityRecord))
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record">表示する区域施設</param>
        /// <param name="zoneCellData">タッチした区域セルのデータ</param>
        public SfZoneFacilityScrollCellData(SfZoneFacilityRecord record, SfZoneCellData zoneCellData)
        {
            ZoneFacilityRecord = record;

            ZoneCellData = zoneCellData;

            FacilitySprite = ZoneFacilityRecord.FacilitySprite;

            CheckButtonEnable();
        }
    }
}