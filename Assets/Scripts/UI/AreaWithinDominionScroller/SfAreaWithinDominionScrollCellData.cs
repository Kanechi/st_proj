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
    public class SfAreaWithinDominionScrollCellData
    {
        // 地域レコード
        public SfArea m_areaRecord = null;

        // 地域背景画像スプライト
        public Sprite m_areaBgImageSprite = null;

        // 地域画像スプライト
        public Sprite m_areaImageSprite = null;

        // true...開拓済み  false...未開拓
        public bool IsDevelopment = false;


        // データが参照しているセル
        public SfAreaWithinDominionScrollCell Cell { get; set; } = null;
        
        // true...選択(フォーカス)されている
        public bool IsSelected { get; set; } = false;

        public SfAreaWithinDominionScrollCellData(SfArea record) {

            m_areaRecord = record;

            // 地域背景画像スプライト
            m_areaBgImageSprite = AssetManager.Instance.Get<Sprite>(m_areaRecord.AreaGroupType.ToEnumString());

            // 地域画像スプライトの作成
            m_areaImageSprite = AssetManager.Instance.Get<Sprite>(m_areaRecord.AreaType.ToEnumString());

            // 開拓済みかどうかのチェック
            if (record.AreaDevelopmentState == eAreaDevelopmentState.Completed)
                IsDevelopment = true;
        }
    }
}