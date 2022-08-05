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
    /// 生産資源アイテム
    /// </summary>
    public class SfProductionResourceItem : SfItem
    {
        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);
        }

        public SfProductionResourceItem Clone() => (SfProductionResourceItem)MemberwiseClone();
    }

    /// <summary>
    /// 加工品アイテム
    /// </summary>
    public class SfProcessedGoodsItem : SfItem
    {
        // 種類

        // 効果 (効果は別途関数化するかもしれない。その場合は値はその関数内で利用されることになる)

        // 値

        public override void Parse(IDictionary<string, object> data)
        {
            base.Parse(data);
        }

        public SfProcessedGoodsItem Clone() => (SfProcessedGoodsItem)MemberwiseClone();
    }

    /// <summary>
    /// 生産資源アイテムテーブル
    /// コモンからエピックまで
    /// レジェンダリは無い
    /// </summary>
    public class SfProductionResourceItemTable : RecordTable<SfProductionResourceItem>
    {
        // 登録
        public void Regist(SfProductionResourceItem record) => RecordList.Add(record);

        // アイテムレコードの取得
        public override SfProductionResourceItem Get(uint id) => RecordList.Find(r => r.Id == id);
        public SfProductionResourceItem Get(uint baseItemid, eRarity rarity) => RecordList.Find(r => r.Rarity == rarity && r.BaseItemId == baseItemid);

        // 同じレアリティで同じ基本アイテム ID の同じアイテムがすでに存在しているかどうかチェック
        public bool CheckExist(uint baseItemid, eRarity rarity) => Get(baseItemid, rarity) != null;
    }

    /// <summary>
    /// 生産資源アイテムテーブル管理
    /// </summary>
    public class SfProductionResourceItemTableManager : Singleton<SfProductionResourceItemTableManager>
    {
        private SfProductionResourceItemTable m_table = new SfProductionResourceItemTable();
        public SfProductionResourceItemTable Table => m_table;

        // ユニーク ID リスト
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();


        /// <summary>
        /// 読み込み処理
        /// </summary>
        public void Load()
        {
            var director = new RecordTableESDirector<SfProductionResourceItem>(new ESLoadBuilder<SfProductionResourceItem, SfProductionResourceItemTable>("SfProductionResourceItemTable"));

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
            var director = new RecordTableESDirector<SfProductionResourceItem>(new ESSaveBuilder<SfProductionResourceItem>("SfProductionResourceItemTable", m_table));

            director.Construct();
        }
    }
}