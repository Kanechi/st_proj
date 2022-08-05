using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{
    /// <summary>
    /// 地域に保管されているアイテム１種類分
    /// </summary>
    public class SfStoragedItem : IJsonParser
    {
        // 保管庫のインデックス(上が０)
        // 保管数がなくなったら保管庫から消える、インデックスが繰り下がる
        public int m_index = 0;
        public int Index { get => m_index; set => m_index = value; }

        // 生産されたアイテムのID
        public uint m_itemId = 0;
        public uint ItemId { get => m_itemId; set => m_itemId = value; }

        // 保管数
        public int m_count = 0;
        public int Count { get => m_count; set => m_count = value; }

        // 保管されている地域の ID
        public uint m_areaId = 0;
        public uint AreaId { get => m_areaId; set => m_areaId = value; }

        public SfStoragedItem() { }
        public SfStoragedItem(int index, uint itemId, int count, uint areaId)
        {
            m_index = index;
            m_itemId = itemId;
            m_count = count;
            m_areaId = areaId;
        }

        public void Parse(IDictionary<string, object> data)
        {
            
        }
    }

    /// <summary>
    /// 保管アイテムテーブル
    /// </summary>
    public class SfStoragedItemTable : RecordTable<SfStoragedItem>
    {
        // 未使用
        public override SfStoragedItem Get(uint id) => null;

        // 登録
        public void Regist(SfStoragedItem record) => RecordList.Add(record);

        // 指定地域の一番大きいインデックスの数を返す
        public int GetMaxIndexByArea(uint areaId) {
            if (RecordList.Count == 0)
                return 0;

            var list = RecordList.Where(r => r.AreaId == areaId);

            if (list.Count() == 0)
                return 0;

            return list.Max(r => r.Index);
        }

        /// <summary>
        /// アイテムを増やす
        /// </summary>
        /// <param name="areaId">どの地域の</param>
        /// <param name="itemId">どのアイテムを</param>
        /// <param name="value">いくつ</param>
        public void IncreaseItem(uint areaId, uint itemId, int value)
        {
            SfStoragedItem item = null;
            if (RecordList.Count != 0)
            {
                item = RecordList.Find(r => r.AreaId == areaId && r.ItemId == itemId);
            }

            if (item == null)
            {
                int index = GetMaxIndexByArea(areaId);

                item = new SfStoragedItem(index, itemId, value, areaId);

                Regist(item);
            }
            else
            {
                item.Count += value;
                // スタック(種類最大値)以上になったら切り捨て
                // stellaris ならアイテムごとに最大値があったけど
                // このゲームがアイテムの種類が多いから・・・
                // けどいっぱい手に入れたいしひとまず一律で 999 で
                if (item.Count > 999)
                    item.Count = 999;
            }
        }

        /// <summary>
        /// 減少はチェックが必要かも
        /// 輸送する際とかも・・・０以下は０になるで停止しておけば問題ない？
        /// 輸送もその他の施設と扱いとしては同じかな？
        /// 使用される際に０以下になることで起こるデメリットが発生するような行動が起こらなければ問題ないかも
        /// 例えば施設建設なんかは建設ボタンを押そうとして押せなくするとかで対処
        /// 
        /// ひとまず放置
        /// </summary>
        /// <param name="areaid"></param>
        /// <param name="itemId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckDecreaseItem(uint areaId, uint itemId, int value)
        {
            return true;
        }

        /// <summary>
        /// ひとまず減ることは後回しで
        /// 
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="itemId"></param>
        /// <param name="value"></param>
        public void DecreaseItem(uint areaId, uint itemId, int value)
        {
            
        }
    }

    public class SfStoragedItemTableManager : Singleton<SfStoragedItemTableManager>
    {
        private SfStoragedItemTable m_table = new SfStoragedItemTable();
        public SfStoragedItemTable Table => m_table;
    }
}