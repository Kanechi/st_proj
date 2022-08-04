using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// ワールドに存在するアイテムの基本アイテム IDをチェックし生成するアイテムの基本アイテム ID を決定する
    /// </summary>
    public class SfProductionResourceListChecker
    {
        private void CheckAndAddList(eProductionResourceCategoryFlag checkFlag, eProductionResouceCategory addCategory, ref eProductionResourceCategoryFlag flag, ref List<SfProductionResourceRecord> baseItemList)
        {
            if ((checkFlag & flag) != 0)
            {
                baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(addCategory));
                flag |= eProductionResourceCategoryFlag.Grain;
            }
        }

        public List<SfProductionResourceRecord> GetRecordByTerrain(eExistingTerrain terrain)
        {
            var baseItemList = new List<SfProductionResourceRecord>();

            eProductionResourceCategoryFlag flag = 0;

            if ((eExistingTerrain.Plane & terrain) != 0)
            {
                CheckAndAddList(eProductionResourceCategoryFlag.Grain, eProductionResouceCategory.Grain, ref flag, ref baseItemList);
            }

            if ((eExistingTerrain.Mountain & terrain) != 0)
            {
                if ((eProductionResourceCategoryFlag.Monster & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Monster));
                    flag |= eProductionResourceCategoryFlag.Monster;
                }

                if ((eProductionResourceCategoryFlag.Plant & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Plant));
                    flag |= eProductionResourceCategoryFlag.Plant;
                }

                if ((eProductionResourceCategoryFlag.Wood & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Wood));
                    flag |= eProductionResourceCategoryFlag.Wood;
                }

            }

            if ((eExistingTerrain.Forest & terrain) != 0)
            {
                if ((eProductionResourceCategoryFlag.Grain & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Grain));
                    flag |= eProductionResourceCategoryFlag.Grain;
                }
                if ((eProductionResourceCategoryFlag.Mineral & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Mineral));
                    flag |= eProductionResourceCategoryFlag.Mineral;
                }
                if ((eProductionResourceCategoryFlag.Monster & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Monster));
                    flag |= eProductionResourceCategoryFlag.Monster;
                }
            }

            if ((eExistingTerrain.River & terrain) != 0)
            {
                if ((eProductionResourceCategoryFlag.Monster & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Monster));
                    flag |= eProductionResourceCategoryFlag.Monster;
                }

                if ((eProductionResourceCategoryFlag.Plant & flag) != 0)
                {
                    baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Plant));
                    flag |= eProductionResourceCategoryFlag.Plant;
                }
            }

            if ((eExistingTerrain.Ocean & terrain) != 0)
            {
                baseItemList.AddRange(SfProductionResourceTableManager.Instance.GetProductionResourceListByCategory(eProductionResouceCategory.Monster));
            }

            return baseItemList;
        }


#if false
        /// <summary>
        /// 既に生成されている基本アイテム ID をチェックし生成されていないものから順に生成を行う
        /// 生成されていないものから順に生成すると地形に対してバラバラに配置されてしまいあ。
        /// 同じものが生成されなくなってしまう・・・
        /// やはり同じものをいくつか生成した方がよいかも・・・
        /// </summary>
        /// <returns></returns>
        public uint CheckAndGenRandomBaseId() {

            // 全ての基本アイテムを取得
            var baseItemList = SfProductionResourceTableManager.Instance.GetAllProductionResourceList();

            // 現状生成されている基本アイテム ID を取得
            var genItemList = SfProductionResourceItemTableManager.Instance.Table.RecordList;

            // 現状生成されている基本アイテム ID を調べ生成されていない基本アイテムアイテム ID をリスト化
            var noGenBaseItemList = new List<SfProductionResourceRecord>();
            for (int i = 0; i < baseItemList.Count; ++i)
            {
                // false...使われていない
                bool isUse = false;

                for (int j = 0; j < genItemList.Count; ++j)
                {
                    // 生成されたアイテムに使用されている基本アイテム ID がレコードの基本アイテム ID 
                    if (baseItemList[i].Id == genItemList[j].BaseItemId)
                    {
                        isUse = true;
                    }
                }

                if (isUse == false)
                {
                    // 使用されていなければ取り付ける
                    noGenBaseItemList.Add(baseItemList[i]);
                }
            }

            // ランダムに基本アイテム ID を算出
            int index = UnityEngine.Random.Range(0, noGenBaseItemList.Count);
            return noGenBaseItemList[index].Id;
        }
    
