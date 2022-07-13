using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace sfproj
{
    public class SfAreaInfoView : MonoBehaviour
    {
        // true...開いている
        private bool m_openFlag = false;

        // 地域名
        [SerializeField]
        private TextMeshProUGUI m_areaName = null;

        // 地域人口
        [SerializeField]
        private TextMeshProUGUI m_areaPopulation = null;

        // 地域画像
        [SerializeField]
        private Image m_areaImage = null;

        // 閉じるボタン
        [SerializeField]
        private Button m_closeBtn = null;

        // TRADE ボタン
        [SerializeField]
        private Button m_tradeBtn = null;

        // TRANSPORT ボタン
        [SerializeField]
        private Button m_transportBtn = null;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool Open(uint areaId) {

            if (m_openFlag == true)
                return false;

            var record = SfAreaRecordTableManager.Instance.Get(areaId);

            return true;
        }
    }
}