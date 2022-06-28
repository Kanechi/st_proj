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
				await LoadResource<Sprite>(n);
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

		/// <summary>
		/// デバッグ用名前リストテンプレート
		/// </summary>
		[SerializeField]
		private NameListObject m_nameListObj;

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


			NameCreateDirector director = new NameCreateDirector(new NameCreateNatural());

			director.Construct(10, m_nameListObj.Get(NameListObject.eCreateNameCategory.CzechF));
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

		/// <summary>
		/// 領土スクロールビュー
		/// </summary>
		[SerializeField]
		private DominionScrollView m_dominionScrollView = null;

		/// <summary>
		/// 王国ウィンドウ
		///		国旗
		///		王国名
		///		国民数
		///		
		///		資源もここで表示するかどうか
		/// </summary>

		/// <summary>
		/// 領土ウィンドウ
		/// </summary>

		/// <summary>
		/// 地域ウィンドウ
		/// </summary>

		/// <summary>
		/// サブペインウィンドウ
		/// </summary>


		/// <summary>
		/// 領域スクロールを開示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="territoryIndex"></param>
		/// <param name="buttonIndex"></param>
		void OnTerritoryClick(TerrainGridSystem sender, int territoryIndex, int buttonIndex){

			// テリトリインデックスから領域レコードを取得
			var dominionRecord = SfDominionRecordTableManager.Instance.GetAtTerritoryIndex(territoryIndex);

			if (dominionRecord == null)
				return;

			m_dominionScrollView.Open(dominionRecord);
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
			// セルに色を設定
			SettingCellColor,
			// テリトリを作成
			CreateTerritory,

			// 世界に領域を作成
			CreateDominionInWorld,
			// 世界に地域を作成
			CreateAreaInWorld,
			// 世界に王国を作成
			CreateKingdomInWorld,
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
		/// 世界における領域の作成
		/// SfDominion を作成
		/// テリトリを SfDominion に設定して管理
		/// </summary>
		public void CreateDominionInWorld() {

			// 表示しているテリトリリストを取得
			List<Territory> dispTerritoryList = tgs.territories.Where(t => t.visible == true).ToList();

			// 表示されているテリトリの数
			int dispTerritoryCount = dispTerritoryList.Count;

			// 表示されているテリトリの数だけ領域を作成
			for (int i = 0; i < dispTerritoryCount; ++i) {

				// 領域を作成しテリトリリストのテリトリインデックスを生成した領域に紐づけ
				var dominionRecord = SfDominionFactoryManager.Instance.Create(tgs.TerritoryGetIndex(dispTerritoryList[i]));

				// 生成した領域を領域管理にとりつけ
				SfDominionRecordTableManager.Instance.Regist(dominionRecord);
			}
		}

		/// <summary>
		/// 世界における地域を作成
		/// SfArea を作成
		/// SfArea を SfDominion に紐づけ
		/// すべて未開拓状態で作成
		/// </summary>
		private void CreateAreaInWorld() {

			// 領域数
			int dominionRecordCount = SfDominionRecordTableManager.Instance.RecordList.Count;

			// 生成した領域数分地域をランダムに生成
			for (int i = 0; i < dominionRecordCount; ++i)
			{
				// 領域を取得
				var dominionRecord = SfDominionRecordTableManager.Instance.RecordList[i];

				// ランダムな数を設定して地域を生成(最低値は設定可能で１以下は無し、最大値は設定可能)
				int areaIncDec = Random.Range(0, ConfigController.Instance.AreaIncDecValue + 1);

				// 領域のテリトリにあるセルの数を取得
				var cellCount = tgs.territories[dominionRecord.m_territoryIndex].cells.Count;

				int areaCount = cellCount <= areaIncDec ? cellCount : cellCount + areaIncDec;

				for (int cellNo = 0; cellNo < areaCount; ++cellNo)
				{
					// 地域を生成
					var areaRecord = SfAreaFactoryManager.Instance.RandomCreate(cellNo, dominionRecord.Id);

					SfAreaRecordTableManager.Instance.Regist(areaRecord);

					// 生成した地域を領域に設定していく
					dominionRecord.AreaIdList.Add(areaRecord.Id);
				}
			}
		}

		/// <summary>
		/// 世界に国を作成
		/// SfDominion からランダムに国を選定
		/// 選定された SfDominion の地域を開拓状態にして初期設定を行っていく
		/// </summary>
		public void CreateKingdomInWorld() {

			// 表示しているテリトリのリストを取得
			var dispTerritoryList = tgs.territories.Where(t => t.visible == true).ToList();

			// テリトリから領域を取得
			var dominionList = new List<SfDominionRecord>();
			foreach (Territory t in dispTerritoryList)
			{
				dominionList.Add(SfDominionRecordTableManager.Instance.GetAtTerritoryIndex(tgs.TerritoryGetIndex(t)));
			}

			// 表示されているテリトリの数
			int dispTerritoryCount = dispTerritoryList.Count;

			// 最大王国数だけ王国を作成
			for (int i = 0; i < ConfigController.Instance.KingdomCount; ++i)
			{
			}

			// 最大王国数だけランダムに領域を作成
			for (int i = 0; i < ConfigController.Instance.KingdomCount; ++i)
			{
				// ランダムに領土を領域に選定
				Territory kingdomTerritory = dispTerritoryList[Random.Range(0, dispTerritoryCount)];

				// 自身の王国のみ設定した色を使用
				Color color = i == 0 ? ConfigController.Instance.KingdomColor : Random.ColorHSV();
				color.a = 0.4f;

				// 王国の色を設定
				foreach (var cell in kingdomTerritory.cells)
					tgs.CellSetColor(cell, color);

				// 王国にした領土を territoryList から外す
				dispTerritoryList.Remove(kingdomTerritory);
				// 領域数を更新
				dispTerritoryCount = dispTerritoryList.Count;
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
							m_scenePhase = eScenePhase.CreateDominionInWorld;
						}
					}
					break;

				case eScenePhase.CreateDominionInWorld:
					{
						CreateDominionInWorld();
						m_scenePhase = eScenePhase.CreateAreaInWorld;
					}
					break;

				case eScenePhase.CreateAreaInWorld:
					{
						CreateAreaInWorld();
						m_scenePhase = eScenePhase.CreateKingdomInWorld;
					}
					break;

				case eScenePhase.CreateKingdomInWorld:
					{
						CreateKingdomInWorld();
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
