using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    public class SfProductionResourceCellData
    {
        // �A�C�R���摜
        public Sprite IconSprite { get; private set; }



        // ���A�x������
        public eRarity Rerity { get; private set; }

        public SfProductionResourceCellData(SfProductionResourceItem item)
        {
            var record = SfProductionResourceTableManager.Instance.Get(item.BaseItemId);

            IconSprite = record.Sprite;

            Rerity = item.Rarity;
        }
    }
}