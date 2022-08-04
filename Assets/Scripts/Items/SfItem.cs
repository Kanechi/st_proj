using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sfproj
{
    public enum eRarity {
        None = -1,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
    }

    public class SfItem : IJsonParser
    {
        // アイテム ID (ユニーク ID)
        public uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // 名称
        public string m_name = "";
        public string Name { get => m_name; set => m_name = value; }

        // 基本となるアイテム ID
        public uint m_baseItemId = 0;
        public uint BaseItemId { get => m_baseItemId; set => m_baseItemId = value; }

        // レアリティ
        public eRarity m_rarity = eRarity.None;
        public eRarity Rarity { get => m_rarity; set => m_rarity = value; }

        public virtual void Parse(IDictionary<string, object> data)
        {
            data.Get(nameof(m_id), out m_id);

            data.Get(nameof(m_name), out m_name);

            data.Get(nameof(m_baseItemId), out m_baseItemId);

            data.GetEnum(nameof(m_rarity), out m_rarity);
        }
    }

    /// <summary>
    /// 生産資源アイテム (コモン、アンコモン)
    /// </summary>
    public class SfProductionResourceItem : SfItem 
    {
        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);
        }
    }

    /// <summary>
    /// 領域原産の生産資源アイテム (レア、エピック)
    /// </summary>
    public class SfDominionProductionResourceItem : SfProductionResourceItem
    {
        // 原産地 ID
        public uint m_placeOriginId = 0;
        public uint PlaceOriginId { get => m_placeOriginId; set => m_placeOriginId = value; }

        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);

            data.Get(nameof(m_placeOriginId), out m_placeOriginId);
        }
    }

    /// <summary>
    /// 地域原産の生産資源アイテム (レジェンダリー)
    /// </summary>
    public class SfAreaProductionResourceItem : SfProductionResourceItem
    {
        // 原産地 ID
        public uint m_placeOriginId = 0;
        public uint PlaceOriginId { get => m_placeOriginId; set => m_placeOriginId = value; }

        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);

            data.Get(nameof(m_placeOriginId), out m_placeOriginId);
        }
    }

    /// <summary>
    /// 加工品アイテム
    /// </summary>
    public class SfProcessedGoodsItem : SfItem
    {
        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);
        }
    }

    /// <summary>
    /// 領域原産の加工品アイテム
    /// </summary>
    public class SfDominionProcessedGoodsItem : SfProcessedGoodsItem
    {
        // 原産地 ID
        public uint m_placeOriginId = 0;
        public uint PlaceOriginId { get => m_placeOriginId; set => m_placeOriginId = value; }

        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);

            data.Get(nameof(m_placeOriginId), out m_placeOriginId);
        }
    }

    /// <summary>
    /// 地域原産の加工品アイテム
    /// </summary>
    public class SfAreaProcessedGoodsItem : SfProcessedGoodsItem
    {
        // 原産地 ID
        public uint m_placeOriginId = 0;
        public uint PlaceOriginId { get => m_placeOriginId; set => m_placeOriginId = value; }

        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);

            data.Get(nameof(m_placeOriginId), out m_placeOriginId);
        }
    }

    /// <summary>
    /// 生産資源アイテムテーブル
    /// </summary>
    public class SfProductionResourceItemTable : RecordTable<SfItem>
    {
        // 登録
        public void Regist(SfItem record) => RecordList.Add(record);

        // アイテムレコードの取得
        public override SfItem Get(uint id) => RecordList.Find(r => r.Id == id);

        /// <summary>
        /// コモンとアンコモンのアイテムがすでに存在する
        /// </summary>
        /// <param name="baseId"></param>
        /// <param name="rarity"></param>
        /// <returns>true...重複する</returns>
        public bool CheckExistCommonAndUncommon(uint baseId, eRarity rarity)
        {
            return RecordList.Find(r => r.Rarity == rarity && r.BaseItemId == baseId) != null;
        }

        /// <summary>
        /// レアとエピックのアイテムがすでに存在する
        /// 領域 ID の一致まで確認
        /// </summary>
        /// <param name="baseId"></param>
        /// <param name="rarity"></param>
        /// <param name="dominion"></param>
        /// <returns>true...重複する</returns>
        public bool CheckExistRareAndEpic(uint baseId, eRarity rarity, SfDominion dominion)
        {
            var record = RecordList.Find(r => r.Rarity == rarity && r.BaseItemId == baseId);
            if (record == null)
                return true;
            var dominionItemRecord = record as SfDominionProductionResourceItem;
            return dominionItemRecord.PlaceOriginId == dominion.Id;
        }

        /// <summary>
        /// レジェンダリのアイテムがすでに存在する
        /// 地域 ID の一致まで確認
        /// </summary>
        /// <param name="baseId"></param>
        /// <param name="rarity"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public bool CheckExistLegendary(uint baseId, eRarity rarity, SfArea area)
        {
            var record = RecordList.Find(r => r.Rarity == rarity && r.BaseItemId == baseId);
            if (record == null)
                return true;
            var areaItemRecord = record as SfAreaProductionResourceItem;
            return areaItemRecord.PlaceOriginId == area.Id;
        }
    }

#if false
    /// <summary>
    /// 地域 管理
    /// プレイ中に生成されているすべての SfAreaData
    /// 保存時は別ファイルの別クラスに実装
    /// </summary>
    public class SfItemTable : RecordTable<SfItem>
    {
        // 登録
        public void Regist(SfItem record) => RecordList.Add(record);

        // アイテムレコードの取得
        public override SfItem Get(uint id) => RecordList.Find(r => r.Id == id);

        /// <summary>
        /// 新規に地域特有の新しい生産アイテムを登録
        /// ・・・つくりづらい
        /// ・・・factory でやることかな
        /// </summary>
        /// <param name="area"></param>
        /// <param name="productionRecource"></param>
        public void CreateNewItem(SfArea area, SfProductionResourceRecord productionRecource)
        { 
            // カテゴリを生産アイテムに設定
            

            // 名称を決定

        }
    }

    /// <summary>
    /// 地域レコードテーブル管理
    /// </summary>
    public class SfItemTableManager : Singleton<SfItemTableManager>
    {
        private SfItemTable m_table = new SfItemTable();
        public SfItemTable Table => m_table;

        // ユニーク ID リスト
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        /// <summary>
        /// 読み込み処理
        /// </summary>
        public void Load()
        {
            var director = new RecordTableESDirector<SfItem>(new ESLoadBuilder<SfItem, SfItemTable>("SfItemDataTable"));

            director.Construct();

            if (director.GetResult() != null && director.GetResult().RecordList.Count > 0)
            {
                m_table.RecordList.AddRange(director.GetResult().RecordList);
            }
        }

        /// <summary>
        /// 保存処理
        /// </summary>
        public void Save()
        {
            var director = new RecordTableESDirector<SfItem>(new ESSaveBuilder<SfItem>("SfItemDataTable", m_table));

            director.Construct();
        }
    }
#endif
}