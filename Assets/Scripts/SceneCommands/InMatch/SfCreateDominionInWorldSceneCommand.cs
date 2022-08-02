using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TGS;
using UnityEngine;

namespace sfproj {

	/// <summary>
	/// 領域の作成
	/// </summary>
	public class SfCreateDominionInWorldSceneCommand : SfInMatchSceneCommandBase
	{
		public override void Update(SfInMatchGameSceneController scene)
		{
			// 表示しているテリトリリストを取得
			List<Territory> dispTerritoryList = scene.TGS.territories.Where(t => t.visible == true).ToList();

			// 表示されているテリトリの数
			int dispTerritoryCount = dispTerritoryList.Count;

			// 表示されているテリトリの数だけ領域を作成
			for (int i = 0; i < dispTerritoryCount; ++i)
			{

				// 領域を作成しテリトリリストのテリトリインデックスを生成した領域に紐づけ
				var dominionRecord = SfDominionFactoryManager.Instance.Create(scene.TGS.TerritoryGetIndex(dispTerritoryList[i]));

				// 生成した領域を領域管理にとりつけ
				SfDominionTableManager.Instance.Table.Regist(dominionRecord);
			}

			scene.Invoker.ChangeSceneState(eSceneState.CreateAreaInWorld);
		}
	}
}
