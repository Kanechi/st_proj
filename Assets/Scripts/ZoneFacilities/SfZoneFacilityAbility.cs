using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{
    /// <summary>
    /// 区域施設の能力
    /// </summary>
    public abstract class SfZoneFacilityAbility
    {
        // 施設側の ID と一致
        public abstract uint FacilityTypeId { get; }
        // 施設側のカテゴリと一致
        public abstract eZoneFacilityCategory FacilityCategory { get; }

        /// <summary>
        /// 取り付け時処理
        /// </summary>
        /// <param name="facility"></param>
        public virtual void OnSetup(SfZoneFacility facility) { }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="facility">この能力を使っている施設</param>
        public virtual void OnUpdate(float deltaTime, SfZoneFacility facility) { }

        /// <summary>
        /// 取り外し時処理
        /// </summary>
        public virtual void OnRemove(SfZoneFacility facility) { }
    }

    /// <summary>
    /// パッシブ施設能力
    /// 
    /// どこに
    /// なにを
    /// いくつ
    /// 
    /// 例として
    ///     「地域」の「最大人口」を「１０増加」
    ///     「地域」の「防御力」を「５０増加」
    /// </summary>
    public abstract class SfZoneFacilityPassiveAbility : SfZoneFacilityAbility
    {
        
    }

    /// <summary>
    /// アクティブ施設能力
    /// 
    /// どこに
    /// なにを
    /// いくつ
    /// 
    /// 例として
    ///     「地域」の「穀物の生産資源」を「１秒間隔」で「５増加」
    /// </summary>
    public abstract class SfZoneFacilityActiveAbility : SfZoneFacilityAbility
    {

    }
    
    // 生産資源施設アクティブ能力
    public abstract class SfZoneProductionResourceFacilityActiveAbility : SfZoneFacilityActiveAbility
    {
        public override eZoneFacilityCategory FacilityCategory => eZoneFacilityCategory.ProductionResource;
    }

    // 加工品施設アクティブ能力
    public abstract class SfZoneProcessedGoodsFacilityActiveAbility : SfZoneFacilityActiveAbility
    {
        public override eZoneFacilityCategory FacilityCategory => eZoneFacilityCategory.ProcessedGoods;
    }

    /// <summary>
    /// 施設アクティブ能力テーブル
    /// </summary>
    public class SfZoneFacilityActiveAbilityTable : RecordTable<SfZoneFacilityAbility>
    {
        // 未使用
        public override SfZoneFacilityAbility Get(uint id) => null;

        // ID と カテゴリから取得
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
                // TODO: ひとまずデバッグで穀物生産能力施設
                record = new SfZoneFacilityGrainIncreaseActiveAbility();
            }

            return record;
        }
    }

    /// <summary>
    /// 施設アクティブ能力テーブル管理
    /// </summary>
    public class SfZoneFacilityActiveAbilityTableManager : Singleton<SfZoneFacilityActiveAbilityTableManager>
    {
        private SfZoneFacilityActiveAbilityTable m_table = new SfZoneFacilityActiveAbilityTable();
        public SfZoneFacilityActiveAbilityTable Table => m_table;
    }
}