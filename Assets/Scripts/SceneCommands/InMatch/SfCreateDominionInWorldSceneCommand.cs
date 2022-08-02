using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TGS;
using UnityEngine;

namespace sfproj {

	/// <summary>
	/// �̈�̍쐬
	/// </summary>
	public class SfCreateDominionInWorldSceneCommand : SfInMatchSceneCommandBase
	{
		public override void Update(SfInMatchGameSceneController scene)
		{
			// �\�����Ă���e���g�����X�g���擾
			List<Territory> dispTerritoryList = scene.TGS.territories.Where(t => t.visible == true).ToList();

			// �\������Ă���e���g���̐�
			int dispTerritoryCount = dispTerritoryList.Count;

			// �\������Ă���e���g���̐������̈���쐬
			for (int i = 0; i < dispTerritoryCount; ++i)
			{

				// �̈���쐬���e���g�����X�g�̃e���g���C���f�b�N�X�𐶐������̈�ɕR�Â�
				var dominionRecord = SfDominionFactoryManager.Instance.Create(scene.TGS.TerritoryGetIndex(dispTerritoryList[i]));

				// ���������̈��̈�Ǘ��ɂƂ��
				SfDominionTableManager.Instance.Table.Regist(dominionRecord);
			}

			scene.Invoker.ChangeSceneState(eSceneState.CreateAreaInWorld);
		}
	}
}
