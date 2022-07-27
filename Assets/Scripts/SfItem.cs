using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sfproj
{
    public class SfItem : IJsonParser
    {
        // アイテム ID (ユニーク ID)
        public uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // 名称
        public string m_name = "";
        public string Name => m_name;

        // 基本となるアイテムカテゴリ(生産か加工か)
        public eFacilityItemGenCategory m_baseItemCategory = eFacilityItemGenCategory.None;
        public eFacilityItemGenCategory BaseItemCategory => m_baseItemCategory;

        // 基本となるアイテム ID
        public uint m_baseItemId = 0;
        public uint BaseItemId => m_baseItemId;

        public void Parse(IDictionary<string, object> data)
        {
            data.Get(nameof(m_id), out m_id);

            data.Get(nameof(m_name), out m_name);

            data.GetEnum(nameof(m_baseItemCategory), out m_baseItemCategory);

            data.Get(nameof(m_baseItemId), out m_baseItemId);
        }
    }

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
}