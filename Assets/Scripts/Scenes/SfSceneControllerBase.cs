using UnityEngine;
using System.Text;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using System.Linq;
using Cysharp.Threading.Tasks;
using TGS;

namespace sfproj
{
    public class SfSceneControllerBase : SerializedMonoBehaviour
    {
		// グリッド
		protected TerrainGridSystem tgs;
		public TerrainGridSystem TGS => tgs;

		// デバッグ用メッセージ
		protected GUIStyle labelStyle;
		protected StringBuilder sb = new StringBuilder();
		protected string message = "";

		// true...初期化完了
		protected bool m_isInit = false;

		/// <summary>
		/// リソースファイル名リスト
		/// </summary>
		[SerializeField]
		protected List<string> m_resourceNameList = new List<string>();
		public List<string> ResourceNameList => m_resourceNameList;



		/// <summary>
		/// デバッグ用初期化処理
		/// </summary>
		protected void DebugInitilize() {
			// setup GUI styles
			labelStyle = new GUIStyle();
			labelStyle.alignment = TextAnchor.UpperLeft;
			labelStyle.normal.textColor = Color.black;
		}
	}
}