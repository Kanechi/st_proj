using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj {
	/// <summary>
	/// �n��̍쐬
	/// </summary>
	public class SfCreateAreaInWorldSceneCommand : SfInMatchSceneCommandBase
	{
		public override void Update(SfInMatchGameSceneController scene)
		{
			// �̈搔
			int dominionRecordCount = SfDominionTableManager.Instance.Table.RecordList.Count;

			// ���������̈搔���n��������_���ɐ���
			for (int i = 0; i < dominionRecordCount; ++i)
			{
				// �̈���擾
				var dominion = SfDominionTableManager.Instance.Table.RecordList[i];

				// �����_���Ȑ���ݒ肵�Ēn��𐶐�(�Œ�l�͐ݒ�\�łP�ȉ��͖����A�ő�l�͐ݒ�\)
				int areaIncDec = Random.Range(0, SfConfigController.Instance.AreaIncDecValue + 1);

				// �̈�̃e���g���ɂ���Z���̐����擾
				var cellCount = scene.TGS.territories[dominion.m_territoryIndex].cells.Count;

				int areaCount = cellCount <= areaIncDec ? cellCount : cellCount + areaIncDec;

				int cellNo = 0;
				while (cellNo < areaCount)
				{
					// �n��𐶐�
					var area = SfAreaFactoryManager.Instance.RandomCreate(cellNo, dominion.Id);

					SfAreaTableManager.Instance.Table.Regist(area);

					// ���������n���̈�ɐݒ肵�Ă���
					dominion.AreaIdList.Add(area.Id);

					cellNo++;
				}

				// �X�N���[���Z���̂��ׂĂ���ՂƓ��A�Ɋ��蓖�Ă��ĂȂ������`�F�b�N
				// �����P�ł��Ȃ��Ƃ��߂Ȃ̂Ń`�F�b�N

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

				// �ʏ킪���������ꍇ��
				if (isOk == false)
				{
					// �ʏ��ǉ�
					var area = SfAreaFactoryManager.Instance.RandomCreate(cellNo, dominion.Id, eAreaGroupType.Normal);

					SfAreaTableManager.Instance.Table.Regist(area);

					// ���������n���̈�ɐݒ肵�Ă���
					dominion.AreaIdList.Add(area.Id);

					cellNo++;
				}

				// �̈�̈�ԍ��[�̒ʏ�n��ɕ����̒n�`���쐬
				{
					var areaId = SfDominionTableManager.Instance.Table.GetMinimumCellIndexArea(dominion.Id, eAreaGroupType.Normal);

					SfAreaTableManager.Instance.Table.AddTerrain(areaId, eExistingTerrain.Plane);
				}
			}

			scene.Invoker.ChangeSceneState(eSceneState.CreateKingdomInWorld);
		}
	}

}
