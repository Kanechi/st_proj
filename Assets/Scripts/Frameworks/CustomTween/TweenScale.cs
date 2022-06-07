#define DISABLE_REVERSE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace Frameworks.Tween {

    /// <summary>
    /// DOTween を利用した拡大 Tween
    /// </summary>
    public class TweenScale : TweenBase {

        /// <summary>
        /// 初期値
        /// </summary>
        [SerializeField]
        protected Vector2 from_;
        public Vector2 From { get => from_; set => from_ = value; }


        /// <summary>
        /// 最終値
        /// </summary>
        [SerializeField]
        protected Vector2 to_;
        public Vector2 To { get => to_; set => to_ = value; }

        /// <summary>
        /// 初期化
        /// </summary>
        protected override void Init() {

#if !DISABLE_REVERSE
            transform.localScale = isReverse_ == true ? to_ : from_;
#else
            transform.localScale = from_;
#endif

            if (IsUseCurve == false) {
                fromTweener_ = transform.DOScale(from_, duration_).SetEase(ease_);
                toTweener_ = transform.DOScale(to_, duration_).SetEase(ease_);
            }
            else if (IsUseCurve == true) {
                fromTweener_ = transform.DOScale(from_, duration_).SetEase(curve_);
                toTweener_ = transform.DOScale(to_, duration_).SetEase(curve_);
            }
            else {
                Debug.LogWarning("easing is not set !!!");
            }
        }
    }
}