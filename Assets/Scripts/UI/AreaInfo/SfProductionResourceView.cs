using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// ���Y�����r���[
    /// </summary>
    public class SfProductionResourceView : MonoBehaviour
    {
        // �Z���v���n�u
        [SerializeField]
        private GameObject m_cellPrefab = null;

        // �Z�� transform ���X�g
        [SerializeField]
        private List<Transform> m_cellTransformList = new List<Transform>();

        // ���Z��
        private List<SfProductionResourceCell> m_cellList = new List<SfProductionResourceCell>();

        // ���Z���̃f�[�^���X�g
        private List<SfProductionResourceCellData> m_cellDataList = new List<SfProductionResourceCellData>();

        // true...����������
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

            // �v���n�u������Z�����쐬
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

            // �n��f�[�^���ɂ��鐶�Y�����A�C�e�����X�g�����o���ݒ肵�Ă���

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