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
        // 王国 ID (もしネットワークを作成する場合はサーバーIDに準拠)
        public int m_id = 0;
        public int Id { get => m_id; set => m_id = value; }

        // 王国名
        public string m_name = "";
        public string Name { get => m_name; set => m_name = value; }

        // 王国の色
        public Color m_color = Color.white;
        public Color Color { get => m_color; set => m_color = value; }

        // true...自分の国
        public bool m_selfFlag = false;
        public bool SelfFlag { get => m_selfFlag; set => m_selfFlag = value; }

        // 領域 ID リスト
        public List<uint> m_sfDominionIdList = new List<uint>();
        public List<uint> DominionIdList { get => m_sfDominionIdList; set => m_sfDominionIdList = value; }

        // 国民の数
        public uint m_population = 0;
        public uint Population { get => m_population; set => m_population = value; }
    }

    /// <summary>
    /// 王国生成工場基底
    /// </summary>
    public abstract class SfKingdomFactoryBase
    {
        public SfKingdomRecord Create(int uniqueId)
        {
            var record = CreateRecord();

            // ユニーク ID 設定
            record.Id = uniqueId;

            // 王国名設定
            CreateName(record);

            // 王国カラー設定
            SettingColor(record);

            // 自分の国かどうかのフラグの設定
            SettingSelfFlag(record);

            return record;
        }

        // レコード生成
        protected abstract SfKingdomRecord CreateRecord();

        // 王国名生成
        protected abstract void CreateName(SfKingdomRecord record);
        // 王国カラーの設定
        protected abstract void SettingColor(SfKingdomRecord record);

        protected abstract void SettingSelfFlag(SfKingdomRecord record);
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
        protected override void CreateName(SfKingdomRecord record) {
            record.Name = ConfigController.Instance.KingdomName;
        }

        // 王国カラーの設定
        protected override void SettingColor(SfKingdomRecord record) {
            record.Color = ConfigController.Instance.KingdomColor;
        }

        // 自分の国かどうかのフラグ
        protected override void SettingSelfFlag(SfKingdomRecord record)
        {
            record.SelfFlag = true;
        }
    }


    // その他の国のランダム生成
    // ある程度の国を事前に作成しておいて割り振るだけに
    // とどめるか、すべて０から作成するか・・・
    public class SfOtherKingdomFactory : SfKingdomFactory
    {
        // 王国名生成
        protected override void CreateName(SfKingdomRecord record)
        {
            record.Name = "test";
        }

        // 王国カラーの設定
        protected override void SettingColor(SfKingdomRecord record)
        {
            record.Color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 0.6f);
        }

        // 自分の国かどうかのフラグ
        protected override void SettingSelfFlag(SfKingdomRecord record)
        {
            record.SelfFlag = false;
        }
    }

    /// <summary>
    /// 王国工場管理
    /// </summary>
    public class SfKingdomFactoryManager : Singleton<SfKingdomFactoryManager>
    {
        public SfKingdomRecord Create(bool selfKingdom)
        {
            SfKingdomFactoryBase factory = null;

            if (selfKingdom)
            {
                factory = new SfSelfKingdomFactory();
            }
            else
            {
                factory = new SfOtherKingdomFactory();
            }

            // ユニーク ID
            //uint uniqueId = SfConstant.CreateUniqueId(ref SfKingdomRecordTableManager.Instance.m_uniqueIdList);

            // 領域レコード
            var record = factory.Create(SfKingdomRecordTableManager.Instance.m_uniqueId);

            SfKingdomRecordTableManager.Instance.m_uniqueId++;

            return record;
        }
    }

    /// <summary>
    /// 王国レコード管理
    /// プレイ中に生成されているすべての SfKingdomRecord
    /// </summary>
    public class SfKingdomRecordTable : RecordTable<SfKingdomRecord>
    {
        // ユニーク ID リスト
        public int m_uniqueId = 0;

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