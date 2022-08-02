using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
	/// <summary>
	/// シーンコマンド
	/// </summary>
	public abstract class SfSceneCommandBase<Ty>
	{
		protected int m_phaseNo = 0;

		public abstract void Update(Ty scene);
	}

	public abstract class SfInMatchSceneCommandBase : SfSceneCommandBase<SfInMatchGameSceneController>
	{

	}
}