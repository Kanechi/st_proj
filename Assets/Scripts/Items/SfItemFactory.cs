using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// アイテム初回生成ビルダー (基底)
    /// </summary>
    public abstract class SfItemFactoryBuilderBase
    {
        // 作成されたアイテム
        protected SfItem m_createdItem = null;
        public SfItem GetCreatedItem() => m_createdItem;

        public abstract void CreateItem(uint itemId);

        /// <summary>
        /// アイテムのレアリティをランダムに設定
        /// </summary>
        public void RandomSettingItemRarity()
        {
            // コモン、アンコモンならそのままの名前が使われ
            // レア、エピックなら領域名が使われ
            // レジェンダリーなら地域名が使われる
            int[] m_rarityList = new int[] {
                SfConfigController.Instance.ItemRarityRateCommon,
                SfConfigController.Instance.ItemRarityRateUncommmon,
                SfConfigController.Instance.ItemRarityRateRare,
                SfConfigController.Instance.ItemRarityRateEpic,
                SfConfigController.Instance.ItemRarityRateLegendary
            };

            int rarity = SfConstant.WeightedPick(m_rarityList);

            m_createdItem.Rarity = (eRarity)rarity;
        }

        // 基本アイテムの設定
        public abstract void SettingBaseItemID();

        // 基本アイテムの基本名を取得
        public abstract string GetBaseName();

        // アイテムの名称の設定
        // アイテムの名称の設定
        public void SettingItemName(SfZoneFacility zoneFacility)
        {
            string uniqueName = "";

            if (m_createdItem.Rarity == eRarity.Common || m_createdItem.Rarity == eRarity.Uncommon)
            {
                // 世界の名前から
                uniqueName = SfConfigController.Instance.WorldName;
            }
            else if (m_createdItem.Rarity == eRarity.Rare || m_createdItem.Rarity == eRarity.Epic)
            {
                // 領域から
                var area = SfAreaTableManager.Instance.Table.Get(zoneFacility.AreaId);
                var dominion = SfDominionTableManager.Instance.Table.Get(area.DominionId);

                uniqueName = dominion.Name;
            }
            else if (m_createdItem.Rarity == eRarity.Legendary)
            {
                // 地域から
                var area = SfAreaTableManager.Instance.Table.Get(zoneFacility.AreaId);

                uniqueName = area.Name;
            }

            m_createdItem.Name = uniqueName + GetBaseName();
        }
    }

    /// <summary>
    /// 生産資源アイテム初回作成用ビルダー (基底)
    /// </summary>
    public class SfProductionResourceItemFactoryBuilder : SfItemFactoryBuilderBase
    {
        // アイテムの基準となる生産資源レコード
        private SfProductionResourceRecord m_record = null;

        // constructor
        public SfProductionResourceItemFactoryBuilder(SfProductionResourceRecord record) => m_record = record;

        public override void CreateItem(uint itemId) {
            m_createdItem = new SfItem();
            m_createdItem.Id = itemId;
            m_createdItem.BaseItemCategory = eFacilityItemGenCategory.Production;
        }

        // 基本アイテムの設定
        public override void SettingBaseItemID() => m_createdItem.BaseItemId = m_record.Id;

        // 基本アイテムの基本名をランダムに取得
        public override string GetBaseName()
        {
            int index = UnityEngine.Random.Range(0, m_record.BaseNameList.Count);
            return m_record.BaseNameList[index];
        }
    }

    /// <summary>
    /// 加工品アイテム初回作成用ビルダー (基底)
    /// </summary>
    public class SfProcessGoodsItemFactoryBuilder : SfItemFactoryBuilderBase
    {
        // アイテムの基準となる加工品レコード
        private SfProcessedGoodsRecord m_record = null;

        // constructor
        public SfProcessGoodsItemFactoryBuilder(SfProcessedGoodsRecord record) => m_record = record;

        public override void CreateItem(uint itemId)
        {
            m_createdItem = new SfItem();
            m_createdItem.Id = itemId;
            m_createdItem.BaseItemCategory = eFacilityItemGenCategory.Processed;
        }

        // 基本アイテムの設定
        public override void SettingBaseItemID() => m_createdItem.BaseItemId = m_record.Id;

        // 基本アイテムの基本名を取得
        public override string GetBaseName() => m_record.BaseName;
    }

    /// <summary>
    /// アイテム初回作成用ディレクタ
    /// アイテムが初回作成されるときにのみ呼び出す
    /// </summary>
    public class SfItemFactoryDirector
    {
        private SfItemFactoryBuilderBase m_builder = null;

        public SfItemFactoryDirector(SfItemFactoryBuilderBase builder) => m_builder = builder;

        public SfItem GetCreatedItem() => m_builder.GetCreatedItem();

        /// <summary>
        /// 初回作成のみ
        /// </summary>
        /// <param name="zoneFacility">アイテムの初回作成が行われる施設データ</param>
        public void Construct(SfZoneFacility zoneFacility)
        {
            m_builder.CreateItem(SfConstant.CreateUniqueId(ref SfItemTableManager.Instance.m_uniqueIdList));

            // 基準となるアイテムのアイテム ID を設定
            m_builder.SettingBaseItemID();

            // アイテムのレアリティをランダムに設定する
            m_builder.RandomSettingItemRarity();

            // 名称はレアリティで決定されるのでレアリティの後に名称を設定する

            // アイテムの名称を設定する
            m_builder.SettingItemName(zoneFacility);
        }
    }
}