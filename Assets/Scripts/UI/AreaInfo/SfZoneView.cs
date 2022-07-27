using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace sfproj
{
    /// <summary>
    /// ���r���|
    /// �n��r���[���̋��r���[
    /// </summary>
    public class SfZoneView : MonoBehaviour
    {
        // ���Z�� prefab
        [SerializeField]
        private GameObject m_zonePrefab = null;

        // ����ς݋�搔����(���p�\�ȋ��̐�)
        [SerializeField]
        private TextMeshProUGUI m_zoneCount = null;

        // ���Z�� transform
        [SerializeField]
        private List<Transform> m_zoneCellTransformList = new List<Transform>();

        // ���Z��
        private List<SfZoneCell> m_zoneCellList = new List<SfZoneCell>();

        // ���Z���̃f�[�^���X�g
        private List<SfZoneCellData> m_zoneCellDataList = new List<SfZoneCellData>();

        // true...����������
        private bool m_isInit = false;

        /// <summary>
        /// ��搔�l����e�[�u��
        /// </summary>
        private int[] m_zoneCtPopRatioTable = new int[] { 
            10,
            20,
            40,
            80,
            160,
            320,
            640,
            1280,
            2560
        };

        // Start is called before the first frame update
        void Start()
        {
            OnInitialize();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnInitialize()
        {
            if (m_isInit == true)
                return;
            m_isInit = true;

            // �v���n�u������Z�����쐬
            foreach (var t in m_zoneCellTransformList)
            {
                var obj = GameObject.Instantiate(m_zonePrefab, t);
                m_zoneCellList.Add(obj.GetComponent<SfZoneCell>());
            }
        }

        public void SetData(SfArea record) {

            OnInitialize();

            int zoneCt = 0;
            foreach (var zoneCtPopRatio in m_zoneCtPopRatioTable) {
                if (zoneCtPopRatio > record.Population)
                {
                    break;
                }
                zoneCt++;
            }

            int zoneMaxCt = record.MaxZoneCount;

            // �ő�l�����Ȃ�F���I�����W
            string colorStr = "<color=#F8913F>";

            if (zoneCt >= zoneMaxCt)
            {
                // �ő�l�ȏ�ɂȂ�����ő�l�ɐݒ肵�ĐF��ԂɕύX
                zoneCt = zoneMaxCt;
                colorStr = "<color=#F82121>";
            }
            
            m_zoneCount.text = colorStr + zoneCt.ToString() + "</color> / " + zoneMaxCt.ToString();

            for (int i = 0; i < SfConfigController.ZONE_MAX_DISPLAY_COUNT; ++i)
            {
                var zoneFacility = SfZoneFacilityTableManager.Instance.Table.Get(record.Id, i);
                SfZoneCellData zoneCellData = null;
                if (zoneFacility != null)
                {
                    zoneCellData = new SfZoneCellData(zoneFacility);
                }
                else
                {
                    zoneCellData = new SfZoneCellData(record.Id, i);
                }

                zoneCellData.SetZoneView(this);
                m_zoneCellDataList.Add(zoneCellData);

                // �n��ɑ��݂����搔���̋��f�[�^��ݒ�
                if (i < zoneCt)
                {
                    zoneCellData.UnlockFlag = true;
                    m_zoneCellList[i].gameObject.SetActive(true);
                }
                else if (i < zoneMaxCt)
                {
                    m_zoneCellList[i].gameObject.SetActive(true);
                }
                else
                {
                    m_zoneCellList[i].gameObject.SetActive(false);
                }

                m_zoneCellList[i].SetData(zoneCellData);
            }
        }

        /// <summary>
        /// �ĕ`��
        /// </summary>
        public void Reload()
        { 

        }
    }
}