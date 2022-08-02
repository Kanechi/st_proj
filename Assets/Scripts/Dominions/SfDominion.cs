using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using TGS;

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
    public class SfDominion
    {
        // 領域 ID
        public uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // 国からみて何番目に手に入れた領域なのかの番号
        // 奪われたり取り返したりすると番号は変わる
        public int m_dominionIndex = 0;
        public int DominionIndex { get => m_dominionIndex; set => m_dominionIndex = value; }

        // 領域名
        public string m_name = "";
        public string Name { get => m_name; set => m_name = value; }

        // テリトリインデックス
        public int m_territoryIndex = -1;
        public int TerritoryIndex { get => m_territoryIndex; set => m_territoryIndex = value; }

        // true...統治済み
        public bool m_ruleFlag = false;
        public bool RuleFlag { get => m_ruleFlag; set => m_ruleFlag = value; }


        // 統治している王国 ID (-1...統治されていない)
        public int m_kingdomId = -1;
        public int GovernKingdomId { get => m_kingdomId; set => m_kingdomId = value; }

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



    /// 領域 管理
    /// プレイ中に生成されているすべての SfDominion
    /// 保存時は別ファイルの別クラスに実装
    /// </summary>
    public class SfDominionTable : RecordTable<SfDominion>
    {


        // 登録
        public void Regist(SfDominion record) => RecordList.Add(record);

        // 領域レコードの取得
        public override SfDominion Get(uint id) => RecordList.Find(r => r.Id == id);

        public SfDominion GetAtTerritoryIndex(int territoryIndex) => RecordList.Find(r => r.TerritoryIndex == territoryIndex);

        /// <summary>
        /// 統治済みのフラグの変更
        /// </summary>
        /// <param name="dominionId"></param>
        /// <param name="ruleFlag"></param>
        public void ChangeRuleFlag(uint dominionId, bool ruleFlag) => Get(dominionId).RuleFlag = ruleFlag;

        /// <summary>
        /// 王国 ID の変更
        /// </summary>
        /// <param name="dominionId"></param>
        /// <param name="kingdomId"></param>
        public void ChangeKingdomId(uint dominionId, int kingdomId) => Get(dominionId).GovernKingdomId = kingdomId;

        /// <summary>
        /// 拠点フラグの変更
        /// </summary>
        /// <param name="dominionId"></param>
        /// <param name="capitalFlag"></param>
        public void ChangeCapitalFlag(uint dominionId, bool capitalFlag) => Get(dominionId).CapitalFlag = capitalFlag;

        /// <summary>
        /// インデックスの一番小さい地域 ID を取得
        /// </summary>
        /// <param name="dominionId"></param>
        /// <param name="groupType"></param>
        /// <returns></returns>
        public uint GetMinimumCellIndexArea(uint dominionId, eAreaGroupType groupType = eAreaGroupType.None) {
            var areaIdList = Get(dominionId).AreaIdList;
            uint minimumAreaId = 0;
            foreach (uint areaId in areaIdList) {
                SfArea areaRecord = SfAreaTableManager.Instance.Table.Get(areaId);
                if (areaRecord.AreaGroupType == groupType) {
                    minimumAreaId = areaId;
                    break;
                }
            }
            return minimumAreaId;
        }
    }

    /// <summary>
    /// 領域管理
    /// </summary>
    public class SfDominionTableManager : Singleton<SfDominionTableManager>
    {
        private SfDominionTable m_table = new SfDominionTable();
        public SfDominionTable Table => m_table;

        // ユニーク ID リスト
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        /// <summary>
        /// 読み込み処理
        /// </summary>
        public void Load()
        {
            RecordTableESDirector<SfDominion> director = new RecordTableESDirector<SfDominion>(new ESLoadBuilder<SfDominion, SfDominionTable>("SfDominionTable"));
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
            var director = new RecordTableESDirector<SfDominion>(new ESSaveBuilder<SfDominion>("SfDominionTable", m_table));
            director.Construct();
        }
    }
}