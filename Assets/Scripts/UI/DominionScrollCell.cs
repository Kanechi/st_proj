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
    /// �̈�X�N���[���r���[�̃Z��(�n��Z��)
    /// �^�b�`���邱�ƂŒn��E�B���h�E���㕔�ɕ\������
    /// </summary>
    public class DominionScrollCell : EnhancedScrollerCellView
    {
        private RectTransform m_rectTransform;
        public RectTransform RectTransform => m_rectTransform != null ? m_rectTransform : m_rectTransform = GetComponent<RectTransform>();

        /// <summary>
        /// �w�i
        /// </summary>
        [SerializeField]
        public Image m_bg = null;

        RectTransform m_bgRect = null;
        RectTransform BgRect => m_bgRect != null ? m_bgRect : m_bgRect = m_bg.gameObject.GetComponent<RectTransform>();

        /// <summary>
        /// �}�X�N�摜
        /// </summary>
        [SerializeField]
        public Image m_maskBg = null;

        RectTransform m_maskRect = null;
        RectTransform MaskRect => m_maskRect != null ? m_maskRect : m_maskRect = m_maskBg.gameObject.GetComponent<RectTransform>();

        /// <summary>
        /// �n��摜(���A��A��ՁA���A)
        /// </summary>
        [SerializeField]
        public Image m_areaImage = null;

        RectTransform m_selectedImageRect = null;
        RectTransform SelectedImageRect => m_selectedImageRect != null ? m_selectedImageRect : m_selectedImageRect = m_selectedImage.gameObject.GetComponent<RectTransform>();

        /// <summary>
        /// �n��E�B���h�E�J���{�^��
        /// </summary>
        [SerializeField]
        public Button m_openAreaWindowBtn = null;

        /// <summary>
        /// �t�H�[�J�X�I�𒆉摜
        /// </summary>
        [SerializeField]
        public Image m_selectedImage = null;

        /// <summary>
        /// ���ݐݒ肳��Ă���̈�Z���f�[�^
        /// </summary>
        public DominionScrollCellData Data { get; set; } = null;

        // ��D�Z���r���[�ɂ����D���^�b�`�����ۂ̏���
        public UnityAction<DominionScrollCell> Selected { get; set; } = null;

        public int DataIndex { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            m_openAreaWindowBtn.OnClickAsObservable().Subscribe(_ => OnClicked());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetData(int dataIndex, DominionScrollCellData data)
        {
            DataIndex = dataIndex;
            Data = data;
            Data.Cell = this;

            // ���̐ݒ�
            name = data.m_areaRecord.m_name;

            // �w�i�T�C�Y�̐ݒ�
            BgRect.sizeDelta = DominionScrollView.CellSize;

            // �}�X�N�摜�̃T�C�Y��ݒ�
            MaskRect.sizeDelta = DominionScrollView.MaskCellSize;

            // �I���摜�̃T�C�Y��I��
            SelectedImageRect.sizeDelta = DominionScrollView.SelectedImageSize;

            // �w�i�摜�̐ݒ�
            m_bg.sprite = Data.m_areaBgImageSprite;

            // �摜�̐ݒ�
            m_areaImage.sprite = Data.m_areaImageSprite;

            SetSelected(Data.IsSelected);
        }

        public void SetSelected(bool select)
        {
            m_selectedImage.gameObject.SetActive(select);
        }

        /// <summary>
        /// �N���b�N�����ۂ̏���
        /// �t�H�[�J�X�����̂�
        /// �I��������Ԃōēx�^�b�`���Ă������N����Ȃ�
        /// </summary>
        private void OnClicked()
        {
            Selected?.Invoke(this);
        }
    }
}