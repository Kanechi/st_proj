using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Sirenix.OdinInspector;

namespace stproj {

    /// <summary>
    /// ゲームプレイ詳細
    /// </summary>
    [Serializable]
    public class GamePlayDetailRecord : IJsonParser
    {
        // 識別 ID
        [SerializeField]
        private uint m_id = 0;
        public uint Id { get => m_id; set => m_id = value; }

        // 保存番号
        [SerializeField]
        private uint m_saveNo = 0;
        public uint SaveNo { get => m_saveNo; set => m_saveNo = value; }

        /// <summary>
        /// 土地詳細
        /// </summary>
        [Serializable]
        public class LandDetail
        {
            // tgs のシード値
            public int m_tgsSeedValue = 0;

            // 土地タイプ
            public eLandType m_landType = eLandType.OneLand;

            // 土地サイズ
            public int m_landSize = 0;
        }

        /// <summary>
        /// 地域詳細(セル１つ分)
        /// </summary>
        [SerializeField]
        public class AreaDetail
        {
            // 地域 ID
            public uint m_id = 0;

            // 地域名
            public string m_name = "";

            // 区域最大数
            public int m_maxZoneCount = 0;

            // 設置している区域タイプ
            public List<eZoneType> m_zoneTypeList = new List<eZoneType>();
        }

        /// <summary>
        /// 領域詳細(スクロールビュー１つ分)
        /// どこかの王国に統治されている領土の情報
        /// </summary>
        [Serializable]
        public class DominionDetail
        {
            // 領域 ID
            public uint m_id = 0;

            // 領域が属している tgs の territory index
            public int m_territoryIndex = 0;

            // 所属している地域 ID リスト
            public List<uint> m_areaIdList = new List<uint>();
        }

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
        /// 領土詳細
        /// </summary>
        [Serializable]
        public class TerritoryDetail
        {
            // tgs の terrainIndex
            public int m_terrainIndex = 0;

            // 王国 ID(0...統治されていない)
            public uint m_kingdomId = 0;

            // 領域 ID(0...統治されていない)
            public uint m_dominionId = 0;

            // 領土名
            public string m_name = "";

            // 隣接地形フラグ
            public eAdjacentTerrainType m_adjacentTerrainTypeFlag = 0;
        }

        /// <summary>
        /// 王国詳細
        /// </summary>
        [Serializable]
        public class KingdomDetail
        {
            // 王国 ID
            public uint m_id = 0;

            // 王国名
            public string m_name = "";

            // 統治領土 ID リスト
            public List<uint> m_dominionIdList = new List<uint>();
        }

        public void Parse(IDictionary<string, object> data)
        {
        }
    }
}