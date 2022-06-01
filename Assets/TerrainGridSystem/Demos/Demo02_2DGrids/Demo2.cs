using UnityEngine;
using System.Text;
using System.Collections.Generic;
using Sirenix.OdinInspector;

using stproj;

namespace TGS {
	
	public class Demo2 : SerializedMonoBehaviour {

		TerrainGridSystem tgs;
		GUIStyle labelStyle;
		StringBuilder sb = new StringBuilder();
		string message = "";

		private bool m_isInit = false;
		private bool m_isInitCells = false;

		void Start () {
			tgs = TerrainGridSystem.instance;

			tgs.seed = Random.Range(0, 10001);

			// setup GUI styles
			labelStyle = new GUIStyle ();
			labelStyle.alignment = TextAnchor.UpperLeft;
			labelStyle.normal.textColor = Color.black;

			// Events
			// OnCellMouseDown occurs when user presses the mouse button on a cell
			tgs.OnCellMouseDown += OnCellMouseDown;
			// OnCellMouseUp occurs when user releases the mouse button on a cell even after a drag
			tgs.OnCellMouseUp += OnCellMouseUp;
			// OnCellClick occurs when user presses and releases the mouse button as in a normal click
			tgs.OnCellClick += OnCellClick;

			// Draw disputing frontier between territories 0 and 3 in yellow
			tgs.TerritoryDrawFrontier(0, 3, null, Color.yellow);

#if true
			tgs.OnTerritoryMouseDown += OnTerritoryMouseDown;
			tgs.OnTerritoryMouseUp += OnTerritoryMouseUp;
			tgs.OnTerritoryClick += OnTerritoryClick;
#endif


			// 開始時に端に隣接しているセルを列挙
			Cell cell = tgs.CellGetAtPosition(0, 0);
			AddMessage("cell index : " + cell.index);

			//tgs.();

			m_isInit = true;

		}

        void OnCellMouseDown (TerrainGridSystem grid, int cellIndex, int buttonIndex) {
			//AddMessage("cell index : " + cellIndex);
		}

		void OnCellMouseUp(TerrainGridSystem grid, int cellIndex, int buttonIndex) {
			//AddMessage("cell index : " + cellIndex);
		}

		void OnCellClick (TerrainGridSystem grid, int cellIndex, int buttonIndex) {

			int row = tgs.CellGetRow(cellIndex);
			int column = tgs.CellGetColumn(cellIndex);
			
			AddMessage("cell index : " + cellIndex);
			AddMessage("column row : " + column + ", " + row );

		}


		void OnTerritoryMouseDown(TerrainGridSystem sender, int territoryIndex, int buttonIndex) { 
		}

		void OnTerritoryMouseUp(TerrainGridSystem sender, int territoryIndex, int buttonIndex) { 
		}

		void OnTerritoryClick(TerrainGridSystem sender, int territoryIndex, int buttonIndex)
		{
#if false
			// タッチしたテリトリの色を変更
			foreach (var cell in sender.territories[territoryIndex].cells)
			{
				tgs.CellSetColor(cell.index, Color.red);
				tgs.CellSetVisible(cell.index, false);
			}
			tgs.TerritorySetVisible(territoryIndex, false);
#endif

			//DeleteTerritory(territoryIndex);
		}

		/// <summary>
		/// 土地を非表示
		/// 領域と領域内のセルの非表示
		/// 
		/// マップを作成する際のマップ端と海を作る際に利用
		/// </summary>
		/// <param name="territoryIndex"></param>
		static public void DeleteTerritory(int territoryIndex) {

			var tgs = TerrainGridSystem.instance;

			foreach (var cell in tgs.territories[territoryIndex].cells)
			{
				// タッチしたテリトリの色を変更
				//tgs.CellSetColor(cell.index, Color.red);

				// タッチしたテリトリのセルを非表示
				tgs.CellSetVisible(cell.index, false);
			}
			// タッチしたテリトリを非表示
			tgs.TerritorySetVisible(territoryIndex, false);
		}

		/// <summary>
		/// 土地の色を変更
		/// </summary>
		/// <param name="territoryIndex"></param>
		/// <param name="color"></param>
		static public void ChangeColorTerritory(int territoryIndex, Color color) {
			var tgs = TerrainGridSystem.instance;

			foreach (var cell in tgs.territories[territoryIndex].cells)
			{
				// テリトリの色を変更
				tgs.CellSetColor(cell.index, color);
			}
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

		public void InitCells(BoxCollider collider) {

			// Initialize Cell list
			List<int> cellsUnderBoxCollider = new List<int>();
			// Use CellGetInArea with box collider to get cells under it
			tgs.CellGetInArea(collider, cellsUnderBoxCollider, resolution, padding, offset);

			// If we have more than one cell
			if (cellsUnderBoxCollider != null) {
				// Color the cells under the box colllider
				for (int k = 0; k < cellsUnderBoxCollider.Count; k++) {
					//tgs.CellSetColor(cellsUnderBoxCollider[k], Color.red);

					var cell = tgs.cells[cellsUnderBoxCollider[k]];

					Demo2.DeleteTerritory(cell.territoryIndex);
				}
			}
		}


		float m_totalTime = 0.0f;

        private void Update() {

			if (m_isInit == false)
				return;

			if (m_isInitCells == true)
				return;

			foreach (var collider in currentBoxCollider[(int)ConfigController.Instance.LandType]) {
				InitCells(collider);
			}

			m_totalTime += Time.deltaTime;
			if (m_totalTime >= 1.0f) {
				m_isInitCells = true;
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
