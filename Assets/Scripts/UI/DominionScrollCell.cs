using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UniRx;
using System.Linq;
using UnityEngine.UI;

namespace sfproj
{
    /// <summary>
    /// 領域スクロールビューのセル(地域セル)
    /// タッチすることで地域ウィンドウを上部に表示する
    /// </summary>
    public class DominionScrollCell : EnhancedScrollerCellView
    {
        /// <summary>
        /// 背景
        /// </summary>
        [SerializeField]
        public Image m_bg = null;

        /// <summary>
        /// 地域画像(町、城、遺跡、洞窟)
        /// </summary>
        [SerializeField]
        public Image m_areaImage = null;

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


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


    }
}