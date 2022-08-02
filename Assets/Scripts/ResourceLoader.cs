using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace sfproj
{
	/// <summary>
	/// リソース読み込み
	/// 読み込んだリソースの破棄はまだ未実装
	/// </summary>
	public abstract class ResourceLoader
	{
		/// <summary>
		/// リソースローディング
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
	/// テクスチャ2D 用読み込み
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

			// リソース読み込み
			await loader.OnResourceLoading(resourceFileNameList);

			// 全てのリソースを読み込んだかチェック
			while (true)
			{
				if (loader.CheckCompleteResourceLoading())
					break;
			}

			// 読み込み数をリセット
			loader.ResetLoadingCount();
		}
	}
}