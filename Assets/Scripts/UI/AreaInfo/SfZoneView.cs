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
#if false
        // ���ő吔����(����A���J�����킹�Ă̍ő吔)
        [SerializeField]
        private TextMeshProUGUI m_zoneMaxCount = null;
#endif
        // ����ς݋�搔����(���p�\�ȋ��̐�)
        [SerializeField]
        private TextMeshProUGUI m_zoneCount = null;

        // ���Z��
        [SerializeField]
        private List<SfZoneCell> m_zoneCellList = new List<SfZoneCell>();

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
           
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetData(SfAreaRecord record) {

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

#if false
            m_zoneCount.text = zoneCt.ToString();
            m_zoneMaxCount.text = zoneMaxCt.ToString();
#endif
            for (int i = 0; i < zoneCt; ++i){

                var zoneCellData = new SfZoneCellData(i, record.ZoneTypeDict[i], record.ZoneExpantionDict[i]);

                if (i < zoneCt)
                {
                    zoneCellData.UnlockFlag = true;

                    m_zoneCellList[i].gameObject.SetActive(true);

                    m_zoneCellList[i].SetData(zoneCellData);
                }
                else
                {
                    zoneCellData.UnlockFlag = false;

                    m_zoneCellList[i].gameObject.SetActive(false) ;

                    m_zoneCellList[i].SetData(zoneCellData);
                }
            }
        }
    }
}