#endif
    }

    /// <summary>
    /// 最終的に生成したアイテムを領域か地域どちらに取り付けるかは外部でレアリティを調べて取り付ける
    /// </summary>
    public abstract class SfItemFactoryBase
    {
        public SfItem Create(uint id, uint baseId, eRarity rarity)
        {
            var item = CreateItem();

            item.Id = id;

            item.BaseItemId = baseId;

            item.Rarity = rarity;

            item.Name = CreateItemName(baseId, rarity);

            return item;
        }

        public abstract SfItem CreateItem();

        // アイテムの名前を生成
        public abstract string CreateItemName(uint baseId, eRarity rarity);
    }

    /// <summary>
    /// 生産資源アイテムの生成
    /// 生産資源アイテムと加工品アイテムでクラスを分けるか考え中
    /// </summary>
    public abstract class SfProductionResourceItemCreateFactory : SfItemFactoryBase
    {
        public override SfItem CreateItem()
        {
            var item = new SfItem();

            return item;
        }
    }

    /// <summary>
    /// 生産資源アイテム工場
    /// </summary>
    public class SfProductionResourceItemFactory : SfProductionResourceItemCreateFactory {

        // アイテムの名前を生成
        public override string CreateItemName(uint baseId, eRarity rarity)
        {
            // 基本アイテム名リストを取得
            var record = SfProductionResourceTableManager.Instance.Get(baseId);

            // ランダムに１つ選択
            int index = UnityEngine.Random.Range(0, record.BaseNameList.Count);
            string baseName = record.BaseNameList[index];

            // レアリティがレアかエピックならランダム名を取り付け
            if (rarity == eRarity.Rare || rarity == eRarity.Epic)
            {
                baseName = "希少な" + baseName; 
            }

            return baseName;
        }
    }

    /// <summary>
    /// アイテム生成用ビルダー
    /// </summary>
    public abstract class SfItemBuilderBase {

        // 生成されたアイテム、もしくはすでに生成されているアイテム
        protected SfItem m_createdItem = null;
        public SfItem GetResult() => m_createdItem;

        // 基本アイテム ID を地形ごとにランダムに生成
        public abstract uint GenBaseItemId();

        // アイテムのレア度を決定
        public abstract eRarity GenItemRarity();

        // true...生成した基本アイテム ID と レア度が設定されているアイテムがすでに存在している
        public abstract bool CheckExist(uint baseItemId, eRarity rarity);

        // 生成されるアイテムのユニーク ID を生成
        protected abstract uint GenUniqueItemId();

        // アイテムの生成
        public abstract void CreateItem(uint baseItemId, eRarity rarity);

        // アイテムの登録
        public abstract void RegistItem();
    }

    /// <summary>
    /// アイテム生成用ビルダー
    /// </summary>
    public class SfProductionResourceItemBuilder : SfItemBuilderBase
    {
        // 原産地域
        private SfArea m_placeOriginArea = null;

        // constructor
        public SfProductionResourceItemBuilder(SfArea placeOriginArea) => m_placeOriginArea = placeOriginArea;

        // 基本アイテム ID を地形ごとにランダムに生成
        public override uint GenBaseItemId()
        {
            var checker = new SfProductionResourceListChecker();

            var baseItemList = checker.GetRecordByTerrain(m_placeOriginArea.ExistingTerrain);

            int index = UnityEngine.Random.Range(0, baseItemList.Count);

            return baseItemList[index].Id;
        }

        // アイテムのレア度を決定
        public override eRarity GenItemRarity() {

            // コモン、アンコモンならそのままの名前が使われ
            // レア、エピックなら領域名が使われ
            // レジェンダリーなら地域名が使われる
            int[] m_rarityList = new int[] {
                SfConfigController.Instance.ItemRarityRateCommon,
                SfConfigController.Instance.ItemRarityRateUncommmon,
                SfConfigController.Instance.ItemRarityRateRare,
                SfConfigController.Instance.ItemRarityRateEpic,
            };

            int rarity = SfConstant.WeightedPick(m_rarityList);

            return (eRarity)rarity;
        }

        // true...生成した基本アイテム ID と レア度が設定されているアイテムがすでに存在している
        public override bool CheckExist(uint baseItemId, eRarity rarity)
        {
            m_createdItem = SfProductionResourceItemTableManager.Instance.Table.Get(baseItemId, rarity);

            return m_createdItem != null;
        }

        // 生成されるアイテムのユニーク ID を生成
        protected override uint GenUniqueItemId() => SfConstant.CreateUniqueId(ref SfProductionResourceItemTableManager.Instance.m_uniqueIdList);

        // アイテムの生成
        public override void CreateItem(uint baseItemId, eRarity rarity) {

            var factory = new SfProductionResourceItemFactory();

            uint uniqueId = GenUniqueItemId();

            m_createdItem = factory.Create(uniqueId, baseItemId, rarity);
        }

        // アイテムの登録
        public override void RegistItem()
        {
            SfProductionResourceItemTableManager.Instance.Table.Regist(m_createdItem as SfProductionResourceItem);
        }
    }

    /// <summary>
    /// アイテム生成用ディレクター
    /// </summary>
    public class SfItemGenDirector
    {
        private SfItemBuilderBase m_builder = null;

        public SfItemGenDirector(SfItemBuilderBase builder) => m_builder = builder;

        public SfItem GetResult() => m_builder.GetResult();

        public void Construct() {

            uint baseItemId = m_builder.GenBaseItemId();

            eRarity rarity = m_builder.GenItemRarity();

            // 生成した基本アイテム ID と レア度のアイテムがすでに存在したらそれを使う
            if (m_builder.CheckExist(baseItemId, rarity) == false)
            {
                m_builder.CreateItem(baseItemId, rarity);

                // 新規で生成したらアイテムテーブルに登録
                m_builder.RegistItem();
            }
        }
    }
}