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
        // 親の区域ビュー
        // SfZoneView から生成されるときにのみ取り付けを行う
        public SfZoneView ZoneView { get; private set; } = null;
        public void SetZoneView(SfZoneView zoneView) => ZoneView = zoneView;

        // 区域施設(null == 建設されていない)
        private SfZoneFacility m_zoneFacility = null;
        public SfZoneFacility ZoneFacility { get => m_zoneFacility; set => m_zoneFacility = value; }

        // 地域 ID
        [ShowInInspector, ReadOnly]
        private uint m_areaId = 0;
        public uint AreaId => m_areaId;

        // 区域セルインデックス
        [ShowInInspector, ReadOnly]
        private int m_cellIndex = -1;
        public int CellIndex => m_cellIndex;

        // 区域タイプ(None じゃない場合は Add アイコン解除)
        [ShowInInspector, ReadOnly]
        public uint ZoneFacilityTypeId => m_zoneFacility != null ? m_zoneFacility.FacilityTypeId : 0;

        // 拡張数
        [ShowInInspector, ReadOnly]
        public int ExpansionCount => m_zoneFacility != null ? m_zoneFacility.ExpantionCount : 0;

        // true...解放されている(鍵アイコン解除)
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
        /// 施設タイプの変更
        /// </summary>
        /// <param name="type"></param>
        public void ChangeFacilityType(uint typeId, eZoneFacilityCategory category) {

            // 区域施設テーブルを変更
            if (m_zoneFacility != null)
            {
                SfZoneFacilityTableManager.Instance.Table.ChangeZoneFacilityType(m_zoneFacility.AreaId, m_zoneFacility.CellIndex, typeId, category);
                SfZoneFacilityTableManager.Instance.Table.SetZoneFacilityExpantion(m_zoneFacility.AreaId, m_zoneFacility.CellIndex, 1);
            }
            else
            {
                SfZoneFacilityTableManager.Instance.Table.BuildZoneFacilityType(m_areaId, m_cellIndex, typeId, category, 1);
            }

            // 変更した区域をこのデータに設定
            m_zoneFacility = SfZoneFacilityTableManager.Instance.Table.Get(m_areaId, m_cellIndex);

            // セルのボタン有効化の変更と施設画像を変更
            Cell.SettingZoneButtonEnable();
        }
    }
}