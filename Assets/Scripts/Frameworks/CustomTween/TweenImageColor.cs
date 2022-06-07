#define DISABLE_REVERSE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace Frameworks.Tween {
    public class TweenImageColor : TweenBase {

        /// <summary>
        /// 初期値
        /// </summary>
        [SerializeField]
        protected Color from_;

        /// <summary>
        /// 最終値
        /// </summary>
        [SerializeField]
        protected Color to_;

        /// <summary>
        /// マテリアル
        /// </summary>
        [SerializeField]
        protected Image image_ = null;

        /// <summary>
        /// 元の色 停止した際に子の色に戻す
        /// </summary>
        private Color defaultColor_;

        /// <summary>
        /// 初期化
        /// </summary>
        protected override void Init() {

            defaultColor_ = image_.color;

#if !DISABLE_REVERSE
            image_.color = isReverse_ == true ? to_ : from_;
#else
            image_.color = from_;
#endif

            if (IsUseCurve == false) {
                fromTweener_ = image_.DOColor(from_, duration_).SetEase(ease_);
                toTweener_ = image_.DOColor(to_, duration_).SetEase(ease_);
            }
            else if (IsUseCurve == true) {
                fromTweener_ = image_.DOColor(from_, duration_).SetEase(curve_);
                toTweener_ = image_.DOColor(to_, duration_).SetEase(curve_);
            }
            else {
                Debug.LogWarning("easing is not set !!!");
            }
        }

        public override void Stop() {
            base.Stop();

            image_.color = defaultColor_;
        }
    }
}