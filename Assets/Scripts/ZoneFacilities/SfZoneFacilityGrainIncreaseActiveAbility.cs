using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{
    /// <summary>
    /// ���Y�����̑���
    /// �ЂƂ܂�����
    /// </summary>
    public class SfZoneFacilityGrainIncreaseActiveAbility : SfZoneProductionResourceFacilityActiveAbility
    {
        public override uint FacilityTypeId => 10000;

        // �Ȃɂ��A�����Ȃ琶�Y���s��
        private eProductionResouceCategory m_productCategory => eProductionResouceCategory.Grain;

        // ������
        private float m_interval = 1.0f;    // ����
        private float m_counter = 0.0f;     // �J�E���^�[
        private int m_value = 5;           // ������

        public override void OnUpdate(float deltaTime, SfZoneFacility facility)
        {
            // ���Ԃ�������
            m_counter += deltaTime;
            if (m_interval <= m_counter)
            {
                m_counter = 0.0f;

                var area = SfAreaTableManager.Instance.Table.Get(facility.AreaId);

                // �n��ɐݒ肳��Ă��鐶�Y�����̒���
                foreach (var itemId in area.ProductionResourceItemIdList)
                {
                    // ��������������
                    var itemRecord = SfProductionResourceItemTableManager.Instance.Table.Get(itemId).GetBaseItem();
                    if (itemRecord.Category == m_productCategory)
                    {
                        // �n��̑q�ɂɑ���
                    }
                }
            }
        }
    }
}