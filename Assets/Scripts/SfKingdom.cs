using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// 王国
    /// 保存情報
    /// ゲーム開始時に一度だけ作成
    /// </summary>
    [Serializable]
    public class SfKingdomRecord
    {
        [Serializable]
        public class SfKingdomDetail
        {
            // 王国 ID
            public uint m_id = 0;
            // 王国名
            public string m_name = "";
            // 王国の色
            public Color m_color = Color.white;

            // 領域 ID リスト
            public List<uint> m_sfDominionIdList = new List<uint>();
        }
    }

    /// <summary>
    /// 王国
    /// スクロールビューをタップしたらどこかに簡易ウィンドウとして表示
    /// </summary>
    public class SfKingdom : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}