using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace sfproj {


	/// <summary>
	/// 王国を作成
	/// </summary>
	public class SfCreateKingdomInWorldSceneCommand : SfInMatchSceneCommandBase
	{
		public override void Update(SfInMatchGameSceneController scene)
		{
			int kingdomCount = SfConfigController.Instance.KingdomCount;

			// 自身の王国を作成する
			{
				var kingdomRecord = SfKingdomFactoryManager.Instance.Create(true);

				SfKingdomTableManager.Instance.Table.Regist(kingdomRecord);

				kingdomCount--;
			}

			// 自身の国を除いた数分の最大王国数だけその他の王国を作成
			for (int i = 0; i < kingdomCount; ++i)
			{
				var kingdomRecord = SfKingdomFactoryManager.Instance.Create(false);

				SfKingdomTableManager.Instance.Table.Regist(kingdomRecord);
			}

			// 生成した王国に領域を割り当てていく

			// 領域リスト
			var dominionRecordList = SfDominionTableManager.Instance.Table.RecordList.ToList();

			// 領域の数
			int dominionCount = dominionRecordList.Count;

			// 最大王国数だけランダムに領域を色分け
			for (int i = 0; i < SfConfigController.Instance.KingdomCount; ++i)
			{
				// ランダムに領域を選択
				var dominionRecord = dominionRecordList[Random.Range(0, dominionCount)];

				// 自身の王国のみ設定した色を使用
				Color color = i == 0 ? SfConfigController.Instance.KingdomColor : Random.ColorHSV();
				color.a = 0.4f;

				// 領域からテリトリを取得
				var territory = scene.TGS.territories[dominionRecord.m_territoryIndex];

				// テリトリの色を王国の色に設定
				foreach (var cell in territory.cells)
					scene.TGS.CellSetColor(cell, color);

				// 王国に領域 ID を設定
				// この際に設定する王国はランダム設定なので Index 順で構わない
				var kingdomRecord = SfKingdomTableManager.Instance.Table.RecordList[i];
				kingdomRecord.m_sfDominionIdList.Add(dominionRecord.Id);

				// 領域番号(領域を手に入れた順の番号)
				int dominionIndex = 0;

				// ゲーム開始時領域の初期化
				StartInitializeDominion(kingdomRecord.Id, dominionRecord.Id, dominionIndex);


				// 王国にした領域を外す
				dominionRecordList.Remove(dominionRecord);
				// 領域数を更新
				dominionCount = dominionRecordList.Count;
			}

			scene.Invoker.ChangeSceneState(eSceneState.MainUpdate);
		}

		/// <summary>
		/// 王国に割り当てられた領域の初期化
		/// </summary>
		/// <param name="dominionRecord"></param>
		private void StartInitializeDominion(int kingdomId, uint dominionId, int index)
		{

			// 王国 ID を変更
			SfDominionTableManager.Instance.Table.ChangeKingdomId(dominionId, kingdomId);

			// 占領フラグを設定
			SfDominionTableManager.Instance.Table.ChangeRuleFlag(dominionId, true);

			// 領域の０番を首都領域として設定
			if (index == 0)
				SfDominionTableManager.Instance.Table.ChangeCapitalFlag(dominionId, true);

			// 領域の地域を初期化



			// インデックスの一番小さい通常の地域 IDを取得
			var minimumPlaneAreaId = SfDominionTableManager.Instance.Table.GetMinimumCellIndexArea(dominionId, eAreaGroupType.Normal);


			// ==================================
			// 初期の町を地域に作成
			// ==================================


			// 地域レコードを取得
			var areaRecord = SfAreaTableManager.Instance.Table.Get(minimumPlaneAreaId);

			// 地域をアンロック
			areaRecord.AreaDevelopmentState = eAreaDevelopmentState.Completed;

			// アンロックした地域を拠点として設定
			areaRecord.BaseFlag = true;

			// 初期人口を設定
			// 起源などがあるならそれによって人口を変更する？
			// 人口は全て同じ方が良いか？
			areaRecord.Population = 100;


			
			// 初期町は必ず平地が存在するので田畑の作成が出来る


			// TODO: 以下はデバッグの処理実際は必ず平地が作成されるので田畑しか作成されない
			// 初期区域を設定(川だけに面していた場合何も設置されない)
			if ((areaRecord.ExistingTerrain & eExistingTerrain.Plane) != 0)
			{
				// アンロックした地域が平地に面していたら田畑を新規で建設
				SfZoneFacilityTableManager.Instance.Table.BuildZoneFacilityType(minimumPlaneAreaId, 0, eZoneFacilityType.Production_Farm, 1);
			}
			else if ((areaRecord.ExistingTerrain & eExistingTerrain.Forest) != 0)
			{
				// アンロックした地域が森に面していたら伐採所を新規で建設
				SfZoneFacilityTableManager.Instance.Table.BuildZoneFacilityType(minimumPlaneAreaId, 0, eZoneFacilityType.Production_LoggingArea, 1);
			}
			else if ((areaRecord.ExistingTerrain & eExistingTerrain.Mountain) != 0)
			{
				// アンロックした地域が山に面していたら採掘所を新規で建設
				SfZoneFacilityTableManager.Instance.Table.BuildZoneFacilityType(minimumPlaneAreaId, 0, eZoneFacilityType.Production_Mining, 1);
			}
			else if ((areaRecord.ExistingTerrain & eExistingTerrain.Ocean) != 0)
			{
				// アンロックした地域が海に面していたら港を新規で建設
				SfZoneFacilityTableManager.Instance.Table.BuildZoneFacilityType(minimumPlaneAreaId, 0, eZoneFacilityType.Commercial_Harbor, 1);
			}
		}
	}
}
