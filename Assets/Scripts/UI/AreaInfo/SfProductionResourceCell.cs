using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace sfproj
{
    /// <summary>
    /// �n��ɕ\������鐶�Y�����r���[�̃A�C�R���Z��
    /// </summary>
    public class SfProductionResourceCell : MonoBehaviour
    {
        // ���Y�����A�C�R���摜
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
            // ���A�x�ŐF��ω�
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