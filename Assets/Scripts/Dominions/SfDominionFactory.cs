using System.Collections;
using System.Collections.Generic;
using TGS;
using UnityEngine;

namespace sfproj
{

    /// <summary>
    /// 領域生成工場  基底
    /// 2022/06/08
    ///     現状種類が存在するわけではないので分ける必要はないかも
    ///     一応テンプレート化しておく
    /// </summary>
    public abstract class SfDominionFactoryBase
    {
        public SfDominion Create(uint uniqueId, int territoryIndex)
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
        protected abstract SfDominion CreateRecord();

        // 領域名の作成
        protected abstract string CreateName();

        /// <summary>
        /// 海に隣接しているかチェック
        /// 隣接しているテリトリに１つでも非表示があれば海
        /// </summary>
        /// <returns>true...海に隣接している</returns>
        private bool CheckAdjastingOceanTerrain(int territoryIndex)
        {
            return (TerrainGridSystem.instance.territories[territoryIndex].neighbourVisible == false);
        }
    }

    /// <summary>
    /// 領域レコード生成工場
    /// </summary>
    public abstract class SfDominionCreateFactory : SfDominionFactoryBase
    {
        protected override SfDominion CreateRecord()
        {
            return new SfDominion();
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
    public class SfDominionFactoryManager : Singleton<SfDominionFactoryManager>
    {

        /// <summary>
        /// 領域レコードの作成
        /// </summary>
        /// <param name="territoryIndex"></param>
        /// <returns></returns>
        public SfDominion Create(int territoryIndex)
        {

            // ひとまず SfDominionFactory しかない
            var factory = new SfDominionFactory();

            // ユニーク ID の作成
            uint uniqueId = SfConstant.CreateUniqueId(ref SfDominionTableManager.Instance.m_uniqueIdList);

            // 領域レコードを作成
            var record = factory.Create(uniqueId, territoryIndex);

            return record;
        }
    }

}