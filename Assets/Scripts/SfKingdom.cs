using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{


    /// <summary>
    /// 王国
    /// 保存情報
    /// ゲーム開始時に一度だけ作成
    /// </summary>
    [Serializable]
    public class SfKingdomRecord
    {
        // 王国 ID
        public uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // 王国名
        public string m_name = "";
        public string Name { get => m_name; set => m_name = value; }

        // 王国の色
        public Color m_color = Color.white;
        public Color Color { get => m_color; set => m_color = value; }

        // 領域 ID リスト
        public List<uint> m_sfDominionIdList = new List<uint>();
        public List<uint> DominionIdList { get => m_sfDominionIdList; set => m_sfDominionIdList = value; }

        // 国民の数
        public uint m_population = 0;
        public uint Population { get => m_population; set => m_population = value; }

        // 資源とかは別枠？
    }

    /// <summary>
    /// 王国生成工場基底
    /// </summary>
    public abstract class SfKingdomFactoryBase
    {
        public SfKingdomRecord Create(uint uniqueId)
        {
            var record = CreateRecord();

            // ユニーク ID 設定
            record.Id = uniqueId;

            // 王国名設定
            record.Name = CreateName();

            // 王国カラー設定
            record.Color = SettingColor();

            return record;
        }

        // レコード生成
        protected abstract SfKingdomRecord CreateRecord();

        // 王国名生成
        protected abstract string CreateName();
        // 王国カラーの設定
        protected abstract Color SettingColor();
    }

    /// <summary>
    /// 王国生成工場
    /// </summary>
    public abstract class SfKingdomFactory : SfKingdomFactoryBase
    {
        protected override SfKingdomRecord CreateRecord()
        {
            return new SfKingdomRecord();
        }
    }

    /// <summary>
    /// 自国の生成
    /// 自国の生成はゲーム開始前に設定した項目を設定
    /// </summary>
    public class SfSelfKingdomFactory : SfKingdomFactory
    {
        // 王国名生成
        protected override string CreateName() {
            return ConfigController.Instance.KingdomName;
        }

        // 王国カラーの設定
        protected override Color SettingColor() {
            return ConfigController.Instance.KingdomColor;
        }
    }


    // その他の国のランダム生成
    // ある程度の国を事前に作成しておいて割り振るだけに
    // とどめるか、すべて０から作成するか・・・
    public class SfOtherKingdomFactory : SfKingdomFactory
    {
        // 王国名生成
        protected override string CreateName()
        {
            return "test";
        }

        // 王国カラーの設定
        protected override Color SettingColor()
        {
            return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 0.6f);
        }
    }

    /// <summary>
    /// 王国工場管理
    /// </summary>
    public class SfKingdomFactoryManager : Singleton<SfKingdomFactoryManager>
    {
    }

    /// <summary>
    /// 王国レコード管理
    /// プレイ中に生成されているすべての SfKingdomRecord
    /// </summary>
    public class SfKingdomRecordTable : RecordTable<SfKingdomRecord>
    {
        // ユニーク ID リスト
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        // 登録
        public void Regist(SfKingdomRecord record) => RecordList.Add(record);

        // 領域レコードの取得
        public override SfKingdomRecord Get(uint id) => RecordList.Find(r => r.Id == id);
    }

    public class SfKingdomRecordTableManager : SfKingdomRecordTable
    {
        private static SfKingdomRecordTableManager s_instance = null;

        public static SfKingdomRecordTableManager Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;

                s_instance = new SfKingdomRecordTableManager();
                s_instance.Load();
                return s_instance;
            }
        }

        /// <summary>
        /// 読み込み処理
        /// </summary>
        public void Load()
        {
            RecordTableESDirector<SfKingdomRecord> director = new RecordTableESDirector<SfKingdomRecord>(new ESLoadBuilder<SfKingdomRecord, SfKingdomRecordTable>("SfKingdomRecordTable"));
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
            var director = new RecordTableESDirector<SfKingdomRecord>(new ESSaveBuilder<SfKingdomRecord>("SfKingdomRecordTable", this));
            director.Construct();
        }
    }
}