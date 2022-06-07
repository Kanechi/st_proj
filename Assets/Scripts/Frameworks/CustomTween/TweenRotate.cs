#define DISABLE_REVERSE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace Frameworks.Tween {
    public class TweenRotate : TweenBase {
        /// <summary>
        /// 初期値
        /// </summary>
        [SerializeField]
        protected Vector3 from_;

        /// <summary>
        /// 最終値
        /// </summary>
        [SerializeField]
        protected Vector3 to_;

        /// 初期化
        /// <summary>
        /// </summary>
        protected override void Init() {

#if !DISABLE_REVERSE
            transform.localEulerAngles = isReverse_ == true ? to_ : from_;
#else
            transform.localEulerAngles = from_;
#endif

            if (IsUseCurve == false) {
                fromTweener_ = transform.DORotate(from_, duration_).SetEase(ease_);
                toTweener_ = transform.DORotate(to_, duration_).SetEase(ease_);
            }
            else if (IsUseCurve == true) {
                fromTweener_ = transform.DORotate(from_, duration_).SetEase(curve_);
                toTweener_ = transform.DORotate(to_, duration_).SetEase(curve_);
            }
            else {
                Debug.LogWarning("easing is not set !!!");
            }
        }
    }
}