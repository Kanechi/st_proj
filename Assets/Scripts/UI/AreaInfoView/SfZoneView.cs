using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace sfproj
{
    /// <summary>
    /// 区域ビュ－
    /// 地域ビュー内の区域ビュー
    /// </summary>
    public class SfZoneView : MonoBehaviour
    {
        // 区域最大数文字(解放、未開放合わせての最大数)
        [SerializeField]
        private TextMeshProUGUI m_zoneMaxCount = null;

        // 解放済み区域数文字(利用可能な区域の数)
        [SerializeField]
        private TextMeshProUGUI m_zoneCount = null;

        // 区域セル
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

            // 人口から現在解放されている数を計算
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