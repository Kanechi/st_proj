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
#if false
        // 区域最大数文字(解放、未開放合わせての最大数)
        [SerializeField]
        private TextMeshProUGUI m_zoneMaxCount = null;
#endif
        // 解放済み区域数文字(利用可能な区域の数)
        [SerializeField]
        private TextMeshProUGUI m_zoneCount = null;

        // 区域セル
        [SerializeField]
        private List<SfZoneCell> m_zoneCellList = new List<SfZoneCell>();

        /// <summary>
        /// 区域数人口比テーブル
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

            // 最大値未満なら色をオレンジ
            string colorStr = "<color=#F8913F>";

            if (zoneCt >= zoneMaxCt)
            {
                // 最大値以上になったら最大値に設定して色を赤に変更
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