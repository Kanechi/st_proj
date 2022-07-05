using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace sfproj
{
    /// <summary>
    /// �����̉������
    /// ����̏�������ۂ͒n����Ƃ��̈��񂩂猩��̂ł���ł͂Ȃ�
    /// ����̏��
    /// 
    /// </summary>
    public class SfKingdomUserInfo : MonoBehaviour
    {
        // ��摜
        [SerializeField]
        private Image m_faceImage = null;

        // �l���Q�[�W
        [SerializeField]
        private Slider m_popSlider = null;

        // �l��/�ő�l���e�L�X�g
        [SerializeField]
        private TextMeshProUGUI m_popText = null;

        // �������x���e�L�X�g
        [SerializeField]
        private TextMeshProUGUI m_kingdomLvText = null;

        // �����o���l�Q�[�W
        [SerializeField]
        private Slider m_expSlider = null;

        // ������
        [SerializeField]
        private TextMeshProUGUI m_kingdomName = null;


        // �����̉����̃f�[�^
        private SfKingdomRecord m_record = null;
        public SfKingdomRecord SelfKingdomRecord => m_record;


        // Start is called before the first frame update
        void Start()
        {
            
            m_record = SfKingdomRecordTableManager.Instance.GetSelfKingdom();

            // ��摜�̐ݒ�

            // �������̐ݒ�
        }

        // Update is called once per frame
        void Update()
        {
#if false
            // �l���̕����͗̈� ID ����n�� ID �܂ł����̂ڂ��Ēn����̐l���̍��v��
            // �ő�l���̍��v�Ōv�Z���s��
            m_popSlider.maxValue = m_record.MaxPopulation;

            m_popSlider.value = m_record.Population;

            m_popText.text = m_record.Population.ToString() + "/" + m_record.MaxPopulation.ToString();
#endif
            // �o���l�e�[�u�����玟�̌o���l���擾���Čo���l�X���C�_�[�ɐݒ�
            //m_expSlider.maxValue

            // ���݂̌o���l��ݒ�(�ݐό^)
            m_expSlider.value = m_record.m_kingdomExp;

            // ���x���A�b�v�̓��R�[�h�Ǘ��ōs��
            m_kingdomLvText.text = m_record.m_kingdomLv.ToString();
        }


    }
}