using Framework.CommonUI;
using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework {

    /// <summary>
    /// モーダルウィンドウ
    /// 汎用型の確認ウィンドウ
    /// ここでいろいろなタイプを設定しようかと思ったが
    /// それぞれの場面で設定したほうが良いかな。
    /// 結局ポップアップは必要だし。
    /// </summary>
    public class ModalWindow : WindowBase {

        /// <summary>
        /// タイトル部分
        /// </summary>
        [Serializable]
        public class TitleObject {
            // 本体
            public GameObject origin_ = null;
            // テキスト
            public Text titleText_ = null;
            // 背景
            public Image titleBg_ = null;
        }
        public TitleObject titleObj_ = new TitleObject();

        /// <summary>
        /// メイン部分
        /// </summary>
        [Serializable]
        public class MainFrameObject {
            // 本体
            public GameObject origin_ = null;
            // ロゴ画像
            public Image logoImage_ = null;
            // 上部説明文
            public Text upperExplanationText_ = null;
            // アイコン画像
            public Image iconImage_ = null;
            // 下部説明文
            public Text underExplanationText_ = null;
            // アイテム数テキスト
            public Text itemHoldText_ = null;
        }
        public MainFrameObject frameObj_ = new MainFrameObject();

        /// <summary>
        /// ボタン部分
        /// １つボタンの場合は rightBtn_ と rightBtnText_ の SetActive を false に設定し
        /// コールバックを leftBtn_ に設定する
        /// </summary>
        [Serializable]
        public class ButtonObject {
            // 本体
            public GameObject origin_ = null;
            // 左ボタン
            public CustomButton leftBtn_ = null;
            // 右ボタン
            public CustomButton rightBtn_ = null;
            // 左ボタンテキスト
            public TextMeshProUGUI leftBtnText_ = null;
            // 右ボタンテキスト
            public TextMeshProUGUI rightBtnText_ = null;
        }
        public ButtonObject btnObj_ = new ButtonObject();

        private UnityAction leftBtnEvent_ = null;
        public UnityAction LeftBtnEvent { set => leftBtnEvent_ = value; }

        private UnityAction rightBtnEvent_ = null;
        public UnityAction RightBtnEvent { set => rightBtnEvent_ = value; }

        private void Awake() {

            closeBtn_.gameObject.SetActive(false);

            btnObj_.leftBtn_.PointerClick = (data) => { leftBtnEvent_?.Invoke(); };

            btnObj_.rightBtn_.PointerClick = (data) => { rightBtnEvent_?.Invoke(); };
        }

        public void Open(UnityAction<ModalWindow> opened) {

            opened?.Invoke(this);

            base.Open();
        }
    }
}