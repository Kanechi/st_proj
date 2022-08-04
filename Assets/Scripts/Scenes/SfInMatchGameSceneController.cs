using UnityEngine;
using System.Collections.Generic;
using UniRx;
using System.Linq;
using Cysharp.Threading.Tasks;
using TGS;

namespace sfproj
{
	/// <summary>
	/// シーンの状態
	/// </summary>
	public enum eSceneState : int
	{
		None = -1,

		// TGS のテリトリを初期化
		InitializeTGSTerritory,

		// 世界に領域を作成
		CreateDominionInWorld,
		// 世界に地域を作成
		CreateAreaInWorld,

		// 作成した地域に生産資源を生成して設定

		// 世界に王国を作成
		CreateKingdomInWorld,



		// 通常更新(ゲームプレイ中)
		MainUpdate,
	}
	


	/// <summary>
	/// インマッチシーンコマンドインヴォーカー
	/// </summary>
	public class SfInMatchSceneCommandInvoker
	{
		private Dictionary<eSceneState, SfInMatchSceneCommandBase> m_invoker = null;

		public SfInMatchSceneCommandInvoker(Dictionary<eSceneState, SfInMatchSceneCommandBase> invoker)
		{
			m_invoker = invoker;
		}

		private eSceneState m_sceneState = eSceneState.None;

		public void ChangeSceneState(eSceneState state)
		{
			m_sceneState = state;

			Debug.Log("SceneState : " + state.ToString());
		}

		public void Update(SfInMatchGameSceneController scene)
		{
			if (m_invoker == null)
				return;

			if (m_sceneState == eSceneState.None)
				return;

			m_invoker[m_sceneState]?.Update(scene);
		}
	}





	public class SfeMainUpdateSceneCommand : SfInMatchSceneCommandBase
	{
        public override void Update(SfInMatchGameSceneController scene)
        {
            
        }
    }









	/// <summary>
	/// 対戦ゲームのメインとなるシーン
	/// </summary>
	public class SfInMatchGameSceneController : SfSceneControllerBase
	{
		/// <summary>
		/// デバッグ用名前リストテンプレート
		/// </summary>
		[SerializeField]
		private NameListObject m_nameListObj;

		protected SfInMatchSceneCommandInvoker m_sceneCmdInvoker = null;
		public SfInMatchSceneCommandInvoker Invoker => m_sceneCmdInvoker;

		private async UniTaskVoid Start()
        {

			// インヴォーカーの作成とシーンコマンドの設定
			m_sceneCmdInvoker = new SfInMatchSceneCommandInvoker(new Dictionary<eSceneState, SfInMatchSceneCommandBase> {
				{ eSceneState.InitializeTGSTerritory, new SfInitializeTGSTerritorySceneCommand() },
				{ eSceneState.CreateDominionInWorld, new SfCreateDominionInWorldSceneCommand() },
				{ eSceneState.CreateAreaInWorld, new SfCreateAreaInWorldSceneCommand() },
				{ eSceneState.CreateKingdomInWorld, new SfCreateKingdomInWorldSceneCommand() },
				{ eSceneState.MainUpdate, new SfeMainUpdateSceneCommand() },
			});

			// ゲーム管理の初期化
			SfGameManager.Instance.OnInitialize();

			// setup GUI styles
			DebugInitilize();

			// TGS の初期化
			InitializeTgs();

			// リソース読み込み
			await Tex2DResourceLoader.Loading(m_resourceNameList);

			m_isInit = true;

			// ランダム名を生成
			NameCreateFactoryManager.Instance.Create(10, eNameCreateType.Natural,m_nameListObj.Get(NameListObject.eCreateNameCategory.CzechF));

			Invoker.ChangeSceneState(eSceneState.InitializeTGSTerritory);
		}

		/// <summary>
		/// TGS の初期化
		/// </summary>
		private void InitializeTgs() {

			Debug.Log("InitializeTgs");

			tgs = TerrainGridSystem.instance;

			// シード設定
			//tgs.seed = Random.Range(0, 10001);

			// セルタッチイベント
			tgs.OnCellMouseDown += OnCellMouseDown;
			tgs.OnCellMouseUp += OnCellMouseUp;
			tgs.OnCellClick += OnCellClick;

			// 領域タッチイベント
			tgs.OnTerritoryMouseDown += OnTerritoryMouseDown;
			tgs.OnTerritoryMouseUp += OnTerritoryMouseUp;
			tgs.OnTerritoryClick += OnTerritoryClick;

			// Draw disputing frontier between territories 0 and 3 in yellow
			tgs.TerritoryDrawFrontier(0, 3, null, Color.yellow);
		}

