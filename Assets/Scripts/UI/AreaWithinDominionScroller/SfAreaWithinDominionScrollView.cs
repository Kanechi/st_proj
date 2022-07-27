using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UnityEngine.Events;
using UniRx;

namespace sfproj
{
    /// <summary>
    /// 領域内地域スクロールビュー
    /// </summary>
    public class SfAreaWithinDominionScrollView : WindowBase, IEnhancedScrollerDelegate
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
        private EnhancedScrollerCellView m_cellView = null;
        //private EnhancedScrollerCellView m_cell = null;

        // データリスト
        private List<SfAreaWithinDominionScrollCellData> m_dataList = new List<SfAreaWithinDominionScrollCellData>();

        // 現在選択中のセルデータ
        private SfAreaWithinDominionScrollCellData m_selectedCellData = null;

        protected override void Start()
        {
            base.Start();

            closeBtn_.OnClickAsObservable().Subscribe(_ => { Close(); });

            var canvasTransform = SfGameManager.Instance.CurrentCanvas.GetComponent<RectTransform>();

            // 画面高さを取得
            float height = canvasTransform.sizeDelta.y; //Screen.height;

            // 画面高さの４分の１のサイズを取得
            float heightOneFour = height * 0.25f;

            // スクロールビューの高さをそのサイズに設定
            var rectTransform = gameObject.GetComponent<RectTransform>();
            var size = rectTransform.sizeDelta;
            size.y = heightOneFour;
            rectTransform.sizeDelta = size;

            // 画面高さの３分の２の位置を取得
            //float posTwoThird = height * 0.6666f;

            // その位置にスクロールビューの高さを設定
            var pos = rectTransform.localPosition;
            pos.y = -(heightOneFour + (heightOneFour * 0.5f));
            rectTransform.localPosition = pos;

            // セルサイズを計算。高さの９０％
            CELL_SIZE = heightOneFour * 0.9f;

            CellSize = new Vector2(heightOneFour * 0.9f, heightOneFour * 0.9f);

            MaskCellSize = new Vector2(heightOneFour * 0.65f, heightOneFour * 0.65f);

            SelectedImageSize = new Vector2(heightOneFour * 0.2f, heightOneFour * 0.2f);
        }

        /// <summary>
        /// 領域をタッチした際アクティブ化されての同じフレームでは ReloadData() が反映されないので
        /// 次フレーム以降で ReloadData しなければならない。そのためのイベントで、
        /// 領域スクロールビュー開示時イベントに処理を代入し処理完了後に null を代入する
        /// </summary>
        private UnityAction m_openedReloadDataEvent = null;

        private void Update()
        {
            m_openedReloadDataEvent?.Invoke();
        }



        /// <summary>
        /// 領域 ID からデータリストを作成
        /// </summary>
        /// <param name="dominionId"></param>
        private void CreateData(SfDominion dominionRecord) {

            m_dataList.Clear();

            // 領域にある 地域 ID を取得
            List<uint> areaIdList = dominionRecord.AreaIdList;

            // 地域 ID リストから地域レコードを取得しセルデータを作成していく
            foreach(uint areaId in areaIdList) {

                var areaRecord = SfAreaTableManager.Instance.Table.Get(areaId);
                if (areaRecord == null)
                    continue;

                m_dataList.Add(new SfAreaWithinDominionScrollCellData(areaRecord));
            }
        }


        // 開示処理
        public bool Open(SfDominion dominionRecord, UnityAction opened = null)
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

            m_openedReloadDataEvent = OpenedReloadData;

            return true;
        }

        private void OpenedReloadData() {
            m_scroller.Delegate = this;

            m_scroller.ReloadData();

            m_openedReloadDataEvent = null;
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
#if true
            var cell = scroller.GetCellView(m_cellView) as SfAreaWithinDominionScrollCell;

            // セルの名前を地域の名前にしたいので SetData 内部で name に対して地域名を設定する
            //cell.name = m_dataList[dataIndex].Identifier + "_Cell";
            cell.RectTransform.sizeDelta = CellSize;

            cell.SetData(dataIndex, m_dataList[dataIndex]);

            cell.Selected = OnSelected;

            return cell;
#else
            return null;
#endif
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
        public void OnSelected(SfAreaWithinDominionScrollCell cell)
        {
            // 選択しているものをタッチした場合はフォーカス解除
            bool isFocus = true;

#if false
            if (ReferenceEquals(m_selectedCellData, cell.Data))
                isFocus = false;
#endif
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
