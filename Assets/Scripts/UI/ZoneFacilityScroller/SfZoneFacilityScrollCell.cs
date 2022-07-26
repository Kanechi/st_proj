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
    /// ���ݎ{�ݑI��p�X�N���[���Z��
    /// </summary>
    public class SfZoneFacilityScrollCell : EnhancedScrollerCellView
    {
        static private float BaseCellSize = 164.0f;

        private RectTransform m_rectTransform;
        public RectTransform RectTransform => m_rectTransform != null ? m_rectTransform : m_rectTransform = GetComponent<RectTransform>();


        /// <summary>
        /// ���{�݉摜
        /// </summary>
        [SerializeField]
        private Image m_zoneFacilityImage = null;

        /// <summary>
        /// �w�i�摜
        /// </summary>
        [SerializeField]
        private Image m_selectedFrameImage = null;

        /// <summary>
        /// �w�i�{�^��
        /// </summary>
        [SerializeField]
        private Button m_bgFrameBtn = null;


        /// <summary>
        /// Build �{�^��
        /// </summary>
        [SerializeField]
        private Button m_buildButton = null;


        /// <summary>
        /// �w���s�摜
        /// </summary>
        [SerializeField]
        private Image m_notBuyImage = null;

        /// <summary>
        /// Expantion �{�^��
        /// </summary>
        [SerializeField]
        private Button m_expantionButton = null;

        public SfZoneFacilityScrollCellData Data { get; set; }

        public UnityAction<SfZoneFacilityScrollCell> Selected { get; set; } = null;

        // Start is called before the first frame update
        void Start()
        {
            m_bgFrameBtn.OnClickAsObservable().Subscribe(_ => OnClicked());
            m_buildButton.OnClickAsObservable().Subscribe(_ => OnBuildBtn());
            m_expantionButton.OnClickAsObservable().Subscribe(_ => OnExpantionBtn());
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// �{�^���\���`�F�b�N
        /// </summary>
        private void CheckBtnDisp()
        {
            m_buildButton.gameObject.SetActive(Data.EnableBuildBtn);

            m_notBuyImage.gameObject.SetActive(Data.EnableNotBuildImage);

            m_expantionButton.gameObject.SetActive(Data.EnableExpationBtn);

            //m_expantionButton.enabled = !Data.DisableExpantionImage;

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
            m_selectedFrameImage.gameObject.SetActive(selected);
        }

        public void OnClicked()
        {
            Selected?.Invoke(this);
        }

        /// <summary>
        /// ���݃{�^�����������ۂ̏���
        /// </summary>
        private void OnBuildBtn() {

            // �f�[�^�e�[�u����ύX
            SfAreaDataTableManager.Instance.ChangeZoneFacilityType(
                    Data.m_zoneCellData.AreaId,
                    Data.m_zoneCellData.CellIndex,
                    Data.m_record.Type
                );

            // �f�[�^�e�[�u���ɂ���{�݂�ύX������A�v���X�{�^���A���{�݃{�^���ɕύX����K�v������

            // �Z���f�[�^��ύX
            Data.m_zoneCellData.ChangeFacilityType(Data.m_record.Type);
        }

        /// <summary>
        /// �g���{�^�����������ۂ̏���
        /// </summary>
        private void OnExpantionBtn() { 
        
            //Data.m_zoneCellData.ExpansionCount
        }
    }
}