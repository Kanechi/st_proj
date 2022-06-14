using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{
    /// <summary>
    /// 表示している領域スクロールセル(地域セル)のデータ
    /// </summary>
    public class DominionScrollCellData
    {
        // 地域レコード
        public SfAreaRecord m_areaRecord = null;

        // 地域画像スプライト
        public Sprite m_areaImageSprite = null;

        public DominionScrollCellData(uint areaId) {

            m_areaRecord = SfAreaRecordTableManager.Instance.Get(areaId);

            // 地域画像スプライトの作成
        }
    }
}