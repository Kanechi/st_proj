using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// �r���[�n�̊Ǘ�
    /// </summary>
    public class SfViewManager : SingletonMonoBehaviour<SfViewManager>
    {

        /// <summary>
        /// �̓y�X�N���[���r���[
        /// </summary>
        [SerializeField]
        private DominionScrollView m_dominionScrollView = null;
        public DominionScrollView DominionScrollView => m_dominionScrollView;

        /// <summary>
        /// �n����r���[
        /// </summary>
        [SerializeField]
        private SfAreaInfoView m_areaInfoView = null;
        public SfAreaInfoView AreaInfoView => m_areaInfoView;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnFinalize() {

            // �Q�[�����C���I�����Ƀr���[�n�����ׂĉ�����ă^�C�g����ʂɖ߂�
            m_dominionScrollView = null;
            m_areaInfoView = null;
        }
    }
}