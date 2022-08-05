using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    public class SfProductionResourceCellData
    {
        // アイコン画像
        public Sprite IconSprite { get; private set; }



        // レア度文字列
        public eRarity Rerity { get; private set; }

        public SfProductionResourceCellData(SfProductionResourceItem item)
        {
            var record = SfProductionResourceTableManager.Instance.Get(item.BaseItemId);

            IconSprite = record.Sprite;

            Rerity = item.Rarity;
        }
    }
}