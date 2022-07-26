using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sfproj
{
    public class SfItemData : IJsonParser
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
    public class SfItemDataTable : RecordTable<SfItemData>
    {
        // ユニーク ID リスト
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        // 登録
        public void Regist(SfItemData record) => RecordList.Add(record);

        // アイテムレコードの取得
        public override SfItemData Get(uint id) => RecordList.Find(r => r.Id == id);
    }

    /// <summary>
    /// 地域レコードテーブル管理
    /// </summary>
    public class SfItemDataTableManager : SfItemDataTable
    {
        private static SfItemDataTableManager s_instance = null;

        public static SfItemDataTableManager Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;

                s_instance = new SfItemDataTableManager();

                s_instance.Load();

                return s_instance;
            }
        }

        /// <summary>
        /// 読み込み処理
        /// </summary>
        public void Load()
        {

            var builder = new ESLoadBuilder<SfItemData, SfItemDataTable>("SfItemDataTable");

            var director = new RecordTableESDirector<SfItemData>(builder);

            director.Construct();

            if (director.GetResult() != null && director.GetResult().RecordList.Count > 0)
            {
                m_recordList.AddRange(director.GetResult().RecordList);
            }
        }

        /// <summary>
        /// 保存処理
        /// </summary>
        public void Save()
        {

            var builder = new ESSaveBuilder<SfItemData>("SfItemDataTable", this);

            var director = new RecordTableESDirector<SfItemData>(builder);

            director.Construct();
        }
    }
}