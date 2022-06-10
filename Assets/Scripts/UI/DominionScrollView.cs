using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UnityEngine.Events;

namespace sfproj
{
    public class DominionScrollView : WindowBase, IEnhancedScrollerDelegate
    {
        static private float CELL_SIZE = 250.0f;

        [SerializeField]
        private EnhancedScroller m_scroller = null;

        [SerializeField]
        private EnhancedScrollerCellView m_cell = null;

        protected override void Start()
        {
            base.Start();
            
            // 画面高さを取得
            float height = Screen.height;

            // 画面高さの３分の１のサイズを取得
            float heightOneThird = height * 0.3333f;



            Debug.Log(height + ":" + heightOneThird);

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
        }

        public override bool Open(UnityAction opened = null)
        {
            if (base.Open(opened) == false)
                return false;

            m_scroller.Delegate = this;

            m_scroller.ReloadData();

            return true;
        }

        public override void Close(UnityAction closed = null)
        {
            base.Close(closed);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cell = scroller.GetCellView(m_cell) as DominionScrollCell;



            return cell;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return CELL_SIZE;
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return 0;
        }
    }
}
