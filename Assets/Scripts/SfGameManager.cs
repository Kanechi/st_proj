using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGS;

namespace sfproj
{
    /// <summary>
    /// �r���[�n�̊Ǘ�
    /// </summary>
    public class SfGameManager : SingletonMonoBehaviour<SfGameManager>
    {
        /// <summary>
        /// ���݂̃L�����o�X
        /// </summary>
        [SerializeField]
        private Canvas m_canvas = null;
        public Canvas CurrentCanvas => m_canvas;

        /// <summary>
        /// �̓y�X�N���[���r���[
        /// </summary>
        [SerializeField]
        private SfAreaWithinDominionScrollView m_areaWithinDominionScrollView = null;
        public SfAreaWithinDominionScrollView AreaWithinDominionScrollView => m_areaWithinDominionScrollView;

        /// <summary>
        /// �n����r���[
        /// </summary>
        [SerializeField]
        private SfAreaInfoView m_areaInfoView = null;
        public SfAreaInfoView AreaInfoView => m_areaInfoView;

        /// <summary>
        /// release �����͏���
        /// </summary>
        [SerializeField]
        private SfInputSystem m_inputSystem = null;
        public SfInputSystem InputSystem => m_inputSystem;

        protected override void Awake()
        {
            base.Awake();

            m_areaWithinDominionScrollView.gameObject.SetActive(false);
            m_areaInfoView.gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnInitialize() {

        }

        public void OnFinalize() {

            // �Q�[�����C���I�����Ƀr���[�n�����ׂĉ�����ă^�C�g����ʂɖ߂�
            m_areaWithinDominionScrollView = null;
            m_areaInfoView = null;
        }
    }
}