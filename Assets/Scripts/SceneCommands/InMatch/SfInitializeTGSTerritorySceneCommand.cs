using System.Collections;
using System.Collections.Generic;
using TGS;
using UnityEngine;
using System.Linq;

namespace sfproj {
	/// <summary>
	/// TGS �̃e���g�����쐬
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
		/// �Z���̏�����
		/// �R���_�[�ɐG��Ă���̈�̃Z�������ׂĔ�\���ɂ���
		/// </summary>
		/// <param name="collider"></param>
		private void InitCells(BoxCollider collider)
		{
			var tgs = m_scene.TGS;

			// �R���_�[�ɂӂ�Ă���Z���̃��X�g���擾
			List<int> cellsUnderBoxCollider = m_scene.GetUnderBoxColliderCells(collider);

			// �Z���������Ă���e���g���̃Z�������ׂĔ�\���ɂ��Ă���
			if (cellsUnderBoxCollider != null)
			{
				for (int k = 0; k < cellsUnderBoxCollider.Count; k++)
				{

					var territoryIndex = tgs.cells[cellsUnderBoxCollider[k]].territoryIndex;

					foreach (var cell in tgs.territories[territoryIndex].cells)
					{
						// �e���g���̃Z�����\��
						tgs.CellSetVisible(cell.index, false);
					}

					// ��\���ɂ���e���g���ׂ̗��`�F�b�N
					var territory = tgs.territories[territoryIndex];

					foreach (Territory t in territory.neighbours)
					{
						t.neighbourVisible = false;
					}

					// �e���g�����\��
					tgs.TerritorySetVisible(territoryIndex, false);
				}
			}
		}

		/// <summary>
		/// �Z���F�̐ݒ�
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
		/// TGS �e���g���̍쐬
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
