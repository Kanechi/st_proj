using System.Collections;
using System.Collections.Generic;
using TGS;
using UnityEngine;
using System.Linq;

namespace sfproj {
	/// <summary>
	/// TGS のテリトリを作成
	/// </summary>
	public class SfInitializeTGSTerritorySceneCommand : SfInMatchSceneCommandBase
	{
		private enum ePhase
		{
			SetScene,
			SettingCellColor,
			CreateTerritory
		}

		private SfInMatchGameSceneController m_scene = null;

		public SfInitializeTGSTerritorySceneCommand() => m_phaseNo = (int)ePhase.SetScene;

		/// <summary>
		/// セルの初期化
		/// コリダーに触れている領域のセルをすべて非表示にする
		/// </summary>
		/// <param name="collider"></param>
		private void InitCells(BoxCollider collider)
		{
			var tgs = m_scene.TGS;

			// コリダーにふれているセルのリストを取得
			List<int> cellsUnderBoxCollider = m_scene.GetUnderBoxColliderCells(collider);

			// セルが属しているテリトリのセルをすべて非表示にしていく
			if (cellsUnderBoxCollider != null)
			{
				for (int k = 0; k < cellsUnderBoxCollider.Count; k++)
				{

					var territoryIndex = tgs.cells[cellsUnderBoxCollider[k]].territoryIndex;

					foreach (var cell in tgs.territories[territoryIndex].cells)
					{
						// テリトリのセルを非表示
						tgs.CellSetVisible(cell.index, false);
					}

					// 非表示にするテリトリの隣をチェック
					var territory = tgs.territories[territoryIndex];

					foreach (Territory t in territory.neighbours)
					{
						t.neighbourVisible = false;
					}

					// テリトリを非表示
					tgs.TerritorySetVisible(territoryIndex, false);
				}
			}
		}

		/// <summary>
		/// セル色の設定
		/// </summary>
		/// <param name="scene"></param>
		private void SettingCellColor()
		{
			var list = m_scene.TGS.cells.ToList();

			foreach (var cell in list)
			{
				m_scene.TGS.CellSetColor(cell, SfConfigController.Instance.LandColor);
			}

			m_phaseNo = (int)ePhase.CreateTerritory;
		}

		/// <summary>
		/// TGS テリトリの作成
		/// </summary>
		/// <param name="scene"></param>
		private void CreateTerritory()
		{
			var colliderList = m_scene.GetLandDivideCollider(SfConfigController.Instance.LandType);

			foreach (var collider in colliderList)
			{
				InitCells(collider);
			}

			m_scene.Invoker.ChangeSceneState(eSceneState.CreateDominionInWorld);
		}

		public override void Update(SfInMatchGameSceneController scene)
		{
			switch ((ePhase)m_phaseNo)
			{
				case ePhase.SetScene:
					m_scene = scene;
					m_phaseNo = (int)ePhase.SettingCellColor;
					break;
				case ePhase.SettingCellColor:
					SettingCellColor();
					break;
				case ePhase.CreateTerritory:
					CreateTerritory();
					break;
			}
		}
	}
}