        void OnCellMouseDown(TerrainGridSystem grid, int cellIndex, int buttonIndex) {
			Debug.Log("OnCellMouseDown");
		}

		void OnCellMouseUp(TerrainGridSystem grid, int cellIndex, int buttonIndex) {
			Debug.Log("OnCellMouseUp");
		}

		void OnCellClick(TerrainGridSystem grid, int cellIndex, int buttonIndex) {
			Debug.Log("OnCellClick");
		}

		void OnTerritoryMouseDown(TerrainGridSystem sender, int territoryIndex, int buttonIndex) {

			Debug.Log("OnTerritoryMouseDown");
		}

		void OnTerritoryMouseUp(TerrainGridSystem sender, int territoryIndex, int buttonIndex) {

			Debug.Log("OnTerritoryMouseUp");
		}



		/// <summary>
		/// 領域スクロールを開示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="territoryIndex"></param>
		/// <param name="buttonIndex"></param>
		void OnTerritoryClick(TerrainGridSystem sender, int territoryIndex, int buttonIndex){

			Debug.Log("OnTerritoryClick0");

			// 地域ビューが開いていたらテリトリには触れないようにする
			if (SfGameManager.Instance.AreaInfoView.OpenFlag == true)
				return;


			Debug.Log("OnTerritoryClick1");


			// テリトリインデックスから領域レコードを取得
			var dominionRecord = SfDominionTableManager.Instance.Table.GetAtTerritoryIndex(territoryIndex);

			if (dominionRecord == null)
				return;

			Debug.Log("OnTerritoryClick2");

			SfGameManager.Instance.AreaWithinDominionScrollView.Open(dominionRecord);
		}

		// Parameters to pass through to our new method
		[SerializeField] 
		private int resolution = 10;
		[SerializeField] 
		private float padding = 0.1f;
		[SerializeField] 
		private float offset = 0.0f;

		// コリダーに触れているセル番号のリストを取得
		public List<int> GetUnderBoxColliderCells(BoxCollider collider) {
			// コリダーにふれているセルのリストを取得
			List<int> cellsUnderBoxCollider = new List<int>();
			tgs.CellGetInArea(collider, cellsUnderBoxCollider, resolution, padding, offset);
			return cellsUnderBoxCollider;
		}



		// 大陸分割用のコリジョン
		[SerializeField]
		private List<List<BoxCollider>> currentBoxCollider = new List<List<BoxCollider>>();
		public List<BoxCollider> GetLandDivideCollider(eLandType type) => currentBoxCollider[(int)type];



		/// <summary>
		/// 土地の色を変更
		/// </summary>
		/// <param name="territoryIndex"></param>
		/// <param name="color"></param>
		public void ChangeColorTerritory(int territoryIndex, Color color)
		{
			var tgs = TerrainGridSystem.instance;

			foreach (var cell in tgs.territories[territoryIndex].cells)
			{
				// テリトリの色を変更
				tgs.CellSetColor(cell.index, color);
			}
		}

		/// <summary>
		/// 土地を非表示
		/// 領域と領域内のセルの非表示
		/// 
		/// マップを作成する際のマップ端と海を作る際に利用
		/// </summary>
		/// <param name="territoryIndex"></param>
		public void DeleteTerritory(int territoryIndex)
		{
			foreach (var cell in tgs.territories[territoryIndex].cells)
			{
				// タッチしたテリトリのセルを非表示
				tgs.CellSetVisible(cell.index, false);
			}

			// タッチしたテリトリ

			// タッチしたテリトリを非表示
			tgs.TerritorySetVisible(territoryIndex, false);
		}

		/// <summary>
		/// 更新処理
		/// </summary>
		private void Update()
		{
			if (m_isInit == false)
				return;

			if (m_sceneCmdInvoker == null)
				return;

			m_sceneCmdInvoker.Update(this);
		}



#if false
		void OnGUI () {
			GUI.Label (new Rect (0, 5, Screen.width, 30), "Try changing the grid properties in Inspector. You can click a cell to merge it.", labelStyle);
			GUI.Label(new Rect(0, 35, Screen.width, 60), message, labelStyle);
		}

		void AddMessage(string text) {
			if (sb.Length > 200) sb.Length = 0;
			sb.AppendLine(text);
			message = sb.ToString();
			Debug.Log(text);
        }

		/// <summary>
		/// Merge cell example. This function will make cell1 marge with a random cell from its neighbours.
		/// </summary>
		void MergeCell (Cell cell1) {
			int neighbourCount = cell1.neighbours.Count;
			if (neighbourCount == 0)
				return;
			Cell cell2 = cell1.neighbours [Random.Range (0, neighbourCount)];
			tgs.CellMerge (cell1, cell2);
			tgs.Redraw ();
		}
#endif
	}
}
