using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace sfproj {
    public class ConfigController : SingletonMonoBehaviour<ConfigController> {

        // 大陸のタイプ
        [SerializeField]
        private eLandType m_landType = eLandType.OneLand;
        public eLandType LandType => m_landType;

        // 大陸の大きさ(領域数)
        // マップサイズを小さくするとコリジョンも小さくしなければならないので
        // 表示している領域の数の多さと領域自体の大きさで調整
        // 1...少ない(領域自体の表示数が少なく、領域自体が大きい)
        // 2...普通
        // 3...多い(領域自体の表示数が多く、領域自体が小さい)
        [SerializeField, Range(1, 3)]
        private int m_landSize = 1;
        public int LandSize => m_landSize;

        // 国の数
        // 自分の国を含めた世界に初期配置されている国の数
        // 組織の数の方が良いかも
        // 最大数は大陸の大きさで変化させる必要がある
        [SerializeField, Range(2, 20)]
        private int m_kingdomCount = 2;
        public int KingdomCount => m_kingdomCount;

        // 初期の領域の数
        [SerializeField, Range(1, 3)]
        private int m_occupiedTerritoryCount = 1;
        public int OccupiedTerritoryCount => m_occupiedTerritoryCount;

        // 国の名前
        [SerializeField]
        private string m_kingdomName = "";
        public string KingdomName => m_kingdomName;

        // 王国の色
        [SerializeField]
        private Color m_kingdomColor;
        public Color KingdomColor => m_kingdomColor;

        // リーダーの髪型
        // リーダーの髪色
        // リーダーの体大きさ

        // リーダーの

        // 兵士の髪型
        // 兵士の髪色
        // 兵士の体大きさ

        // 終了年数
        [SerializeField]
        private int m_gameFinishYear = 100;
        public int GameFinishYear => m_gameFinishYear;


        /// <summary>
        /// 国以外の土地の色
        /// </summary>
        [SerializeField]
        private Color m_landColor;
        public Color LandColor => m_landColor;

    }
}