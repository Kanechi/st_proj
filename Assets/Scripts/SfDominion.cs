using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{

    public enum eAdjacentTerrainType : uint
    {
        // 平地に隣接
        Plane = 1u << 0,
        // 森に隣接
        Forest = 1u << 1,
        // 海に隣接
        Ocean = 1u << 2,
        // 山に隣接
        Mountain = 1u << 3,
        // 川に隣接
        River = 1u << 4,
    }

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
        /// <summary>
        /// 領域詳細
        /// </summary>
        [Serializable]
        public class SfDominionDetail
        {
            // 領域 ID
            public uint m_id = 0;
            // 領域名
            public string m_name = "";
            // テリトリインデックス
            public int m_territoryIndex = -1;

            // true...統治済み
            public bool m_ruleFlag = false;
            // 統治している王国 ID (0...統治されていない)
            public uint m_kingdomId = 0;
            // true...首都
            public bool m_capitalFlag = false;

            // 隣接地形タイプ
            public eAdjacentTerrainType m_adjacentTerrainType = 0;

            // 地域 ID リスト
            public List<uint> m_sfAreaIdList = new List<uint>();
        }

        [SerializeField]
        private SfDominionDetail m_detail = new SfDominionDetail();

        // 領域 ID
        public uint Id { get => m_detail.m_id; set => m_detail.m_id = value; }
        // 領域名
        public string Name { get => m_detail.m_name; set => m_detail.m_name = value; }
        // テリトリインデックス
        public int TerritoryIndex { get => m_detail.m_territoryIndex; set => m_detail.m_territoryIndex = value; }

        // 統治済みフラグ
        public bool RuleFlag { get => m_detail.m_ruleFlag; set => m_detail.m_ruleFlag = value; }
        // 統治している王国 ID (0...統治されていない)
        public uint GovernKingdomId { get => m_detail.m_kingdomId; set => m_detail.m_kingdomId = value; }
        // 首都フラグ
        public bool CapitalFlag { get => m_detail.m_capitalFlag; set => m_detail.m_capitalFlag = value; }

        // 隣接地形タイプ
        public eAdjacentTerrainType AdjacentTerrainType { get => m_detail.m_adjacentTerrainType; set => m_detail.m_adjacentTerrainType = value; }
        // 地域 ID リスト
        public List<uint> AreaIdList { get => m_detail.m_sfAreaIdList; set => m_detail.m_sfAreaIdList = value; }
    }

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

            // 隣接地形タイプの設定
            record.AdjacentTerrainType = CreateAdjacentTerrainType();

            return record;
        }

        // 領域レコードの作成
        protected abstract SfDominionRecord CreateRecord();

        // 領域名の作成
        protected abstract string CreateName();

        // 隣接地形タイプの設定
        protected abstract eAdjacentTerrainType CreateAdjacentTerrainType();
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

        // 隣接地形タイプの設定
        protected override eAdjacentTerrainType CreateAdjacentTerrainType()
        {
            return eAdjacentTerrainType.Plane;
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
            uint uniqueId = SfConstant.CreateUniqueId(ref SfDominionManager.Instance.m_uniqueIdList);

            // 領域レコードを作成
            var record = factory.Create(uniqueId, territoryIndex);

            return record;
        }
    }

    /// <summary>
    /// 領域管理
    /// </summary>
    public class SfDominionManager : Singleton<SfDominionManager> {

        // ユニーク ID リスト
        public HashSet<uint> m_uniqueIdList = new HashSet<uint>();

        // 領域リスト
        private List<SfDominionRecord> m_list = new List<SfDominionRecord>();
        public List<SfDominionRecord> DominionRecordList => m_list;
    }
}