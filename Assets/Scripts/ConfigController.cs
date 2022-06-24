using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace sfproj {
    public class ConfigController : SingletonMonoBehaviour<ConfigController> {

        [Title("ゲーム上で設定可能", horizontalLine: false)]

        [Title("プレイ設定")]

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

        [Title("ゲーム上で設定可能", horizontalLine: false)]

        [Title("バランス設定")]

        // 領域に出現する地域の最低値
        [SerializeField, Range(1, 9)]
        private int m_minAreaValue = 2;
        public int MinAreaValue => m_minAreaValue;

        // 領域に出現する地域の最大値
        [SerializeField, Range(1, 9)]
        private int m_maxAreaValue = 6;
        public int MaxAreaValue => m_maxAreaValue;

        [Title("町と遺跡と洞窟、合わせて100%", horizontalLine: false)]
        // 領域の地域に設定される町の割合
        [SerializeField, Range(0, 100)]
        private int m_areaTownRate = 80;
        public int AreaTownRate => m_areaTownRate;

        // 領域の地域に設定される遺跡の割合
        [SerializeField, Range(0, 100)]
        private int m_areaRemainsRate = 10;
        public int AreaRemainsRate => m_areaRemainsRate;

        // 領域の地域に設定される洞窟の割合
        [SerializeField, Range(0, 100)]
        private int m_areaCaveRate = 10;
        public int AreaCaveRate => m_areaCaveRate;

        // 地域に設定される区域の最低値
        [SerializeField, Range(1, 20)]
        private int m_minZoneValue = 5;
        public int MinZoneValue => m_minZoneValue;

        // 地域に設定される区域の最大値
        [SerializeField, Range(1, 20)]
        private int m_maxZoneValue = 10;
        public int MaxZoneValue => m_maxZoneValue;

        [Title("地形の分布")]

        // 平原が分布される割合
        [SerializeField, Range(0, 100)]
        private int m_distributionRatioPlane = 80;
        public int DistributionRatioPlane => m_distributionRatioPlane;

        // 山が分布される割合
        [SerializeField, Range(0, 100)]
        private int m_distributionRatioMountain = 20;
        public int DistributionRatioMountain => m_distributionRatioMountain;

        // 森が分布される割合
        [SerializeField, Range(0, 100)]
        private int m_distributionRatiofForest = 30;
        public int DistributionRatioForest => m_distributionRatiofForest;

        // 川が分布される割合
        [SerializeField, Range(0, 100)]
        private int m_distributionRatioRiver = 5;
        public int DistributionRatioRiver => m_distributionRatioRiver;

        [Title("ゲーム上で設定不可")]

        // 国以外の土地の色
        [SerializeField]
        private Color m_landColor;
        public Color LandColor => m_landColor;

    }
}