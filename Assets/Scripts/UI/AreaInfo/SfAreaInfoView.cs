using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using UnityEngine.Events;

namespace sfproj
{
    public class SfAreaInfoView : MonoBehaviour
    {
        // true...開いている
        private bool m_openFlag = false;
        public bool OpenFlag => m_openFlag;

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

        // 区域ビュー
        [SerializeField]
        private SfZoneView m_zoneView = null;

        // 生産され保管されている資源ビュー

        // 軍隊ビュー

        // Start is called before the first frame update
        void Start()
        {
            m_closeBtn.OnClickAsObservable().Subscribe(_ => OnClose());
            m_tradeBtn.OnClickAsObservable().Subscribe(_ => OnTrade());
            m_transportBtn.OnClickAsObservable().Subscribe(_ => OnTransport());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool Open(uint areaId) {

            if (m_openFlag == true)
                return false;

            var record = SfAreaDataTableManager.Instance.Get(areaId);

            m_areaName.text = record.Name;

            m_areaPopulation.text = record.Population.ToString();

            // 画像は画像テンプレートから設定
            //m_areaImage.sprite = Random.Range();

            m_zoneView.SetData(record);

            gameObject.SetActive(true);

            m_openFlag = true;

            return true;
        }

        /// <summary>
        /// 閉じる際の処理
        /// </summary>
        public void OnClose() {
            if (m_openFlag == false)
                return;

            gameObject.SetActive(false);

            m_openFlag = false;
        }

        /// <summary>
        /// 隣国の隣接する地域と資源の取引
        /// </summary>
        public void OnTrade() { 
        
        }

        /// <summary>
        /// 自国の別の地域に資源を輸送
        /// </summary>
        public void OnTransport() { 
        
        }
    }
}