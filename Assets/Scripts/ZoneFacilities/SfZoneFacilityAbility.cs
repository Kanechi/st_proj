using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{
    /// <summary>
    /// ���{�݂̔\��
    /// </summary>
    public abstract class SfZoneFacilityAbility
    {
        // �{�ݑ��� ID �ƈ�v
        public abstract uint FacilityTypeId { get; }
        // �{�ݑ��̃J�e�S���ƈ�v
        public abstract eZoneFacilityCategory FacilityCategory { get; }

        /// <summary>
        /// ���t��������
        /// </summary>
        /// <param name="facility"></param>
        public virtual void OnSetup(SfZoneFacility facility) { }

        /// <summary>
        /// �X�V����
        /// </summary>
        /// <param name="facility">���̔\�͂��g���Ă���{��</param>
        public virtual void OnUpdate(float deltaTime, SfZoneFacility facility) { }

        /// <summary>
        /// ���O��������
        /// </summary>
        public virtual void OnRemove(SfZoneFacility facility) { }
    }

    /// <summary>
    /// �p�b�V�u�{�ݔ\��
    /// 
    /// �ǂ���
    /// �Ȃɂ�
    /// ������
    /// 
    /// ��Ƃ���
    ///     �u�n��v�́u�ő�l���v���u�P�O�����v
    ///     �u�n��v�́u�h��́v���u�T�O�����v
    /// </summary>
    public abstract class SfZoneFacilityPassiveAbility : SfZoneFacilityAbility
    {
        
    }

    /// <summary>
    /// �A�N�e�B�u�{�ݔ\��
    /// 
    /// �ǂ���
    /// �Ȃɂ�
    /// ������
    /// 
    /// ��Ƃ���
    ///     �u�n��v�́u�����̐��Y�����v���u�P�b�Ԋu�v�Łu�T�����v
    /// </summary>
    public abstract class SfZoneFacilityActiveAbility : SfZoneFacilityAbility
    {

    }
    
    // ���Y�����{�݃A�N�e�B�u�\��
    public abstract class SfZoneProductionResourceFacilityActiveAbility : SfZoneFacilityActiveAbility
    {
        public override eZoneFacilityCategory FacilityCategory => eZoneFacilityCategory.ProductionResource;
    }

    // ���H�i�{�݃A�N�e�B�u�\��
    public abstract class SfZoneProcessedGoodsFacilityActiveAbility : SfZoneFacilityActiveAbility
    {
        public override eZoneFacilityCategory FacilityCategory => eZoneFacilityCategory.ProcessedGoods;
    }

    /// <summary>
    /// �{�݃A�N�e�B�u�\�̓e�[�u��
    /// </summary>
    public class SfZoneFacilityActiveAbilityTable : RecordTable<SfZoneFacilityAbility>
    {
        // ���g�p
        public override SfZoneFacilityAbility Get(uint id) => null;

        // ID �� �J�e�S������擾
        public SfZoneFacilityAbility Get(uint id, eZoneFacilityCategory category)
        {
            if (id == 0)
                return null;

            if (category == eZoneFacilityCategory.None)
                return null;

            SfZoneFacilityAbility record = null;
            if (RecordList.Count == 0)
            {
                record = RecordList.Find(r => r.FacilityTypeId == id && r.FacilityCategory == category);
            }
            else { 
                // TODO: �ЂƂ܂��f�o�b�O�ō������Y�\�͎{��
                record = new SfZoneFacilityGrainIncreaseActiveAbility();
            }

            return record;
        }
    }

    /// <summary>
    /// �{�݃A�N�e�B�u�\�̓e�[�u���Ǘ�
    /// </summary>
    public class SfZoneFacilityActiveAbilityTableManager : Singleton<SfZoneFacilityActiveAbilityTableManager>
    {
        private SfZoneFacilityActiveAbilityTable m_table = new SfZoneFacilityActiveAbilityTable();
        public SfZoneFacilityActiveAbilityTable Table => m_table;
    }
}