using UnityEngine;
using System.Text;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using System.Linq;
using Cysharp.Threading.Tasks;

using sfproj;

namespace TGS {

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
	public class Tex2DResourceLoader : ResourceLoader {

		public override async UniTask OnResourceLoading(List<string> resourceFileNameList)
		{
			foreach (string n in resourceFileNameList)
			{
				await LoadResource<Texture2D>(n);
			}
		}

		static public async UniTask Loading(List<string> resourceFileNameList) {

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



	public class Demo2 : SerializedMonoBehaviour {

		TerrainGridSystem tgs;
		GUIStyle labelStyle;
		StringBuilder sb = new StringBuilder();
		string message = "";

		private bool m_isInit = false;

		[SerializeField]
		protected List<string> m_resourceNameList = new List<string>();
		public List<string> ResourceNameList => m_resourceNameList;

		private async UniTaskVoid Start () {

			// setup GUI styles
			labelStyle = new GUIStyle();
			labelStyle.alignment = TextAnchor.UpperLeft;
			labelStyle.normal.textColor = Color.black;

			// TGS の初期化
			InitializeTgs();

			// リソース読み込み
			await Tex2DResourceLoader.Loading(m_resourceNameList);

			m_isInit = true;
		}

		/// <summary>
		/// TGS の初期化
		/// </summary>
		private void InitializeTgs() {

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
		}

		void OnCellMouseUp(TerrainGridSystem grid, int cellIndex, int buttonIndex) {
		}

		void OnCellClick(TerrainGridSystem grid, int cellIndex, int buttonIndex) {
		}

		void OnTerritoryMouseDown(TerrainGridSystem sender, int territoryIndex, int buttonIndex) { 
		}

		void OnTerritoryMouseUp(TerrainGridSystem sender, int territoryIndex, int buttonIndex) { 
		}

		void OnTerritoryClick(TerrainGridSystem sender, int territoryIndex, int buttonIndex){
		}

		// Parameters to pass through to our new method
		[SerializeField] 
		private int resolution = 10;
		[SerializeField] 
		private float padding = 0.1f;
		[SerializeField] 
		private float offset = 0.0f;

		[SerializeField]
		private List<List<BoxCollider>> currentBoxCollider = new List<List<BoxCollider>>();

		public enum eScenePhase {
			None = -1,
			SettingCellColor,
			CreateTerritory,
			CreateKingdom,
		}
		public eScenePhase m_scenePhase = 0;

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
			// タッチしたテリトリを非表示
			tgs.TerritorySetVisible(territoryIndex, false);
		}

		/// <summary>
		/// セルの初期化
		/// コリダーに触れている領域のセルをすべて非表示にする
		/// </summary>
		/// <param name="collider"></param>
		public void InitCells(BoxCollider collider) {

			// コリダーにふれているセルのリストを取得
			List<int> cellsUnderBoxCollider = new List<int>();
			tgs.CellGetInArea(collider, cellsUnderBoxCollider, resolution, padding, offset);

			// セルが属しているテリトリのセルをすべて非表示にしていく
			if (cellsUnderBoxCollider != null) {
				for (int k = 0; k < cellsUnderBoxCollider.Count; k++) {
					var cell = tgs.cells[cellsUnderBoxCollider[k]];
					DeleteTerritory(cell.territoryIndex);
				}
			}
		}

		/// <summary>
		/// 領域の作成
		/// SfDominion を作成
		/// テリトリを SfDominion に設定して管理
		/// SfArea を作成
		/// SfArea を SfDominion に紐づけ
		/// </summary>
		public void CreateWorld() {

			// 表示しているテリトリリストを取得
			List<Territory> dispTerritoryList = tgs.territories.Where(t => t.visible == true).ToList();

			// 表示されているテリトリの数
			int dispTerritoryCount = dispTerritoryList.Count;

			// 表示されているテリトリの数だけ領域を作成
			for (int i = 0; i < dispTerritoryCount; ++i) {

				// 領域を作成しテリトリリストのテリトリインデックスを生成した領域に紐づけ
				var dominionRecord = SfDominionFactoryManager.Instance.Create(tgs.TerritoryGetIndex(dispTerritoryList[i]));

				// 生成した領域を領域管理にとりつけ
				SfDominionManager.Instance.DominionRecordList.Add(dominionRecord);
			}

			// 領域数
			int dominionCount = SfDominionManager.Instance.DominionRecordList.Count;

			// 生成した領域数分地域をランダムに生成
			for (int i = 0; i < dominionCount; ++i)
			{
				// 領域を取得
				var dominion = SfDominionManager.Instance.DominionRecordList[i];

				// ランダムな数を設定して地域を生成(最低値は設定可能で１以下は無し、最大値は設定可能)
				int areaCount = Random.Range(ConfigController.Instance.MinAreaValue, ConfigController.Instance.MaxAreaValue + 1);

				for (int j = 0; j < areaCount; ++j)
				{
					// 地域を生成

					// 生成した地域を領域に設定していく
				}
			}



		}

		/// <summary>
		/// 国の作成
		/// SfDominion からランダムに国を選定
		/// </summary>
		public void CreateKingdom() {

			// 表示しているテリトリのリストを取得
			var dispTerritoryList = tgs.territories.Where(t => t.visible == true).ToList();

			// 表示されているテリトリの数
			int dispTerritoryCount = dispTerritoryList.Count;

			// 最大王国数だけランダムに領域を作成
			for (int i = 0; i < ConfigController.Instance.KingdomCount; ++i)
			{
				// ランダムに領土を領域に選定
				Territory kingdom = dispTerritoryList[Random.Range(0, dispTerritoryCount)];

				// 自身の王国のみ設定した色を使用
				Color color = i == 0 ? ConfigController.Instance.KingdomColor : Random.ColorHSV();
				color.a = 0.4f;

				// 王国の色を設定
				foreach (var cell in kingdom.cells)
					tgs.CellSetColor(cell, color);

				// 王国にした領土を territoryList から外す
				dispTerritoryList.Remove(kingdom);
				// 領域数を更新
				dispTerritoryCount = dispTerritoryList.Count;

				// 王国データを設定
				

				// 王国データにテリトリーインデックスを設定
			}
		}

		// 合計時間
		float m_totalTime = 0.0f;

		/// <summary>
		/// 更新処理
		/// </summary>
        private void Update() {

			if (m_isInit == false)
				return;

			switch (m_scenePhase) {
				case eScenePhase.SettingCellColor:
					{
						var list = tgs.cells.ToList();

						foreach (var cell in list)
						{
							tgs.CellSetColor(cell, ConfigController.Instance.LandColor);
						}

						m_scenePhase = eScenePhase.CreateTerritory;
					}
					break;
				case eScenePhase.CreateTerritory:
					{
						foreach (var collider in currentBoxCollider[(int)ConfigController.Instance.LandType])
						{
							InitCells(collider);
						}

						// 一回だけ領域の作成を行う

						m_totalTime += Time.deltaTime;
						if (m_totalTime >= 0.1f)
						{
							m_scenePhase = eScenePhase.CreateKingdom;
						}
					}
					break;
				case eScenePhase.CreateKingdom:
					{
						CreateKingdom();
						m_scenePhase = eScenePhase.None;
					}
					break;
				default:
					break;
			}
		}








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
	}
}
