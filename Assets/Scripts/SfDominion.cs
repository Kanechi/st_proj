using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{


    /// <summary>
    /// 領域詳細レコード
    /// 領域の開拓システムは無い
    /// 保存情報
    /// ゲーム開始時に一度だけ生成
    /// 隣接していれば統治するかしないかを選択
    /// 資源を消費して統治が可能
    /// </summary>
    [Serializable]
    public class SfDominionRecord
    {
        // 領域 ID
        public uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // 領域名
        public string m_name = "";
        public string Name { get => m_name; set => m_name = value; }

        // テリトリインデックス
        public int m_territoryIndex = -1;
        public int TerritoryIndex { get => m_territoryIndex; set => m_territoryIndex = value; }

        // true...統治済み
        public bool m_ruleFlag = false;
        public bool RuleFlag { get => m_ruleFlag; set => m_ruleFlag = value; }


        // 統治している王国 ID (0...統治されていない)
        public uint m_kingdomId = 0;
        public uint GovernKingdomId { get => m_kingdomId; set => m_kingdomId = value; }

        // true...首都
        public bool m_capitalFlag = false;
        public bool CapitalFlag { get => m_capitalFlag; set => m_capitalFlag = value; }

        // true...海に隣接している
        public bool m_neighboursOceanFlag = false;
        public bool NeighboursOceanFlag { get => m_neighboursOceanFlag; set => m_neighboursOceanFlag = value; }

        // 地域 ID リスト
        public List<uint> m_sfAreaIdList = new List<uint>();
        public List<uint> AreaIdList { get => m_sfAreaIdList; set => m_sfAreaIdList = value; }
    }

#if false
    /// <summary>
    /// 領域
    /// スクロールビューの情報
    /// </summary>
    public class SfDominion : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
#endif

    /// <summary>
    /// 領域生成工場  基底
    /// 2022/06/08
    ///     現状種類が存在するわけではないので分ける必要はないかも
    ///     一応テンプレート化しておく
    /// </summary>
    public abstract class SfDominionFactoryBase
    {
        public SfDominionRecord Create(uint uniqueId, int territoryIndex)
        {
            var record = CreateRecord();

            // ユニーク ID の設定
            record.Id = uniqueId;

            // 領域名の生成
            record.Name = CreateName();

            // テリトリインデックスの設定
            record.TerritoryIndex = territoryIndex;

            // 海に隣接しているかどうかのフラグを設定
            record.m_neighboursOceanFlag = CheckAdjastingOceanTerrain(territoryIndex);

            return record;
        }

        // 領域レコードの作成
        protected abstract SfDominionRecord CreateRecord();

        // 領域名の作成
        protected abstract string CreateName();

        /// <summary>
        /// 海に隣接しているかチェック
        /// 隣接しているテリトリに１つでも非表示があれば海
        /// </summary>
        /// <returns>true...海に隣接している</returns>
        private bool CheckAdjastingOceanTerrain(int territoryIndex)
        {
            var territory = TGS.TerrainGridSystem.instance.territories[territoryIndex];

            var neighbours = territory.neighbours;

            foreach (var t in neighbours)
            {
                if (t.visible == false)
                    return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 領域レコード生成工場
    /// </summary>
    public abstract class SfDominionCreateFactory : SfDominionFactoryBase
    {
        protected override SfDominionRecord CreateRecord()
        {
            return new SfDominionRecord();
        }
    }

    /// <summary>
    /// 領域生成工場
    /// </summary>
    public class SfDominionFactory : SfDominionCreateFactory
    {
        // この部分を地域によって変化させる？
        protected override string CreateName()
        {
            return "test";
        }


    }

    /// <summary>
    /// 領域工場管理
    /// </summary>
    public class SfDominionFactoryManager : Singleton<SfDominionFactoryManager>{

        /// <summary>
        /// 領域レコードの作成
        /// </summary>
        /// <param name="territoryIndex"></param>
        /// <returns></returns>
        public SfDominionRecord Create(int territoryIndex) {

            // ひとまず SfDominionFactory しかない
            var factory = new SfDominionFactory();

            // ユニーク ID の作成
            uint uniqueId = SfConstant.CreateUniqueId(ref SfDominionRecordTableManager.Instance.m_uniqueIdList);

            // 領域レコードを作成
            var record = factory.Create(uniqueId, territoryIndex);

            return record;
        }
    }



    /// 領域 管理
    /// プレイ中に生成されているすべての SfDominionRecord
    /// 保存時は別ファイルの別クラスに実装
    /// </summary>
    public class SfDominionRecordTable : RecordTable<SfDominionRecord>
    {
        // ユニーク ID リスト
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        // 登録
        public void Regist(SfDominionRecord record) => RecordList.Add(record);

        // 領域レコードの取得
        public override SfDominionRecord Get(uint id) => RecordList.Find(r => r.Id == id);

        public SfDominionRecord GetAtTerritoryIndex(int territoryIndex) => RecordList.Find(r => r.TerritoryIndex == territoryIndex);
    }

    /// <summary>
    /// 領域管理
    /// </summary>
    public class SfDominionRecordTableManager : SfDominionRecordTable
    {
        private static SfDominionRecordTableManager s_instance = null;

        public static SfDominionRecordTableManager Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;

                s_instance = new SfDominionRecordTableManager();
                s_instance.Load();
                return s_instance;
            }
        }

        /// <summary>
        /// 読み込み処理
        /// </summary>
        public void Load()
        {
            RecordTableESDirector<SfDominionRecord> director = new RecordTableESDirector<SfDominionRecord>(new ESLoadBuilder<SfDominionRecord, SfDominionRecordTable>("SfDominionRecordTable"));
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
            var director = new RecordTableESDirector<SfDominionRecord>(new ESSaveBuilder<SfDominionRecord>("SfDominionRecordTable", this));
            director.Construct();
        }
    }
}