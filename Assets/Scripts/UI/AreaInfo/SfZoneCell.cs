using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UniRx;
using System.Linq;

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
        // �v���X�����̃{�^��
        [SerializeField]
        private Button m_addBtn = null;

        // ���{�݉摜(������ꂽ��ԂłȂɂ�����{�݂����݂���Ă���)
        [SerializeField]
        private GameObject m_zoneFacilityObj = null;
        // ���{�݂���������Ă��镔���̃{�^��
        // �����̓v���X�Ɠ����ł��łɌ�������Ă���{�݂͌����ł��Ȃ��悤�ɂ���̂�
        // �g�����ǉ�����Ă���̂��Ⴂ
        [SerializeField]
        private Button m_zoneFacilityBtn = null;

        // �g����
        [SerializeField]
        private TextMeshProUGUI m_expansionCount = null;

        // ���{�݉摜
        [SerializeField]
        private Image m_zoneFacilityImage = null;

        // ���̃Z���̃f�[�^
        [ShowInInspector, ReadOnly]
        private SfZoneCellData m_data = null;
        public SfZoneCellData Data => m_data;

        // Start is called before the first frame update
        void Start()
        {
            m_addBtn.OnClickAsObservable().Subscribe(_ => OnClickedAddBtn());
            m_zoneFacilityBtn.OnClickAsObservable().Subscribe(_ => OnClickedZoneFacilityBtn());
        }

        // Update is called once per frame
        void Update()
        {
            // �^�C�v�ɂ�鎞�Ԍo�߂ɂ����鎑���̊l��
            // �������̓o�b�t�@�̐ݒ�
        }

        public void SetData(SfZoneCellData data)
        {
            m_data = data;
            m_data.Cell = this;

            SettingZoneButtonEnable();

            // �g�����̐ݒ�
            m_expansionCount.text = m_data.ExpansionCount.ToString();
        }

        /// <summary>
        /// ���{�^���̗L�����ݒ�
        /// </summary>
        public void SettingZoneButtonEnable() {

            // ���b�N�{�^���\���`�F�b�N
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

            // �v���X�{�^���\���`�F�b�N
            if (m_data.ZoneFacilityType != eZoneFacilityType.None)
            {
                m_addObj.SetActive(false);
                m_zoneFacilityObj.SetActive(true);
                // ���{�݉摜�̐ݒ�
                SettingFacilityImage();
            }
            else
            {
                m_addObj.SetActive(true);
                m_zoneFacilityObj.SetActive(false);
            }
        }

        /// <summary>
        /// ���{�݉摜�̐ݒ�
        /// </summary>
        public void SettingFacilityImage() {
            var record = SfZoneFacilityRecordTable.Instance.Get(m_data.ZoneFacilityType);
            m_zoneFacilityImage.sprite = record.FacilitySprite;
        }

        /// <summary>
        /// �v���X�{�^�����������ۂ̏���
        /// </summary>
        private void OnClickedAddBtn() {

            SfGameManager.Instance.ZoneFacilityScrollView.Open(Data);
        }

        /// <summary>
        /// ���{�݃{�^�����������ۂ̏���
        /// </summary>
        private void OnClickedZoneFacilityBtn() {

            SfGameManager.Instance.ZoneFacilityScrollView.Open(Data);
        }
    }
}