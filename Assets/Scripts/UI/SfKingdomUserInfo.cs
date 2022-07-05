using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace sfproj
{
    /// <summary>
    /// 自分の王国情報
    /// 相手の情報を見る際は地域情報とか領域情報から見るのでこれではない
    /// 左上の情報
    /// 
    /// </summary>
    public class SfKingdomUserInfo : MonoBehaviour
    {
        // 顔画像
        [SerializeField]
        private Image m_faceImage = null;

        // 人口ゲージ
        [SerializeField]
        private Slider m_popSlider = null;

        // 人口/最大人口テキスト
        [SerializeField]
        private TextMeshProUGUI m_popText = null;

        // 王国レベルテキスト
        [SerializeField]
        private TextMeshProUGUI m_kingdomLvText = null;

        // 王国経験値ゲージ
        [SerializeField]
        private Slider m_expSlider = null;

        // 王国名
        [SerializeField]
        private TextMeshProUGUI m_kingdomName = null;


        // 自分の王国のデータ
        private SfKingdomRecord m_record = null;
        public SfKingdomRecord SelfKingdomRecord => m_record;


        // Start is called before the first frame update
        void Start()
        {
            
            m_record = SfKingdomRecordTableManager.Instance.GetSelfKingdom();

            // 顔画像の設定

            // 王国名の設定
        }

        // Update is called once per frame
        void Update()
        {
#if false
            // 人口の部分は領域 ID から地域 ID までさかのぼって地域情報の人口の合計や
            // 最大人口の合計で計算を行う
            m_popSlider.maxValue = m_record.MaxPopulation;

            m_popSlider.value = m_record.Population;

            m_popText.text = m_record.Population.ToString() + "/" + m_record.MaxPopulation.ToString();
#endif
            // 経験値テーブルから次の経験値を取得して経験値スライダーに設定
            //m_expSlider.maxValue

            // 現在の経験値を設定(累積型)
            m_expSlider.value = m_record.m_kingdomExp;

            // レベルアップはレコード管理で行う
            m_kingdomLvText.text = m_record.m_kingdomLv.ToString();
        }


    }
}