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
    public class SfAreaWithinDominionScrollCell : EnhancedScrollerCellView
    {
        static private float BaseCellSize = 164.0f;

        private RectTransform m_rectTransform;
        public RectTransform RectTransform => m_rectTransform != null ? m_rectTransform : m_rectTransform = GetComponent<RectTransform>();

        /// <summary>
        /// 背景
        /// </summary>
        [SerializeField]
        private Image m_bg = null;

        RectTransform m_bgRect = null;
        RectTransform BgRect => m_bgRect != null ? m_bgRect : m_bgRect = m_bg.gameObject.GetComponent<RectTransform>();

        /// <summary>
        /// マスク画像
        /// </summary>
        [SerializeField]
        private Image m_maskBg = null;

        RectTransform m_maskRect = null;
        RectTransform MaskRect => m_maskRect != null ? m_maskRect : m_maskRect = m_maskBg.gameObject.GetComponent<RectTransform>();

        /// <summary>
        /// 地域画像(町、城、遺跡、洞窟)
        /// </summary>
        [SerializeField]
        private Image m_areaImage = null;

        RectTransform m_selectedImageRect = null;
        RectTransform SelectedImageRect => m_selectedImageRect != null ? m_selectedImageRect : m_selectedImageRect = m_selectedImage.gameObject.GetComponent<RectTransform>();

        /// <summary>
        /// 地域ウィンドウ開示ボタン
        /// 開拓ボタン
        /// </summary>
        [SerializeField]
        private Button m_btn = null;

        /// <summary>
        /// 未開拓画像
        /// 開拓済みになったら非表示にする
        /// </summary>
        [SerializeField]
        private Image m_notDevelopmentImage = null;

        /// <summary>
        /// 地域に存在している地形のアイコン
        /// </summary>
        [SerializeField]
        private Image[] m_terrainIconArray = null;


        /// <summary>
        /// フォーカス選択中画像
        /// </summary>
        [SerializeField]
        private Image m_selectedImage = null;

        [SerializeField]
        private RectTransform m_existTerrainListRect = null;

        /// <summary>
        /// 現在設定されている領域セルデータ
        /// </summary>
        public SfAreaWithinDominionScrollCellData Data { get; set; } = null;

        // 手札セルビューにある手札をタッチした際の処理
        public UnityAction<SfAreaWithinDominionScrollCell> Selected { get; set; } = null;

        public int DataIndex { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            m_btn.OnClickAsObservable().Subscribe(_ => OnClicked());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetData(int dataIndex, SfAreaWithinDominionScrollCellData data)
        {
            DataIndex = dataIndex;
            Data = data;
            Data.Cell = this;

            // 名称設定
            name = data.m_areaRecord.m_name;

            // サイズチェック
            float size = SfAreaWithinDominionScrollView.CellSize.x / BaseCellSize;

            // 背景サイズの設定
            BgRect.sizeDelta = SfAreaWithinDominionScrollView.CellSize;

            // マスク画像のサイズを設定
            MaskRect.sizeDelta = SfAreaWithinDominionScrollView.MaskCellSize;

            // 選択画像のサイズを選択
            SelectedImageRect.sizeDelta = SfAreaWithinDominionScrollView.SelectedImageSize;

            // 背景画像の設定
            m_bg.sprite = Data.m_areaBgImageSprite;

            // 画像の設定
            m_areaImage.sprite = Data.m_areaImageSprite;

            // 未探索画像の表示設定
            m_notDevelopmentImage.gameObject.SetActive(!Data.IsDevelopment);

            // 地形アイコンの表示非表示設定
            SettingTerrainIcon();

            SettingTerrainIconSize(size);

            var sizeDelta = m_existTerrainListRect.sizeDelta;
            sizeDelta.x = 150.0f * size;
            m_existTerrainListRect.sizeDelta = sizeDelta;

            SetSelected(Data.IsSelected);
        }

        private eExistingTerrain[] m_existingTerrains = {
            eExistingTerrain.Plane,
            eExistingTerrain.Forest,
            eExistingTerrain.Mountain,
            eExistingTerrain.River,
            eExistingTerrain.Ocean,
        };

        private void SettingTerrainIcon() {
            for (int i = 0; i < 5; ++i)
            {
                //m_terrainIconArray[i].gameObject.SetActive();
                m_terrainIconArray[i].enabled = ((Data.m_areaRecord.ExistingTerrain & m_existingTerrains[i]) != 0);
            }
        }

        private void SettingTerrainIconSize(float size) {
            foreach (var icon in m_terrainIconArray)
            {
                var scale = icon.transform.localScale;
                scale.x = size;
                scale.y = size;
                icon.transform.localScale = scale;
            }
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
            // ウィンドウ開示処理
            switch (Data.m_areaRecord.AreaDevelopmentState)
            {
                case eAreaDevelopmentState.Not:
                    {
                        // 開拓ウィンドウ開示
                    }
                    break;
                case eAreaDevelopmentState.Completed:
                    {
                        // 地域ウィンドウ表示
                        SfGameManager.Instance.AreaInfoView.Open(Data.m_areaRecord.Id);
                    }
                    break;
                default:
                    // 開拓中の際は処理なし
                    break;
            }

            // フォーカス判定
            Selected?.Invoke(this);
        }
    }
}