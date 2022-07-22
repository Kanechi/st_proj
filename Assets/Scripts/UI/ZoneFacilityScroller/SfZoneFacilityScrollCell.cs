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
    /// 建設施設選択用スクロールセル
    /// </summary>
    public class SfZoneFacilityScrollCell : EnhancedScrollerCellView
    {
        static private float BaseCellSize = 164.0f;

        private RectTransform m_rectTransform;
        public RectTransform RectTransform => m_rectTransform != null ? m_rectTransform : m_rectTransform = GetComponent<RectTransform>();


        /// <summary>
        /// 区域施設画像
        /// </summary>
        [SerializeField]
        private Image m_zoneFacilityImage = null;

        /// <summary>
        /// 背景画像
        /// </summary>
        [SerializeField]
        private Image m_selectedFrameImage = null;

        /// <summary>
        /// 背景ボタン
        /// </summary>
        [SerializeField]
        private Button m_bgFrameBtn = null;


        /// <summary>
        /// Build ボタン
        /// </summary>
        [SerializeField]
        private Button m_buildButton = null;


        /// <summary>
        /// 購入不可画像
        /// </summary>
        [SerializeField]
        private Image m_notBuyImage = null;

        /// <summary>
        /// Expantion ボタン
        /// </summary>
        [SerializeField]
        private Button m_expantionButton = null;

        public SfZoneFacilityScrollCellData Data { get; set; }

        public UnityAction<SfZoneFacilityScrollCell> Selected { get; set; } = null;

        // Start is called before the first frame update
        void Start()
        {
            m_bgFrameBtn.OnClickAsObservable().Subscribe(_ => OnClicked());
            m_buildButton.OnClickAsObservable().Subscribe(_ => OnBuildBtn());
            m_expantionButton.OnClickAsObservable().Subscribe(_ => OnExpantionBtn());
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// ボタン表示チェック
        /// </summary>
        private void CheckBtnDisp()
        {


        }

        public void SetData(SfZoneFacilityScrollCellData data)
        {
            Data = data;
            Data.Cell = this;

            m_zoneFacilityImage.sprite = Data.m_facilitySprite;

            SetSelected(false);

            CheckBtnDisp();
        }

        public void SetSelected(bool selected) {
            m_selectedFrameImage.gameObject.SetActive(selected);
        }

        public void OnClicked()
        {
            Selected?.Invoke(this);
        }

        /// <summary>
        /// 建設ボタンを押した際の処理
        /// </summary>
        private void OnBuildBtn() { 
        
        }

        /// <summary>
        /// 拡張ボタンを押した際の処理
        /// </summary>
        private void OnExpantionBtn() { 
        
        }


    }
}