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
        // 区域セル prefab
        [SerializeField]
        private GameObject m_zonePrefab = null;

        // 解放済み区域数文字(利用可能な区域の数)
        [SerializeField]
        private TextMeshProUGUI m_zoneCount = null;

        // 区域セル transform
        [SerializeField]
        private List<Transform> m_zoneCellTransformList = new List<Transform>();

        // 区域セル
        private List<SfZoneCell> m_zoneCellList = new List<SfZoneCell>();

        // true...初期化完了
        private bool m_isInit = false;

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

            foreach (var t in m_zoneCellTransformList)
            {
                var obj = GameObject.Instantiate(m_zonePrefab, t);
                m_zoneCellList.Add(obj.GetComponent<SfZoneCell>());
            }
        }

        public void SetData(SfAreaRecord record) {

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

            // 最大値未満なら色をオレンジ
            string colorStr = "<color=#F8913F>";

            if (zoneCt >= zoneMaxCt)
            {
                // 最大値以上になったら最大値に設定して色を赤に変更
                zoneCt = zoneMaxCt;
                colorStr = "<color=#F82121>";
            }
            
            m_zoneCount.text = colorStr + zoneCt.ToString() + "</color> / " + zoneMaxCt.ToString();

            for (int i = 0; i < ConfigController.ZONE_MAX_DISPLAY_COUNT; ++i){

                // 表示数分のすべての区域データを作成
                SfZoneCellData zoneCellData = null ;
                if (i < zoneMaxCt)
                    zoneCellData = new SfZoneCellData(i, record.ZoneTypeDict[i], record.ZoneExpantionDict[i]);
                else
                    zoneCellData = new SfZoneCellData(i, eZoneType.None, 0);

                // 地域に存在する区域数分の区域データを設定
                if (i < zoneCt)
                {
                    zoneCellData.UnlockFlag = true;
                    m_zoneCellList[i].gameObject.SetActive(true);
                }
                else if (i < zoneMaxCt) {
                    m_zoneCellList[i].gameObject.SetActive(true);
                }
                else
                {
                    m_zoneCellList[i].gameObject.SetActive(false);
                }

                m_zoneCellList[i].SetData(zoneCellData);
            }
        }
    }
}