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
        // true...�J���Ă���
        private bool m_openFlag = false;
        public bool OpenFlag => m_openFlag;

        // �n�於
        [SerializeField]
        private TextMeshProUGUI m_areaName = null;

        // �n��l��
        [SerializeField]
        private TextMeshProUGUI m_areaPopulation = null;

        // �n��摜
        [SerializeField]
        private Image m_areaImage = null;

        // ����{�^��
        [SerializeField]
        private Button m_closeBtn = null;

        // TRADE �{�^��
        [SerializeField]
        private Button m_tradeBtn = null;

        // TRANSPORT �{�^��
        [SerializeField]
        private Button m_transportBtn = null;

        // ���r���[
        [SerializeField]
        private SfZoneView m_zoneView = null;

        // ���Y����ۊǂ���Ă��鎑���r���[

        // �R���r���[

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

            // �摜�͉摜�e���v���[�g����ݒ�
            //m_areaImage.sprite = Random.Range();

            m_zoneView.SetData(record);

            gameObject.SetActive(true);

            m_openFlag = true;

            return true;
        }

        /// <summary>
        /// ����ۂ̏���
        /// </summary>
        public void OnClose() {
            if (m_openFlag == false)
                return;

            gameObject.SetActive(false);

            m_openFlag = false;
        }

        /// <summary>
        /// �׍��̗אڂ���n��Ǝ����̎��
        /// </summary>
        public void OnTrade() { 
        
        }

        /// <summary>
        /// �����̕ʂ̒n��Ɏ�����A��
        /// </summary>
        public void OnTransport() { 
        
        }
    }
}