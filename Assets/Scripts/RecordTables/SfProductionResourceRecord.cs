using UnityEngine;
using System;

using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace sfproj
{


    /// <summary>
    /// 生産資源カテゴリ
    /// </summary>
    public enum eProductionResouceCategory : uint
    {
        None        = 0,
        // 穀物(イネ、トウモロコシ、麦など)
        Grain       = 110000,
        // 鉱物(石、銅、鉄など)
        Mineral     = 120000,
        // モンスター素材(皮、皮膚、肌、表皮、毛皮、肉、爪など)
        Monster     = 130000,
        // 植物素材(野菜、果実など)
        Plant       = 140000,
        // 木(スギの木、樫の木など)
        Wood        = 150000,
    }

    /// <summary>
    /// 生産資源カテゴリフラグ
    /// </summary>
    public enum eProductionResourceCategoryFlag : ulong
    {
        Grain = 1ul << 0,
        Mineral = 1ul << 1,
        Monster = 1ul << 2,
        Plant = 1ul << 3,
        Wood = 1ul << 4,
    }

    /// <summary>
    /// 生産資源
    /// </summary>
    [Serializable]
    public class SfProductionResourceRecord
    {
        // 生産資源画像
        [SerializeField, HideLabel, PreviewField(55), HorizontalGroup(55, LabelWidth = 67)]
        protected Sprite m_icon = null;
        public Sprite Sprite => m_icon;


        // 生産資源 ID
        [SerializeField]
        private uint m_id = 0;
        public uint Id => m_id;

        // 生産資源カテゴリ
        [SerializeField]
        private eProductionResouceCategory m_category = eProductionResouceCategory.None;
        public eProductionResouceCategory Category => m_category;

        /// <summary>
        /// 基準となる名称
        /// ゲーム開始時世界が生成された時点でその地域で生産される生産資源は決定される
        /// その際にこの名前が小麦であった場合、～小麦という名称に変化して保存される
        /// 例えばライラック小麦が生産される場合、
        /// マーケットを建設するとライラックパン、もしくはライラックパスタが生産される
        /// </summary>
        [SerializeField]
        private List<string> m_baseNameList;
        public List<string> BaseNameList => m_baseNameList;
    }



#if false
    /// <summary>
    /// この部分を生産物ごとにファイル分けし量産すれば Unity で管理が楽になる
    /// </summary>
    [CreateAssetMenu(menuName = "RecordTables/Create SfProductionResourceTable", fileName = "SfProductionResourceTable", order = 10001)]
    public class SfProductionResourceTable : EditorRecordTable<SfProductionResource>
    {
        // assets path
        static private readonly string ResourcePath = "RecordTables/SfProductionResourceTable";
        // singleton instance
        protected static SfProductionResourceTable s_instance = null;
        // singleton getter
        public static SfProductionResourceTable Instance => (s_instance != null ? s_instance : s_instance = Resources.Load(ResourcePath) as SfProductionResourceTable);
        // get record
        public override SfProductionResource Get(uint id) => m_recordList.Find(r => r.Id == id);
    }
#endif



    public class SfProductionResourceTableManager : Singleton<SfProductionResourceTableManager>
    {
        public List<SfProductionResourceRecord> GetAllProductionResourceList() {

            var list = new List<SfProductionResourceRecord>();

            list.AddRange(SfProductionResourceGrainRecordTable.Instance.RecordList);
            list.AddRange(SfProductionResourceMineralRecordTable.Instance.RecordList);
            list.AddRange(SfProductionResourceMonsterRecordTable.Instance.RecordList);
            list.AddRange(SfProductionResourcePlantRecordTable.Instance.RecordList);
            list.AddRange(SfProductionResourceWoodRecordTable.Instance.RecordList);

            return list;
        }

        /// <summary>
        /// カテゴリ別の生産資源レコードリストを取得「
        /// </summary>
        /// <returns></returns>
        public List<SfProductionResourceRecord> GetProductionResourceListByCategory(eProductionResouceCategory category) {
            switch (category)
            {
                case eProductionResouceCategory.Grain: return SfProductionResourceGrainRecordTable.Instance.RecordList;
                case eProductionResouceCategory.Mineral: return SfProductionResourceMineralRecordTable.Instance.RecordList;
                case eProductionResouceCategory.Monster: return SfProductionResourceMonsterRecordTable.Instance.RecordList;
                case eProductionResouceCategory.Plant: return SfProductionResourcePlantRecordTable.Instance.RecordList;
                case eProductionResouceCategory.Wood: return SfProductionResourceWoodRecordTable.Instance.RecordList;
            }
            return null;
        }

        public SfProductionResourceRecord Get(uint id) {

            if (id >= (uint)eProductionResouceCategory.Grain && id < (uint)eProductionResouceCategory.Mineral)
            {
                return SfProductionResourceGrainRecordTable.Instance.Get(id);
            }
            else if (id >= (uint)eProductionResouceCategory.Mineral && id < (uint)eProductionResouceCategory.Monster)
            {
                return SfProductionResourceMineralRecordTable.Instance.Get(id);
            }
            else if (id >= (uint)eProductionResouceCategory.Monster && id < (uint)eProductionResouceCategory.Plant)
            {
                return SfProductionResourceMonsterRecordTable.Instance.Get(id);
            }
            else if (id >= (uint)eProductionResouceCategory.Plant && id < (uint)eProductionResouceCategory.Wood)
            {
                return SfProductionResourcePlantRecordTable.Instance.Get(id);
            }
            else if (id >= (uint)eProductionResouceCategory.Wood)
            {
                return SfProductionResourceWoodRecordTable.Instance.Get(id);
            }

            return null;
        }
    }
}