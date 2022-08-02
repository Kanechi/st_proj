using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace sfproj {


	/// <summary>
	/// �������쐬
	/// </summary>
	public class SfCreateKingdomInWorldSceneCommand : SfInMatchSceneCommandBase
	{
		public override void Update(SfInMatchGameSceneController scene)
		{
			int kingdomCount = SfConfigController.Instance.KingdomCount;

			// ���g�̉������쐬����
			{
				var kingdomRecord = SfKingdomFactoryManager.Instance.Create(true);

				SfKingdomTableManager.Instance.Table.Regist(kingdomRecord);

				kingdomCount--;
			}

			// ���g�̍��������������̍ő剤�����������̑��̉������쐬
			for (int i = 0; i < kingdomCount; ++i)
			{
				var kingdomRecord = SfKingdomFactoryManager.Instance.Create(false);

				SfKingdomTableManager.Instance.Table.Regist(kingdomRecord);
			}

			// �������������ɗ̈�����蓖�ĂĂ���

			// �̈惊�X�g
			var dominionRecordList = SfDominionTableManager.Instance.Table.RecordList.ToList();

			// �̈�̐�
			int dominionCount = dominionRecordList.Count;

			// �ő剤�������������_���ɗ̈��F����
			for (int i = 0; i < SfConfigController.Instance.KingdomCount; ++i)
			{
				// �����_���ɗ̈��I��
				var dominionRecord = dominionRecordList[Random.Range(0, dominionCount)];

				// ���g�̉����̂ݐݒ肵���F���g�p
				Color color = i == 0 ? SfConfigController.Instance.KingdomColor : Random.ColorHSV();
				color.a = 0.4f;

				// �̈悩��e���g�����擾
				var territory = scene.TGS.territories[dominionRecord.m_territoryIndex];

				// �e���g���̐F�������̐F�ɐݒ�
				foreach (var cell in territory.cells)
					scene.TGS.CellSetColor(cell, color);

				// �����ɗ̈� ID ��ݒ�
				// ���̍ۂɐݒ肷�鉤���̓����_���ݒ�Ȃ̂� Index ���ō\��Ȃ�
				var kingdomRecord = SfKingdomTableManager.Instance.Table.RecordList[i];
				kingdomRecord.m_sfDominionIdList.Add(dominionRecord.Id);

				// �̈�ԍ�(�̈����ɓ��ꂽ���̔ԍ�)
				int dominionIndex = 0;

				// �Q�[���J�n���̈�̏�����
				StartInitializeDominion(kingdomRecord.Id, dominionRecord.Id, dominionIndex);


				// �����ɂ����̈���O��
				dominionRecordList.Remove(dominionRecord);
				// �̈搔���X�V
				dominionCount = dominionRecordList.Count;
			}

			scene.Invoker.ChangeSceneState(eSceneState.MainUpdate);
		}

		/// <summary>
		/// �����Ɋ��蓖�Ă�ꂽ�̈�̏�����
		/// </summary>
		/// <param name="dominionRecord"></param>
		private void StartInitializeDominion(int kingdomId, uint dominionId, int index)
		{

			// ���� ID ��ύX
			SfDominionTableManager.Instance.Table.ChangeKingdomId(dominionId, kingdomId);

			// ��̃t���O��ݒ�
			SfDominionTableManager.Instance.Table.ChangeRuleFlag(dominionId, true);

			// �̈�̂O�Ԃ���s�̈�Ƃ��Đݒ�
			if (index == 0)
				SfDominionTableManager.Instance.Table.ChangeCapitalFlag(dominionId, true);

			// �̈�̒n���������



			// �C���f�b�N�X�̈�ԏ������ʏ�̒n�� ID���擾
			var minimumPlaneAreaId = SfDominionTableManager.Instance.Table.GetMinimumCellIndexArea(dominionId, eAreaGroupType.Normal);


			// ==================================
			// �����̒���n��ɍ쐬
			// ==================================


			// �n�惌�R�[�h���擾
			var areaRecord = SfAreaTableManager.Instance.Table.Get(minimumPlaneAreaId);

			// �n����A�����b�N
			areaRecord.AreaDevelopmentState = eAreaDevelopmentState.Completed;

			// �A�����b�N�����n������_�Ƃ��Đݒ�
			areaRecord.BaseFlag = true;

			// �����l����ݒ�
			// �N���Ȃǂ�����Ȃ炻��ɂ���Đl����ύX����H
			// �l���͑S�ē��������ǂ����H
			areaRecord.Population = 100;


			
			// �������͕K�����n�����݂���̂œc���̍쐬���o����


			// TODO: �ȉ��̓f�o�b�O�̏������ۂ͕K�����n���쐬�����̂œc�������쐬����Ȃ�
			// ��������ݒ�(�삾���ɖʂ��Ă����ꍇ�����ݒu����Ȃ�)
			if ((areaRecord.ExistingTerrain & eExistingTerrain.Plane) != 0)
			{
				// �A�����b�N�����n�悪���n�ɖʂ��Ă�����c����V�K�Ō���
				SfZoneFacilityTableManager.Instance.Table.BuildZoneFacilityType(minimumPlaneAreaId, 0, eZoneFacilityType.Production_Farm, 1);
			}
			else if ((areaRecord.ExistingTerrain & eExistingTerrain.Forest) != 0)
			{
				// �A�����b�N�����n�悪�X�ɖʂ��Ă����田�̏���V�K�Ō���
				SfZoneFacilityTableManager.Instance.Table.BuildZoneFacilityType(minimumPlaneAreaId, 0, eZoneFacilityType.Production_LoggingArea, 1);
			}
			else if ((areaRecord.ExistingTerrain & eExistingTerrain.Mountain) != 0)
			{
				// �A�����b�N�����n�悪�R�ɖʂ��Ă�����̌@����V�K�Ō���
				SfZoneFacilityTableManager.Instance.Table.BuildZoneFacilityType(minimumPlaneAreaId, 0, eZoneFacilityType.Production_Mining, 1);
			}
			else if ((areaRecord.ExistingTerrain & eExistingTerrain.Ocean) != 0)
			{
				// �A�����b�N�����n�悪�C�ɖʂ��Ă�����`��V�K�Ō���
				SfZoneFacilityTableManager.Instance.Table.BuildZoneFacilityType(minimumPlaneAreaId, 0, eZoneFacilityType.Commercial_Harbor, 1);
			}
		}
	}
}
