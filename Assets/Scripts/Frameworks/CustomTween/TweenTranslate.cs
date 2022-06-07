#define DISABLE_REVERSE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace Frameworks.Tween {

    public class TweenTranslate : TweenBase {

        /// <summary>
        /// 初期値
        /// </summary>
        [SerializeField]
        protected Vector2 from_;
		public Vector2 From { set { from_ = value; } }

        /// <summary>
        /// 最終値
        /// </summary>
        [SerializeField]
        protected Vector2 to_;
		public Vector2 To { set { to_ = value; } }

        /// <summary>
        /// 初期化
        /// </summary>
        protected override void Init() {

#if !DISABLE_REVERSE
            transform.localPosition = isReverse_ == true ? to_ : from_;
#else
            transform.localPosition = from_;
#endif

            if (IsUseCurve == false) {
                fromTweener_ = transform.DOLocalMove(from_, duration_).SetEase(ease_);
                toTweener_ = transform.DOLocalMove(to_, duration_).SetEase(ease_);
            }
            else if (IsUseCurve == true) {
                fromTweener_ = transform.DOLocalMove(from_, duration_).SetEase(curve_);
                toTweener_ = transform.DOLocalMove(to_, duration_).SetEase(curve_);
            }
            else {
                Debug.LogWarning("easing is not set !!!");
            }
        }
    }
}