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

        // 地域背景画像スプライト
        public Sprite m_areaBgImageSprite = null;

        // 地域画像スプライト
        public Sprite m_areaImageSprite = null;

        // データが参照しているセル
        public DominionScrollCell Cell { get; set; } = null;
        
        // true...選択(フォーカス)されている
        public bool IsSelected { get; set; } = false;

        public DominionScrollCellData(SfAreaRecord record) {

            m_areaRecord = record;

            // 地域背景画像スプライト
            m_areaBgImageSprite = AssetManager.Instance.Get<Sprite>(m_areaRecord.AreaGroupType.ToEnumString());

            // 地域画像スプライトの作成
            m_areaImageSprite = AssetManager.Instance.Get<Sprite>(m_areaRecord.AreaType.ToEnumString());
        }
    }
}