using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// 作成された地域に生産資源を生成し散布する
    /// </summary>
    public class SfGenAndScatterProductionResInCreatedAreaSceneCommnad : SfInMatchSceneCommandBase
    {
        public override void Update(SfInMatchGameSceneController scene)
        {
            var areaList = SfAreaTableManager.Instance.Table.RecordList;

            foreach (var area in areaList)
            {
                if (area.AreaGroupType != eAreaGroupType.Normal)
                    continue;

                // 生産資源をいくつ生成するかを決定
                int count = UnityEngine.Random.Range(SfConfigController.Instance.MinAreaProductionResourceItemCt, SfConfigController.Instance.MaxAreaProductionResourceItemCt);

                var builder = new SfProductionResourceItemBuilder(area);
                var director = new SfItemGenDirector<SfProductionResourceItem>(builder);

                while (area.ProductionResourceItemIdList.Count < count)
                {
                    director.Construct();

                    var item = director.GetResult();

                    if (area.ProductionResourceItemIdList.Contains(item.Id) == true)
                        continue;

                    // 地域の生産資源 ID リストに登録
                    area.ProductionResourceItemIdList.Add(item.Id);
                }
            }

            var list = SfProductionResourceItemTableManager.Instance.Table.RecordList;

            scene.Invoker.ChangeSceneState(eSceneState.CreateKingdomInWorld);
        }
    }
}