using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UnityEngine.Events;
using UniRx;

namespace sfproj
{
    public class DominionScrollView : WindowBase, IEnhancedScrollerDelegate
    {
        // セルサイズ(単位ベクトル)
        static private float CELL_SIZE = 0.0f;
        // セルサイズ(幅高さ)
        static public Vector2 CellSize = Vector2.zero;
        // マスクセルサイズ(幅高さ)
        static public Vector2 MaskCellSize = Vector2.zero;
        // 選択画像サイズ(幅高さ)
        static public Vector2 SelectedImageSize = Vector2.zero;

        [SerializeField]
        private EnhancedScroller m_scroller = null;

        [SerializeField]
        private EnhancedScrollerCellView m_cell = null;

        // データリスト
        private List<DominionScrollCellData> m_dataList = new List<DominionScrollCellData>();

        // 現在選択中のセルデータ
        private DominionScrollCellData m_selectedCellData = null;

        protected override void Start()
        {
            base.Start();
            
            // 画面高さを取得
            float height = Screen.height;

            // 画面高さの３分の１のサイズを取得
            float heightOneThird = height * 0.3333f;

            // スクロールビューの高さをそのサイズに設定
            var rectTransform = gameObject.GetComponent<RectTransform>();
            var size = rectTransform.sizeDelta;
            size.y = heightOneThird;
            rectTransform.sizeDelta = size;

            // 画面高さの３分の２の位置を取得
            //float posTwoThird = height * 0.6666f;

            // その位置にスクロールビューの高さを設定
            var pos = rectTransform.localPosition;
            pos.y = -heightOneThird;
            rectTransform.localPosition = pos;

            // セルサイズを計算。高さの９０％
            CELL_SIZE = heightOneThird * 0.9f;

            CellSize = new Vector2(heightOneThird * 0.9f, heightOneThird * 0.9f);

            MaskCellSize = new Vector2(heightOneThird * 0.65f, heightOneThird * 0.65f);

            SelectedImageSize = new Vector2(heightOneThird * 0.2f, heightOneThird * 0.2f);
        }

        /// <summary>
        /// 領域 ID からデータリストを作成
        /// </summary>
        /// <param name="dominionId"></param>
        private void CreateData(SfDominionRecord dominionRecord) {

            m_dataList.Clear();

            // 領域にある 地域 ID を取得
            List<uint> areaIdList = dominionRecord.AreaIdList;

            // 地域 ID リストから地域レコードを取得しセルデータを作成していく
            foreach(uint areaId in areaIdList) {

                var areaRecord = SfAreaRecordTableManager.Instance.Get(areaId);
                if (areaRecord == null)
                    continue;

                m_dataList.Add(new DominionScrollCellData(areaRecord));
            }
        }


        // 開示処理
        public bool Open(SfDominionRecord dominionRecord, UnityAction opened = null)
        {
            gameObject.SetActive(true);

            if (tweenCtrl_ != null)
            {
                tweenCtrl_.Play("Open", () => {
                    opened?.Invoke();
                });
            }
            else
            {
                opened?.Invoke();
            }

            CreateData(dominionRecord);

            m_scroller.Delegate = this;

            m_scroller.ReloadData();

            return true;
        }

        // 閉じる際の処理
        public override void Close(UnityAction closed = null)
        {
            if (tweenCtrl_ != null)
            {
                tweenCtrl_.Play("Close", () => {
                    closed?.Invoke();
                    gameObject.SetActive(false);
                });
            }
            else
            {
                closed?.Invoke();
                gameObject.SetActive(false);
            }
        }

        // スクロール時処理
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cell = scroller.GetCellView(m_cell) as DominionScrollCell;

            // セルの名前を地域の名前にしたいので SetData 内部で name に対して地域名を設定する
            //cell.name = m_dataList[dataIndex].Identifier + "_Cell";
            cell.RectTransform.sizeDelta = CellSize;

            cell.SetData(dataIndex, m_dataList[dataIndex]);

            cell.Selected = OnSelected;

            return cell;
        }

        // セルサイズ
        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return CELL_SIZE;
        }

        // セルの数
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return m_dataList.Count;
        }

        /// <summary>
        /// セル選択時の処理
        /// </summary>
        /// <param name="cell"></param>
        public void OnSelected(DominionScrollCell cell)
        {
            // 選択しているものをタッチした場合はフォーカス解除
            bool isFocus = true;

            if (ReferenceEquals(m_selectedCellData, cell.Data))
                isFocus = false;

            foreach (var data in m_dataList)
                data.IsSelected = false;

            if (isFocus == true)
            {
                m_selectedCellData = cell.Data;

                cell.Data.IsSelected = true;

                // 地域ウィンドウ開示
            }
            else
            {
                m_selectedCellData = null;

                cell.Data.IsSelected = false;

                // 地域ウィンドウを開示していたら閉じる
            }

            foreach (var data in m_dataList)
                cell.SetSelected(data.IsSelected);

            m_scroller.ReloadData(m_scroller.ScrollRect.horizontalNormalizedPosition);
        }
    }

#if false
    
    /// <summary>
    /// このクラスは調整が必要
    /// 別のセルをタップした際に開いた状態でセルだけを入れ替えるという事をしないといけない
    /// 現状だと開いている判定になるためセルの再設定ができない
    /// 
    /// デバッグみたいに初めから画面上に配置しておいて表示非表示で対応する方が良いかもしれない
    /// </summary>
    public class PopupDominionScrollViewController : Singleton<PopupDominionScrollViewController>
    {
        private DominionScrollView Window { get; set; } = null;

        public void Open(SfDominionRecord dominionRecord)
        {
            if (Window != null && Window.IsOpen == true)
                return;

            var prefab = AssetManager.Instance.Get<GameObject>("DominionScrollView");

            var obj = GameObject.Instantiate(prefab, CanvasManager.Instance.Current.transform);

            Window = obj.GetComponent<DominionScrollView>();

            Window.CloseBtn.Btn.OnClickAsObservable().Subscribe(_ => Close());

            Window.Open(dominionRecord);
        }

        public void Close()
        {
            if (Window == null)
                return;

            if (Window.IsOpen == false)
                return;

            GameObject.Destroy(Window.gameObject);

            Window = null;
        }
    }
#endif
}
