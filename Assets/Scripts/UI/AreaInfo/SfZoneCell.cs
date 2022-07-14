using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace sfproj
{
    /// <summary>
    /// ���Z��
    /// �n����ɂ�����̃Z��
    /// </summary>
    public class SfZoneCell : MonoBehaviour
    {
        // ���摜(�������ĂȂ��ꍇ�͉摜����ԏ�ɗ��Ă���)
        [SerializeField]
        private GameObject m_lockObj = null;

        // �v���X�摜(������ꂽ��ԂŎ{�݂��������݂��Ă��Ȃ��ꍇ�͂��̉摜����ԏ�ɗ��Ă���)
        [SerializeField]
        private GameObject m_addObj = null;

        // ���{�݉摜(������ꂽ��ԂłȂɂ�����{�݂����݂���Ă���)
        [SerializeField]
        private GameObject m_zoneFacilityObj = null;

        // �g����
        [SerializeField]
        private TextMeshProUGUI m_expansionCount = null;

        // ���{�݉摜
        private Image m_zoneFacilityImage = null;
        private Image ZoneFacilityImage => m_zoneFacilityImage != null ? m_zoneFacilityImage : m_zoneFacilityImage = m_zoneFacilityObj.GetComponent<Image>();

        // ���̃Z���̃f�[�^
        private SfZoneCellData m_data = null;
        public SfZoneCellData Data => m_data;

        public void SetData(SfZoneCellData data) {
            m_data = data;
            m_data.Cell = this;

            if (m_data.UnlockFlag == true)
            {
                m_lockObj.SetActive(false);
            }
            else
            {
                m_lockObj.SetActive(true);
                m_addObj.SetActive(false);
                m_zoneFacilityObj.SetActive(false);
            }

            if (m_data.ZoneType != eZoneType.None)
            {
                m_addObj.SetActive(false);
            }
            else
            {
                m_addObj.SetActive(true);
                m_zoneFacilityObj.SetActive(false);
            }

            // �g�����̐ݒ�
            m_expansionCount.text = m_data.ExpansionCount.ToString();

            // �{�݉摜�̐ݒ�(�V���A���C�Y�f�[�^����摜�t�@�C��������)
            //ZoneFacilityImage.sprite = null;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // �^�C�v�ɂ�鎞�Ԍo�߂ɂ����鎑���̊l��
            // �������̓o�b�t�@�̐ݒ�
        }
    }
}