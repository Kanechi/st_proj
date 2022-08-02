using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{

    /// <summary>
    /// 王国生成工場基底
    /// </summary>
    public abstract class SfKingdomFactoryBase
    {
        public SfKingdom Create(int uniqueId)
        {
            var record = CreateRecord();

            // ユニーク ID 設定
            record.Id = uniqueId;

            // 王国名設定
            CreateName(record);

            // 王国カラー設定
            SettingColor(record);

            // 自分の国かどうかのフラグの設定
            SettingSelfFlag(record);

            return record;
        }

        // レコード生成
        protected abstract SfKingdom CreateRecord();

        // 王国名生成
        protected abstract void CreateName(SfKingdom record);
        // 王国カラーの設定
        protected abstract void SettingColor(SfKingdom record);

        protected abstract void SettingSelfFlag(SfKingdom record);
    }

    /// <summary>
    /// 王国生成工場
    /// </summary>
    public abstract class SfKingdomFactory : SfKingdomFactoryBase
    {
        protected override SfKingdom CreateRecord()
        {
            return new SfKingdom();
        }
    }

    /// <summary>
    /// 自国の生成
    /// 自国の生成はゲーム開始前に設定した項目を設定
    /// </summary>
    public class SfSelfKingdomFactory : SfKingdomFactory
    {
        // 王国名生成
        protected override void CreateName(SfKingdom record)
        {
            record.Name = SfConfigController.Instance.KingdomName;
        }

        // 王国カラーの設定
        protected override void SettingColor(SfKingdom record)
        {
            record.Color = SfConfigController.Instance.KingdomColor;
        }

        // 自分の国かどうかのフラグ
        protected override void SettingSelfFlag(SfKingdom record)
        {
            record.SelfFlag = true;
        }
    }


    // その他の国のランダム生成
    // ある程度の国を事前に作成しておいて割り振るだけに
    // とどめるか、すべて０から作成するか・・・
    public class SfOtherKingdomFactory : SfKingdomFactory
    {
        // 王国名生成
        protected override void CreateName(SfKingdom record)
        {
            // ランダム生成
            record.Name = "test";
        }

        // 王国カラーの設定
        protected override void SettingColor(SfKingdom record)
        {
            record.Color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 0.6f);
        }

        // 自分の国かどうかのフラグ
        protected override void SettingSelfFlag(SfKingdom record)
        {
            record.SelfFlag = false;
        }
    }

    /// <summary>
    /// 王国工場管理
    /// </summary>
    public class SfKingdomFactoryManager : Singleton<SfKingdomFactoryManager>
    {
        public SfKingdom Create(bool selfKingdom)
        {
            SfKingdomFactoryBase factory = null;

            if (selfKingdom)
            {
                factory = new SfSelfKingdomFactory();
            }
            else
            {
                factory = new SfOtherKingdomFactory();
            }

            // 王国レコードの生成
            var record = factory.Create((int)SfConstant.CreateUniqueId(ref SfKingdomTableManager.Instance.m_uniqueIdList));

            return record;
        }
    }
}