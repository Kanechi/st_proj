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
        // ���ő吔����(����A���J�����킹�Ă̍ő吔)
        [SerializeField]
        private TextMeshProUGUI m_zoneMaxCount = null;

        // ����ς݋�搔����(���p�\�ȋ��̐�)
        [SerializeField]
        private TextMeshProUGUI m_zoneCount = null;

        // ���Z��
        [SerializeField]
        private List<SfZoneCell> m_zoneCellList = new List<SfZoneCell>();

        // Start is called before the first frame update
        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetData(SfAreaRecord record) {

            // �l�����猻�݉������Ă��鐔���v�Z
            int zoneCt = record.Pupulation / 10;

            int zoneMaxCt = record.MaxZoneCount;

            if (zoneCt > zoneMaxCt)
                zoneCt = zoneMaxCt;

            m_zoneCount.text = zoneCt.ToString();
            m_zoneMaxCount.text = zoneMaxCt.ToString();

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