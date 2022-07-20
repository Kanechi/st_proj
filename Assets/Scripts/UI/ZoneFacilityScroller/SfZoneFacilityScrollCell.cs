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
    /// 区域施設スクロールセル
    /// </summary>
    public class SfZoneFacilityScrollCell : EnhancedScrollerCellView
    {
        //static private float BaseCellSize = 320.0f;

        /// <summary>
        /// 区域施設画像
        /// </summary>
        [SerializeField]
        private Image m_zoneFacilityImage = null;

        /// <summary>
        /// 背景画像
        /// </summary>
        [SerializeField]
        private Image m_bgFrameImage = null;

        /// <summary>
        /// 背景ボタン
        /// </summary>
        [SerializeField]
        private Button m_bgFrameBtn = null;

        /// <summary>
        /// 購入不可画像
        /// </summary>
        [SerializeField]
        private Image m_notBuyImage = null;

        /// <summary>
        /// ボタン
        /// 0...Buy     1...Expantion
        /// </summary>
        [SerializeField]
        private List<Button> m_btns = new List<Button>();

        public SfZoneFacilityScrollCellData Data { get; set; }

        public UnityAction<SfZoneFacilityScrollCell> Selected { get; set; } = null;

        // Start is called before the first frame update
        void Start()
        {
            m_btns[0].OnClickAsObservable().Subscribe(_ => OnBuild());
            m_btns[1].OnClickAsObservable().Subscribe(_ => OnExpantion());
        }

        // Update is called once per frame
        void Update()
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
            m_bgFrameImage.gameObject.SetActive(selected);
        }

        public void OnClicked()
        {
            Selected?.Invoke(this);
        }

        public void OnBuild() { 
        
        }

        public void OnExpantion() { 
        
        }

        /// <summary>
        /// ボタン表示チェック
        /// </summary>
        private void CheckBtnDisp() {


        }
    }
}