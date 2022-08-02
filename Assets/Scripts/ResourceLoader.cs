using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace sfproj
{
	/// <summary>
	/// ���\�[�X�ǂݍ���
	/// �ǂݍ��񂾃��\�[�X�̔j���͂܂�������
	/// </summary>
	public abstract class ResourceLoader
	{
		/// <summary>
		/// ���\�[�X���[�f�B���O
		/// </summary>
		private int m_loadingCt = 0;
		private int m_maxLoadingCt = 0;

		public async UniTask LoadResource<Ty>(string id) where Ty : UnityEngine.Object
		{
			m_maxLoadingCt++;
			await AssetManager.Instance.LoadAsync<Ty>(id, prefab => m_loadingCt++);
		}

		public bool CheckCompleteResourceLoading() => m_maxLoadingCt == m_loadingCt;

		public void ResetLoadingCount()
		{
			m_loadingCt = 0;
			m_maxLoadingCt = 0;
		}

		public abstract UniTask OnResourceLoading(List<string> resourceFileNameList);
	}

	/// <summary>
	/// �e�N�X�`��2D �p�ǂݍ���
	/// </summary>
	public class Tex2DResourceLoader : ResourceLoader
	{

		public override async UniTask OnResourceLoading(List<string> resourceFileNameList)
		{
			foreach (string n in resourceFileNameList)
			{
				await LoadResource<Sprite>(n);
			}
		}

		static public async UniTask Loading(List<string> resourceFileNameList)
		{

			var loader = new Tex2DResourceLoader();

			// ���\�[�X�ǂݍ���
			await loader.OnResourceLoading(resourceFileNameList);

			// �S�Ẵ��\�[�X��ǂݍ��񂾂��`�F�b�N
			while (true)
			{
				if (loader.CheckCompleteResourceLoading())
					break;
			}

			// �ǂݍ��ݐ������Z�b�g
			loader.ResetLoadingCount();
		}
	}
}