#define DISABLE_REVERSE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace Frameworks.Tween {
    public class TweenSpriteRendererColor : TweenBase {
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
        /// スプライトレンダラー
        /// </summary>
        [SerializeField]
        protected SpriteRenderer renderer_ = null;

        /// <summary>
        /// 元の色 停止した際に子の色に戻す
        /// </summary>
        private Color defaultColor_;

        /// <summary>
        /// 初期化
        /// </summary>
        protected override void Init() {

            defaultColor_ = renderer_.color;

#if !DISABLE_REVERSE
            renderer_.color = isReverse_ == true ? to_ : from_;
#else
            renderer_.color = from_;
#endif

            if (IsUseCurve == false) {
                fromTweener_ = renderer_.DOColor(from_, duration_).SetEase(ease_);
                toTweener_ = renderer_.DOColor(to_, duration_).SetEase(ease_);
            }
            else if (IsUseCurve == true) {
                fromTweener_ = renderer_.DOColor(from_, duration_).SetEase(curve_);
                toTweener_ = renderer_.DOColor(to_, duration_).SetEase(curve_);
            }
            else {
                Debug.LogWarning("easing is not set !!!");
            }
        }

        public override void Stop() {
            base.Stop();

            renderer_.color = defaultColor_;
        }
    }
}
