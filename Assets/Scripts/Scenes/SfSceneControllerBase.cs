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
		// �O���b�h
		protected TerrainGridSystem tgs;
		public TerrainGridSystem TGS => tgs;

		// �f�o�b�O�p���b�Z�[�W
		protected GUIStyle labelStyle;
		protected StringBuilder sb = new StringBuilder();
		protected string message = "";

		// true...����������
		protected bool m_isInit = false;

		/// <summary>
		/// ���\�[�X�t�@�C�������X�g
		/// </summary>
		[SerializeField]
		protected List<string> m_resourceNameList = new List<string>();
		public List<string> ResourceNameList => m_resourceNameList;



		/// <summary>
		/// �f�o�b�O�p����������
		/// </summary>
		protected void DebugInitilize() {
			// setup GUI styles
			labelStyle = new GUIStyle();
			labelStyle.alignment = TextAnchor.UpperLeft;
			labelStyle.normal.textColor = Color.black;
		}
	}
}