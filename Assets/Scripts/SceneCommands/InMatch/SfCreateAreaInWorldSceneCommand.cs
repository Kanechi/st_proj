using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj {
	/// <summary>
	/// 地域の作成
	/// </summary>
	public class SfCreateAreaInWorldSceneCommand : SfInMatchSceneCommandBase
	{
		public override void Update(SfInMatchGameSceneController scene)
		{
			// 領域数
			int dominionRecordCount = SfDominionTableManager.Instance.Table.RecordList.Count;

			// 生成した領域数分地域をランダムに生成
			for (int i = 0; i < dominionRecordCount; ++i)
			{
				// 領域を取得
				var dominion = SfDominionTableManager.Instance.Table.RecordList[i];

				// ランダムな数を設定して地域を生成(最低値は設定可能で１以下は無し、最大値は設定可能)
				int areaIncDec = Random.Range(0, SfConfigController.Instance.AreaIncDecValue + 1);

				// 領域のテリトリにあるセルの数を取得
				var cellCount = scene.TGS.territories[dominion.m_territoryIndex].cells.Count;

				int areaCount = cellCount <= areaIncDec ? cellCount : cellCount + areaIncDec;

				int cellNo = 0;
				while (cellNo < areaCount)
				{
					// 地域を生成
					var area = SfAreaFactoryManager.Instance.RandomCreate(cellNo, dominion.Id);

					SfAreaTableManager.Instance.Table.Regist(area);

					// 生成した地域を領域に設定していく
					dominion.AreaIdList.Add(area.Id);

					cellNo++;
				}

				// スクロールセルのすべてが遺跡と洞窟に割り当てられてないかをチェック
				// 町が１でもないとだめなのでチェック

				bool isOk = false;
				foreach (var areaId in dominion.AreaIdList)
				{
					var area = SfAreaTableManager.Instance.Table.Get(areaId);

					if (area.AreaGroupType == eAreaGroupType.Normal)
					{
						isOk = true;
						break;
					}
				}

				// 通常が無かった場合は
				if (isOk == false)
				{
					// 通常を追加
					var area = SfAreaFactoryManager.Instance.RandomCreate(cellNo, dominion.Id, eAreaGroupType.Normal);

					SfAreaTableManager.Instance.Table.Regist(area);

					// 生成した地域を領域に設定していく
					dominion.AreaIdList.Add(area.Id);

					cellNo++;
				}

				// 領域の一番左端の通常地域に平原の地形を作成
				{
					var areaId = SfDominionTableManager.Instance.Table.GetMinimumCellIndexArea(dominion.Id, eAreaGroupType.Normal);

					SfAreaTableManager.Instance.Table.AddTerrain(areaId, eExistingTerrain.Plane);
				}
			}

			scene.Invoker.ChangeSceneState(eSceneState.CreateKingdomInWorld);
		}
	}

}
