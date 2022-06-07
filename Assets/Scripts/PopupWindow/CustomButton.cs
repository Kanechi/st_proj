using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using UniRx;
using UniRx.Triggers;

using Frameworks.Tween;

namespace Framework.CommonUI {

    /// <summary>
    /// 通常のボタンから派生して作成する場合は Image は Button の子オブジェクトとして作成される
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class CustomButton : MonoBehaviour {

        /// <summary>
        /// ボタン画像
        /// </summary>
        [SerializeField]
        protected Image image_ = null;
        public Image BtnImage { get => image_; }

        /// <summary>
        /// ボタン本体
        /// </summary>
        [SerializeField]
        protected Button button_ = null;
        public Button Btn { get => button_; }

        /// <summary>
        /// イベントトリガー
        /// タッチ押下、タッチ押上用
        /// </summary>
        protected EventTrigger trigger_ = null;

        /// <summary>
        /// タッチ押下時コールバック setter のみ
        /// </summary>
        protected UnityAction<PointerEventData> onPointerDownCallback_ = null;
        public UnityAction<PointerEventData> PointerDown { set => onPointerDownCallback_ = value; }

        /// <summary>
        /// タッチ押上時コールバック setter のみ
        /// </summary>
        protected UnityAction<PointerEventData> onPointerUpCallback_ = null;
        public UnityAction<PointerEventData> PointerUp { set => onPointerUpCallback_ = value; }

        /// <summary>
        /// クリック時コールバック setter のみ
        /// </summary>
        protected UnityAction<PointerEventData> onPointerClickCallback_ = null;
        public UnityAction<PointerEventData> PointerClick { set => onPointerClickCallback_ = value; }

        /// <summary>
        /// ボタン長押し時のコールバック setter のみ
        /// </summary>
        protected UnityAction onLongTouchCallback_ = null;
        public UnityAction LongTouch { set => onLongTouchCallback_ = value; }

        /// <summary>
        /// 長押し用ステータス
        /// </summary>
        [Serializable]
        public class LongTouchStatus {

            /// <summary>
            /// true...長押し判定あり
            /// </summary>
            public bool enable_ = false;

            /// <summary>
            /// 長押し完了時間
            /// この時間以上なら長押し判定
            /// </summary>
            public float JUDGE_COMP_TIME = 1.0f;

            /// <summary>
            /// 長押し
            /// </summary>
            [HideInInspector]
            public bool isLongTouched_ = false;

            public void Setup(float judgeTime, bool enable = true) {
                enable_ = enable;
                JUDGE_COMP_TIME = judgeTime;
            }
        }
        [SerializeField]
        private LongTouchStatus longTouchSt_ = new LongTouchStatus();
        public LongTouchStatus LongTouchSt { get => longTouchSt_; }

        private TweenController tweenCtrl_ = null;
        public TweenController TweenCtrl { get => tweenCtrl_ != null ? tweenCtrl_ : tweenCtrl_ = GetComponent<TweenController>(); }

        protected virtual void Awake() {

            // SerializeField で設定されていなければ Component として持ってくる
            // 通常の Button から作成している場合 Image が子オブジェクトとして作成されるので
            // それを利用する
            if (image_ == null) {
                image_ = gameObject.GetComponent<Image>();
                if (image_ == null) {
                    image_ = gameObject.GetComponentInChildren<Image>();
                }
            }

            if (button_ == null) {
                button_ = gameObject.GetComponent<Button>();
            }

            if (button_ != null) {
                var nav = button_.navigation;
                nav.mode = Navigation.Mode.None;
                button_.navigation = nav;
            }

            trigger_ = gameObject.GetComponent<EventTrigger>();
        }

        protected virtual void OnDestroy() {

            image_ = null;
            button_ = null;
            trigger_ = null;

            onPointerDownCallback_ = null;
            onPointerUpCallback_ = null;
            onPointerClickCallback_ = null;
            onLongTouchCallback_ = null;
        }

        // Start is called before the first frame update
        protected virtual void Start() {

#if false
            Func<EventTriggerType, UnityAction<UnityAction<BaseEventData>>> listener = (type) => {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = type;
                trigger_.triggers.Add(entry);

                return (callback) => {
                    entry.callback.AddListener(callback);
                };
            };

            var ptDown = listener(EventTriggerType.PointerDown);
            ptDown((data) => { OnPointerDown((PointerEventData)data); });

            var ptUp = listener(EventTriggerType.PointerUp);
            ptUp((data) => { OnPointerUp((PointerEventData)data); });
#endif

            //button_.OnClickAsObservable().Subscribe(_ => OnPointerClick(null));

            button_.OnPointerDownAsObservable().Subscribe(data => OnPointerDown(data));

            button_.OnPointerUpAsObservable().Subscribe(data => OnPointerUp(data));

            button_.OnPointerDownAsObservable()
                .SelectMany(_ => Observable.Interval(TimeSpan.FromSeconds(0.1)))
                .TakeUntil(button_.OnPointerUpAsObservable())
                .DoOnCompleted(() => {

                    if (longTouchSt_.enable_ == false || (longTouchSt_.enable_ == true && longTouchSt_.isLongTouched_ == false)) {
                        OnPointerClick(null);
                        //Debug.Log("released!");
                    }

                    longTouchSt_.isLongTouched_ = false;
                })
                .RepeatUntilDestroy(button_)
                .Subscribe(ct => {

                    //Debug.Log("pressing..." + (ct * 0.5).ToString());

                    if (longTouchSt_.enable_ == true && longTouchSt_.isLongTouched_ == false) {
                        if ((ct * 0.1) >= longTouchSt_.JUDGE_COMP_TIME) {
                            onLongTouchCallback_?.Invoke();
                            longTouchSt_.isLongTouched_ = true;
                        }
                    }
                });
        }

        // Update is called once per frame
        protected virtual void Update() {

#if false
            if (longTouchSt_.enable_ == false)
                return;

            if (longTouchSt_.IsJudgStart == false)
                return;

            if (longTouchSt_.IsLongTouch == true)
                return;

            longTouchSt_.TotalTime += Time.deltaTime;
            if (longTouchSt_.TotalTime >= longTouchSt_.judgeTime_) {
                // 長押し時の処理は成功時一回のみ
                longTouchSt_.IsLongTouch = true;
                onLongTouchCallback_?.Invoke();
            }
#endif
        }

        protected void OnPointerDown(PointerEventData data) {

            TweenCtrl?.Play("Button_Push");

            onPointerDownCallback_?.Invoke(data);
        }

        protected void OnPointerUp(PointerEventData data) {

            onPointerUpCallback_?.Invoke(data);
        }

        protected void OnPointerClick(PointerEventData data) {

            TweenCtrl?.Play("Button_Release");

            onPointerClickCallback_?.Invoke(data);
        }
    }
}