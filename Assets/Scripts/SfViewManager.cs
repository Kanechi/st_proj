using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// ビュー系の管理
    /// </summary>
    public class SfViewManager : SingletonMonoBehaviour<SfViewManager>
    {

        /// <summary>
        /// 領土スクロールビュー
        /// </summary>
        [SerializeField]
        private DominionScrollView m_dominionScrollView = null;
        public DominionScrollView DominionScrollView => m_dominionScrollView;

        /// <summary>
        /// 地域情報ビュー
        /// </summary>
        [SerializeField]
        private SfAreaInfoView m_areaInfoView = null;
        public SfAreaInfoView AreaInfoView => m_areaInfoView;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnFinalize() {

            // ゲームメイン終了時にビュー系をすべて解放してタイトル画面に戻す
            m_dominionScrollView = null;
            m_areaInfoView = null;
        }
    }
}