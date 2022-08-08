using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{
    /// <summary>
    /// 生産資源の増加
    /// ひとまず穀物
    /// </summary>
    public class SfZoneFacilityGrainIncreaseActiveAbility : SfZoneProductionResourceFacilityActiveAbility
    {
        public override uint FacilityTypeId => 10000;

        // なにを、穀物なら生産を行う
        private eProductionResouceCategory m_productCategory => eProductionResouceCategory.Grain;

        // いくつ
        private float m_interval = 1.0f;    // 時間
        private float m_counter = 0.0f;     // カウンター
        private int m_value = 5;           // 増加量

        public override void OnUpdate(float deltaTime, SfZoneFacility facility)
        {
            // 時間が来たら
            m_counter += deltaTime;
            if (m_interval <= m_counter)
            {
                m_counter = 0.0f;

                var area = SfAreaTableManager.Instance.Table.Get(facility.AreaId);

                // 地域に設定されている生産資源の中の
                foreach (var itemId in area.ProductionResourceItemIdList)
                {
                    // 穀物資源だけを
                    var itemRecord = SfProductionResourceItemTableManager.Instance.Table.Get(itemId).GetBaseItem();
                    if (itemRecord.Category == m_productCategory)
                    {
                        // 地域の倉庫に増加
                    }
                }
            }
        }
    }
}