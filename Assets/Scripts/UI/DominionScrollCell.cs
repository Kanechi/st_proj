using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UniRx;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;

namespace sfproj
{
    /// <summary>
    /// 領域スクロールビューのセル(地域セル)
    /// タッチすることで地域ウィンドウを上部に表示する
    /// </summary>
    public class DominionScrollCell : EnhancedScrollerCellView
    {
        private RectTransform m_rectTransform;
        public RectTransform RectTransform => m_rectTransform != null ? m_rectTransform : m_rectTransform = GetComponent<RectTransform>();

        /// <summary>
        /// 背景
        /// </summary>
        [SerializeField]
        public Image m_bg = null;

        RectTransform m_bgRect = null;
        RectTransform BgRect => m_bgRect != null ? m_bgRect : m_bgRect = m_bg.gameObject.GetComponent<RectTransform>();

        /// <summary>
        /// マスク画像
        /// </summary>
        [SerializeField]
        public Image m_maskBg = null;

        RectTransform m_maskRect = null;
        RectTransform MaskRect => m_maskRect != null ? m_maskRect : m_maskRect = m_maskBg.gameObject.GetComponent<RectTransform>();

        /// <summary>
        /// 地域画像(町、城、遺跡、洞窟)
        /// </summary>
        [SerializeField]
        public Image m_areaImage = null;

        RectTransform m_selectedImageRect = null;
        RectTransform SelectedImageRect => m_selectedImageRect != null ? m_selectedImageRect : m_selectedImageRect = m_selectedImage.gameObject.GetComponent<RectTransform>();

        /// <summary>
        /// 地域ウィンドウ開示ボタン
        /// </summary>
        [SerializeField]
        public Button m_openAreaWindowBtn = null;

        /// <summary>
        /// フォーカス選択中画像
        /// </summary>
        [SerializeField]
        public Image m_selectedImage = null;

        /// <summary>
        /// 現在設定されている領域セルデータ
        /// </summary>
        public DominionScrollCellData Data { get; set; } = null;

        // 手札セルビューにある手札をタッチした際の処理
        public UnityAction<DominionScrollCell> Selected { get; set; } = null;

        public int DataIndex { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            m_openAreaWindowBtn.OnClickAsObservable().Subscribe(_ => OnClicked());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetData(int dataIndex, DominionScrollCellData data)
        {
            DataIndex = dataIndex;
            Data = data;
            Data.Cell = this;

            // 名称設定
            name = data.m_areaRecord.m_name;

            // 背景サイズの設定
            BgRect.sizeDelta = DominionScrollView.CellSize;

            // マスク画像のサイズを設定
            MaskRect.sizeDelta = DominionScrollView.MaskCellSize;

            // 選択画像のサイズを選択
            SelectedImageRect.sizeDelta = DominionScrollView.SelectedImageSize;

            // 背景画像の設定
            m_bg.sprite = Data.m_areaBgImageSprite;

            // 画像の設定
            m_areaImage.sprite = Data.m_areaImageSprite;

            SetSelected(Data.IsSelected);
        }

        public void SetSelected(bool select)
        {
            m_selectedImage.gameObject.SetActive(select);
        }

        /// <summary>
        /// クリックした際の処理
        /// フォーカス処理のみ
        /// 選択した状態で再度タッチしても何も起こらない
        /// </summary>
        private void OnClicked()
        {
            Selected?.Invoke(this);
        }
    }
}