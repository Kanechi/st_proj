using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UniRx;
using System.Linq;
using UnityEngine.UI;

namespace sfproj
{
    /// <summary>
    /// �̈�X�N���[���r���[�̃Z��(�n��Z��)
    /// �^�b�`���邱�ƂŒn��E�B���h�E���㕔�ɕ\������
    /// </summary>
    public class DominionScrollCell : EnhancedScrollerCellView
    {
        /// <summary>
        /// �w�i
        /// </summary>
        [SerializeField]
        public Image m_bg = null;

        /// <summary>
        /// �n��摜(���A��A��ՁA���A)
        /// </summary>
        [SerializeField]
        public Image m_areaImage = null;

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


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


    }
}