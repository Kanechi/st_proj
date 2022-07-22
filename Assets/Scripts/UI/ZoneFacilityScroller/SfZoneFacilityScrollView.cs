using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UnityEngine.Events;
using UniRx;

namespace sfproj
{
    public class SfZoneFacilityScrollView : WindowBase, IEnhancedScrollerDelegate
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

        /// <summary>
        /// タッチした区域のデータ
        /// </summary>
        private SfZoneCellData m_touchedZoneCellData = null;

        // データリスト
        private List<SfZoneFacilityScrollCellData> m_dataList = new List<SfZoneFacilityScrollCellData>();

        // 現在選択中のセルデータ
        private SfZoneFacilityScrollCellData m_selectedCellData = null;

        // Start is called before the first frame update
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

        private UnityAction m_openedReloadDataEvent = null;

        private void Update()
        {
            m_openedReloadDataEvent?.Invoke();
        }

        private void CreateData() {

            m_dataList.Clear();

            var list = SfZoneFacilityRecordTable.Instance.RecordList;

            foreach (var record in list)
            {
                m_dataList.Add(new SfZoneFacilityScrollCellData(record, m_touchedZoneCellData));
            }
        }

        public bool Open(SfZoneCellData touchedZoneCellData, UnityAction opened = null)
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

            CreateData();

            m_openedReloadDataEvent = OpenedReloadData;

            return true;
        }


        private void OpenedReloadData()
        {
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
            var cell = scroller.GetCellView(m_cell) as SfZoneFacilityScrollCell;

            // セルの名前を地域の名前にしたいので SetData 内部で name に対して地域名を設定する
            //cell.name = m_dataList[dataIndex].Identifier + "_Cell";
            cell.RectTransform.sizeDelta = CellSize;

            cell.SetData(m_dataList[dataIndex]);

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
        public void OnSelected(SfZoneFacilityScrollCell cell)
        {
            bool isFocus = true;

#if false
            // 選択しているものをタッチした場合はフォーカス解除
            if (ReferenceEquals(m_selectedCellData, cell.Data))
                isFocus = false;
#endif
            foreach (var data in m_dataList)
                data.IsSelected = false;

            if (isFocus == true)
            {
                m_selectedCellData = cell.Data;

                cell.Data.IsSelected = true;

                // 建設施設選択ウィンドウ開示
            }
            else
            {
                m_selectedCellData = null;

                cell.Data.IsSelected = false;

                // 建設施設選択ウィンドウを開示していたら閉じる
            }

            foreach (var data in m_dataList)
                cell.SetSelected(data.IsSelected);

            m_scroller.ReloadData(m_scroller.ScrollRect.horizontalNormalizedPosition);
        }
    }
}