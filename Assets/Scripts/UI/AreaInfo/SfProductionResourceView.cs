using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// 生産資源ビュー
    /// </summary>
    public class SfProductionResourceView : MonoBehaviour
    {
        // セルプレハブ
        [SerializeField]
        private GameObject m_cellPrefab = null;

        // セル transform リスト
        [SerializeField]
        private List<Transform> m_cellTransformList = new List<Transform>();

        // 区域セル
        private List<SfProductionResourceCell> m_cellList = new List<SfProductionResourceCell>();

        // 区域セルのデータリスト
        private List<SfProductionResourceCellData> m_cellDataList = new List<SfProductionResourceCellData>();

        // true...初期化完了
        private bool m_isInit = false;


        // Start is called before the first frame update
        void Start()
        {
            OnInitialize();
        }

        public void OnInitialize()
        {
            if (m_isInit == true)
                return;
            m_isInit = true;

            // プレハブから区域セルを作成
            foreach (var t in m_cellTransformList)
            {
                var obj = GameObject.Instantiate(m_cellPrefab, t);
                m_cellList.Add(obj.GetComponent<SfProductionResourceCell>());
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetData(SfArea area)
        {
            OnInitialize();

            // 地域データ内にある生産資源アイテムリストを取り出し設定していく

            for (int i = 0; i < m_cellTransformList.Count; ++i)
            {
                if (i >= area.ProductionResourceItemIdList.Count)
                {
                    m_cellList[i].gameObject.SetActive(false);
                }
                else
                {
                    m_cellList[i].gameObject.SetActive(true);

                    var item = SfProductionResourceItemTableManager.Instance.Table.Get(area.ProductionResourceItemIdList[i]);

                    var data = new SfProductionResourceCellData(item);

                    m_cellList[i].SetData(data);
                }
            }
        }
    }
}