using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace Frameworks.Tween {

    [RequireComponent(typeof(CanvasGroup))]
    public class TweenAlpha : TweenBase {
        /// <summary>
        /// 初期値
        /// </summary>
        [SerializeField]
        protected float from_;

        /// <summary>
        /// 最終値
        /// </summary>
        [SerializeField]
        protected float to_;

        /// <summary>
        /// マテリアル
        /// </summary>
        [SerializeField]
        protected CanvasGroup group_;

        /// <summary>
        /// 元の色 停止した際に子の色に戻す
        /// </summary>
        private float defaultAlpha_;

        /// <summary>
        /// 初期化
        /// </summary>
        protected override void Init() {

            if (group_ == null)
                group_ = GetComponent<CanvasGroup>();

            group_.alpha = from_;

            if (IsUseCurve == false) {


                fromTweener_ = group_.DOFade(from_, duration_).SetEase(ease_);
                toTweener_ = group_.DOFade(to_, duration_).SetEase(ease_);
            }
            else if (IsUseCurve == true) {
                fromTweener_ = group_.DOFade(from_, duration_).SetEase(curve_);
                toTweener_ = group_.DOFade(to_, duration_).SetEase(curve_);
            }
            else {
                Debug.LogWarning("easing is not set !!!");
            }
        }
    }
}