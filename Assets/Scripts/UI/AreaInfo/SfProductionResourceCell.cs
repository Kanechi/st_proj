using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace sfproj
{
    /// <summary>
    /// 地域に表示される生産資源ビューのアイコンセル
    /// </summary>
    public class SfProductionResourceCell : MonoBehaviour
    {
        // 生産資源アイコン画像
        [SerializeField]
        private Image m_iconImage = null;

        [SerializeField]
        private TextMeshProUGUI m_rarityText = null;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetData(SfProductionResourceCellData data)
        {
            m_iconImage.sprite = data.IconSprite;

            m_rarityText.text = data.Rerity.ToEnumString();

            string rarityColor = "E2C799";
            // レア度で色を変化
            if (data.Rerity == eRarity.Rare)
            {
                rarityColor = "FF00C1";
            }
            Color color;
            ColorUtility.TryParseHtmlString(rarityColor, out color);
            m_rarityText.color = color;
        }
    }
}