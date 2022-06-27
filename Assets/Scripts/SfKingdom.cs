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

        // 
    }

    public abstract class SfKingdomFactoryBase
    {
        public SfKingdomRecord Create(uint uniqueId)
        {
            var record = CreateRecord();

            record.Id = uniqueId;

            record.Name = CreateName();

            return record;
        }

        protected abstract SfKingdomRecord CreateRecord();

        protected abstract string CreateName();
    }

    public abstract class SfKingdomFactory : SfKingdomFactoryBase
    {
        protected override SfKingdomRecord CreateRecord()
        {
            return new SfKingdomRecord();
        }
    }

    // 自国の生成


    // その他の国のランダム生成
    // ある程度の国を事前に作成しておいて割り振るだけに
    // とどめるか、すべて０から作成するか・・・

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