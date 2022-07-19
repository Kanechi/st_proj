using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGS;

namespace sfproj
{
    /// <summary>
    /// ビュー系の管理
    /// </summary>
    public class SfGameManager : SingletonMonoBehaviour<SfGameManager>
    {
        /// <summary>
        /// 現在のキャンバス
        /// </summary>
        [SerializeField]
        private Canvas m_canvas = null;
        public Canvas CurrentCanvas => m_canvas;

        /// <summary>
        /// 領土スクロールビュー
        /// </summary>
        [SerializeField]
        private SfAreaWithinDominionScrollView m_areaWithinDominionScrollView = null;
        public SfAreaWithinDominionScrollView AreaWithinDominionScrollView => m_areaWithinDominionScrollView;

        /// <summary>
        /// 地域情報ビュー
        /// </summary>
        [SerializeField]
        private SfAreaInfoView m_areaInfoView = null;
        public SfAreaInfoView AreaInfoView => m_areaInfoView;

        /// <summary>
        /// release 時入力処理
        /// </summary>
        [SerializeField]
        private SfInputSystem m_inputSystem = null;
        public SfInputSystem InputSystem => m_inputSystem;

        protected override void Awake()
        {
            base.Awake();

            m_areaWithinDominionScrollView.gameObject.SetActive(false);
            m_areaInfoView.gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnInitialize() {

        }

        public void OnFinalize() {

            // ゲームメイン終了時にビュー系をすべて解放してタイトル画面に戻す
            m_areaWithinDominionScrollView = null;
            m_areaInfoView = null;
        }
    }
}