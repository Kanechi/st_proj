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
    public class SfAreaWithinDominionScrollCell : EnhancedScrollerCellView
    {
        static private float BaseCellSize = 164.0f;

        private RectTransform m_rectTransform;
        public RectTransform RectTransform => m_rectTransform != null ? m_rectTransform : m_rectTransform = GetComponent<RectTransform>();

        /// <summary>
        /// �w�i
        /// </summary>
        [SerializeField]
        private Image m_bg = null;

        RectTransform m_bgRect = null;
        RectTransform BgRect => m_bgRect != null ? m_bgRect : m_bgRect = m_bg.gameObject.GetComponent<RectTransform>();

        /// <summary>
        /// �}�X�N�摜
        /// </summary>
        [SerializeField]
        private Image m_maskBg = null;

        RectTransform m_maskRect = null;
        RectTransform MaskRect => m_maskRect != null ? m_maskRect : m_maskRect = m_maskBg.gameObject.GetComponent<RectTransform>();

        /// <summary>
        /// �n��摜(���A��A��ՁA���A)
        /// </summary>
        [SerializeField]
        private Image m_areaImage = null;

        RectTransform m_selectedImageRect = null;
        RectTransform SelectedImageRect => m_selectedImageRect != null ? m_selectedImageRect : m_selectedImageRect = m_selectedImage.gameObject.GetComponent<RectTransform>();

        /// <summary>
        /// �n��E�B���h�E�J���{�^��
        /// �J��{�^��
        /// </summary>
        [SerializeField]
        private Button m_btn = null;

        /// <summary>
        /// ���J��摜
        /// �J��ς݂ɂȂ������\���ɂ���
        /// </summary>
        [SerializeField]
        private Image m_notDevelopmentImage = null;

        /// <summary>
        /// �n��ɑ��݂��Ă���n�`�̃A�C�R��
        /// </summary>
        [SerializeField]
        private Image[] m_terrainIconArray = null;


        /// <summary>
        /// �t�H�[�J�X�I�𒆉摜
        /// </summary>
        [SerializeField]
        private Image m_selectedImage = null;

        [SerializeField]
        private RectTransform m_existTerrainListRect = null;

        /// <summary>
        /// ���ݐݒ肳��Ă���̈�Z���f�[�^
        /// </summary>
        public SfAreaWithinDominionScrollCellData Data { get; set; } = null;

        // ��D�Z���r���[�ɂ����D���^�b�`�����ۂ̏���
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

            // ���̐ݒ�
            name = data.m_areaRecord.m_name;

            // �T�C�Y�`�F�b�N
            float size = SfAreaWithinDominionScrollView.CellSize.x / BaseCellSize;

            // �w�i�T�C�Y�̐ݒ�
            BgRect.sizeDelta = SfAreaWithinDominionScrollView.CellSize;

            // �}�X�N�摜�̃T�C�Y��ݒ�
            MaskRect.sizeDelta = SfAreaWithinDominionScrollView.MaskCellSize;

            // �I���摜�̃T�C�Y��I��
            SelectedImageRect.sizeDelta = SfAreaWithinDominionScrollView.SelectedImageSize;

            // �w�i�摜�̐ݒ�
            m_bg.sprite = Data.m_areaBgImageSprite;

            // �摜�̐ݒ�
            m_areaImage.sprite = Data.m_areaImageSprite;

            // ���T���摜�̕\���ݒ�
            m_notDevelopmentImage.gameObject.SetActive(!Data.IsDevelopment);

            // �n�`�A�C�R���̕\����\���ݒ�
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
        /// �N���b�N�����ۂ̏���
        /// �t�H�[�J�X�����̂�
        /// �I��������Ԃōēx�^�b�`���Ă������N����Ȃ�
        /// </summary>
        private void OnClicked()
        {
            // �E�B���h�E�J������
            switch (Data.m_areaRecord.AreaDevelopmentState)
            {
                case eAreaDevelopmentState.Not:
                    {
                        // �J��E�B���h�E�J��
                    }
                    break;
                case eAreaDevelopmentState.Completed:
                    {
                        // �n��E�B���h�E�\��
                        SfGameManager.Instance.AreaInfoView.Open(Data.m_areaRecord.Id);
                    }
                    break;
                default:
                    // �J�񒆂̍ۂ͏����Ȃ�
                    break;
            }

            // �t�H�[�J�X����
            Selected?.Invoke(this);
        }
    }
}