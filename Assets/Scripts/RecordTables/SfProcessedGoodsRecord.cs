using UnityEngine;
using System;

using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace sfproj
{
    public enum eProcessdGoodsCategory
    {
        None = 0,

        // 食材(穀物、果実、野菜、肉から生成)
        Food = 10000,

        // 装備(鉱物、木、皮から生成)
        Equipment = 20000,

        // 武具(鉱物、木、皮から生成)
        Arms = 30000,

        // ルーン(石から生成)
        Rune = 40000,
    }



    /// <summary>
    /// 加工品
    /// </summary>
    [Serializable]
    public class SfProcessedGoodsRecord
    {
        /// <summary>
        /// 加工品を作成する際に必要な生産資源とその比率
        /// </summary>
        [Serializable]
        public class SfResourceRatio
        {

            // 生産資源 ID
            [SerializeField]
            private uint m_id = 0;
            public uint Id => m_id;

            // 比率
            [SerializeField, Range(0,100)]
            private float m_ratio = 0.0f;
            public float Ratio => m_ratio;
        }

        // アイコン
        [SerializeField, HideLabel, PreviewField(55), HorizontalGroup(55, LabelWidth = 67)]
        protected Sprite m_icon = null;
        public Sprite Sprite => m_icon;

        // 生産資源 ID
        [SerializeField]
        private uint m_id = 0;
        public uint Id => m_id;

        [SerializeField]
        private eProcessdGoodsCategory m_category = eProcessdGoodsCategory.None;
        public eProcessdGoodsCategory Category => m_category;

        /// <summary>
        /// 基準となる名称
        /// ゲーム開始時世界が生成された時点でその地域で生産される生産資源は決定される
        /// その際にこの名前が小麦であった場合、～小麦という名称に変化して保存される
        /// 例えばライラック小麦が生産される場合、
        /// マーケットを建設するとライラックパン、もしくはライラックパスタが生産される
        /// </summary>
        [SerializeField]
        private string m_baseName;
        public string BaseName => m_baseName;    

        // 説明文
        [SerializeField]
        private string m_desc = null;
        public string Desc => m_desc;

        /// <summary>
        /// 比率
        /// リストの０番から降順
        /// 足して 100% になるように設計すること
        /// 
        /// </summary>
        [SerializeField]
        private List<SfResourceRatio> m_resourceRatio = null;
        public List<SfResourceRatio> ResourceRatio => m_resourceRatio;
    }
}