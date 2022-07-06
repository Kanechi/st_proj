using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace sfproj
{
    public class SfAreaInfoView : MonoBehaviour
    {
        // true...ŠJ‚¢‚Ä‚¢‚é
        private bool m_openFlag = false;

        // ’nˆæ–¼
        [SerializeField]
        private TextMeshProUGUI m_areaName = null;



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool Open(uint areaId) {

            if (m_openFlag == true)
                return false;

            var record = SfAreaRecordTableManager.Instance.Get(areaId);

            return true;
        }
    }
}