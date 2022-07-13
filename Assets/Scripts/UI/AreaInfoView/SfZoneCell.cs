using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace sfproj
{
    /// <summary>
    /// 区域セル
    /// 地域情報にある区域のセル
    /// </summary>
    public class SfZoneCell : MonoBehaviour
    {
        // 鍵画像(解放されてない場合は画像が一番上に来ている)
        [SerializeField]
        private GameObject m_lockObj = null;

        // プラス画像(解放された状態で施設を何も建設していない場合はこの画像が一番上に来ている)
        [SerializeField]
        private GameObject m_addObj = null;

        // 区域施設画像(解放された状態でなにかしら施設が建設されている)
        [SerializeField]
        private GameObject m_zoneFacilityObj = null;

        // 拡張数
        [SerializeField]
        private TextMeshProUGUI m_expansionCount = null;

        // 区域施設画像
        private Image m_zoneFacilityImage = null;
        private Image ZoneFacilityImage => m_zoneFacilityImage != null ? m_zoneFacilityImage : m_zoneFacilityImage = m_zoneFacilityObj.GetComponent<Image>();

        // このセルのデータ
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

            // 拡張数の設定
            m_expansionCount.text = m_data.ExpansionCount.ToString();

            // 施設画像の設定(シリアライズデータから画像ファイルを検索)
            //ZoneFacilityImage.sprite = null;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // タイプによる時間経過における資源の獲得
            // もしくはバッファの設定
        }
    }
}