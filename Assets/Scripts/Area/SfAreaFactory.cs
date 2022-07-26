using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj {

    /// <summary>
    /// 地域生成 工場 基底
    /// 2022/0608
    ///     都市と遺跡と洞窟で分けるべき？
    ///     最大区域数 = 最大調査数？
    /// </summary>
    public abstract class SfAreaFactoryBase
    {
        public SfAreaData Create(uint uniqueId, int areaIndex, uint dominionId)
        {
            var data = CreateAreaData();

            // 地域 ID の設定
            data.Id = uniqueId;

            // 地域名を設定
            data.Name = CreateRandomAreaName();

            // 領域 ID を設定
            data.DominionId = dominionId;

            // 地域インデックスの設定
            data.AreaIndex = areaIndex;

            // 地域種タイプの設定
            data.AreaGroupType = SettingRandomAreaGroupType();

            // 地域タイプの設定
            data.AreaType = RandomSettingAreaType();

            // 隣接地形タイプの設定
            data.ExistingTerrain = SettingExistingTerrain(SfDominionRecordTableManager.Instance.Get(dominionId));

            // 最大区域数の設定
            int ct = data.MaxZoneCount = CulcMaxZoneCount();

            for (int i = 0; i < ct; ++i)
            {
                data.ZoneFacilityList.Add(new SfAreaData.ZoneFacilitySet(i));
            }

            return data;
        }

        /// <summary>
        /// 地域レコードを生成
        /// </summary>
        /// <returns></returns>
        protected abstract SfAreaData CreateAreaData();

        /// <summary>
        /// 地域名をランダムに生成
        /// </summary>
        /// <returns></returns>
        protected abstract string CreateRandomAreaName();

        // 地域種タイプを設定
        protected abstract eAreaGroupType SettingRandomAreaGroupType();

        // ランダムに地域タイプを設定
        protected abstract eAreaType RandomSettingAreaType();

        // 隣接地形タイプの設定
        protected abstract eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion);

        // 最大区域数の計算
        protected abstract int CulcMaxZoneCount();
    }

    /// <summary>
    /// 地域レコード生成 工場
    /// </summary>
    public abstract class SfAreaCreateFactory : SfAreaFactoryBase
    {
        protected override SfAreaData CreateAreaData()
        {
            return new SfAreaData();
        }
    }

    /// <summary>
    /// 地域 平地 生成 工場
    /// </summary>
    public class SfAreaFactoryPlane : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test_plane"; }

        // 地域種タイプを設定
        protected override eAreaGroupType SettingRandomAreaGroupType() { return eAreaGroupType.Plane; }

        // 地域タイプをランダムに設定
        protected override eAreaType RandomSettingAreaType() { return eAreaType.Town; }

        // 存在する地形の設定
        protected override eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion)
        {
            eExistingTerrain terrain = 0;

            // 平原、山、森、海、は確率分布だが、重複も可能なのでそれぞれをそれぞれだけの割合で計算


            // 平原
            float rate = UnityEngine.Random.value * 100.0f;
            if (SfConfigController.Instance.DistributionRatioPlane > rate)
            {
                terrain |= eExistingTerrain.Plane;
            }

            // 森
            rate = UnityEngine.Random.value * 100.0f;
            if (SfConfigController.Instance.DistributionRatioForest > rate)
            {
                terrain |= eExistingTerrain.Forest;
            }

            // 山
            rate = UnityEngine.Random.value * 100.0f;
            if (SfConfigController.Instance.DistributionRatioMountain > rate)
            {
                terrain |= eExistingTerrain.Mountain;
            }

            // 川
            rate = UnityEngine.Random.value * 100.0f;
            if (SfConfigController.Instance.DistributionRatioRiver > rate)
            {
                terrain |= eExistingTerrain.River;
            }

            // 海のみ領域が海に面しているかどうかをチェックしてフラグを立てる
            if (dominion.NeighboursOceanFlag == true)
            {
                rate = UnityEngine.Random.value * 100.0f;
                if (SfConfigController.Instance.DistributionRatioOcean > rate)
                {
                    terrain |= eExistingTerrain.Ocean;
                }
            }

            // 何も地形が無かった場合は平原を設定
            if (terrain == 0)
                terrain |= eExistingTerrain.Plane;

            return terrain;
        }


        // 区域最大数の計算
        protected override int CulcMaxZoneCount() { return UnityEngine.Random.Range(SfConfigController.Instance.MinZoneValue, SfConfigController.Instance.MaxZoneValue + 1); }
    }

    /// <summary>
    /// 地域 遺跡 生成 工場
    /// </summary>
    public class SfAreaFactoryRemains : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test_reamins"; }

        // 地域種タイプを設定
        protected override eAreaGroupType SettingRandomAreaGroupType() { return eAreaGroupType.Remains; }

        // 地域タイプをランダムに設定
        protected override eAreaType RandomSettingAreaType() { return eAreaType.Remains; }

        // 存在する地形の設定 (遺跡は地形効果はきにしない)
        protected override eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion)
        {
            return 0;
        }

        protected override int CulcMaxZoneCount() { return -1; }
    }

    /// <summary>
    /// 地域 洞窟 生成 工場
    /// </summary>
    public class SfAreaFactoryCave : SfAreaCreateFactory
    {
        protected override string CreateRandomAreaName() { return "test_cave"; }

        // 地域種タイプを設定
        protected override eAreaGroupType SettingRandomAreaGroupType() { return eAreaGroupType.Cave; }

        // 地域タイプをランダムに設定
        protected override eAreaType RandomSettingAreaType() { return eAreaType.Cave; }

        // 存在する地形の設定 (洞窟は地形効果はきにしない)
        protected override eExistingTerrain SettingExistingTerrain(SfDominionRecord dominion)
        {
            return 0;
        }

        protected override int CulcMaxZoneCount() { return -1; }
    }

    /// <summary>
    /// 地域 生成 工場 管理
    /// </summary>
    public class SfAreaFactoryManager : Singleton<SfAreaFactoryManager>
    {
        private List<SfAreaFactoryBase> factoryList = null;

        public SfAreaFactoryManager()
        {

            factoryList = new List<SfAreaFactoryBase>()
            {
                new SfAreaFactoryPlane(),
                new SfAreaFactoryRemains(),
                new SfAreaFactoryCave(),
            };
        }

        /// <summary>
        /// 地域の作成
        /// </summary>
        /// <param name="areaIndex">地域インデックス(セル番号)</param>
        /// <param name="dominionId">属している領域 ID</param>
        /// <returns></returns>
        public SfAreaData RandomCreate(int areaIndex, uint dominionId, eAreaGroupType factoryType = eAreaGroupType.None)
        {
            // 町か遺跡か洞窟かをランダム
            // 割合は設定できるようにする
            float rate = UnityEngine.Random.value * 100.0f;

            if (factoryType == eAreaGroupType.None)
            {
                // 町 (rate が 80 以下なら町)
                if (SfConfigController.Instance.AreaTownRate > rate)
                {
                    // 町
                    factoryType = eAreaGroupType.Plane;
                }
                // 遺跡 (rate が 80 から 90 なら遺跡)
                else if (SfConfigController.Instance.AreaTownRate <= rate && (SfConfigController.Instance.AreaTownRate + SfConfigController.Instance.AreaRemainsRate) > rate)
                {
                    // 遺跡
                    factoryType = eAreaGroupType.Remains;
                }
                // 洞窟 (rate が 90 から 100 なら遺跡)
                else if ((SfConfigController.Instance.AreaTownRate + SfConfigController.Instance.AreaRemainsRate) <= rate && (SfConfigController.Instance.AreaTownRate + SfConfigController.Instance.AreaRemainsRate + SfConfigController.Instance.AreaCaveRate) > rate)
                {
                    // 洞窟
                    factoryType = eAreaGroupType.Cave;
                }
            }

            if (factoryType == eAreaGroupType.None)
            {
                Debug.LogError("factoryType == -1 !!!");
                return null;
            }

            // ユニーク ID の作成
            uint uniqueId = SfConstant.CreateUniqueId(ref SfAreaDataTableManager.Instance.m_uniqueIdList);

            // 地域データを作成
            var data = factoryList[(int)factoryType].Create(uniqueId, areaIndex, dominionId);

            return data;
        }
    }
}