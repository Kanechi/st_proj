using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace Frameworks.Tween {
    public class TweenMatColor : TweenBase {

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

        [SerializeField]
        protected SpriteRenderer renderer_ = null;

        /// <summary>
        /// マテリアル
        /// </summary>
        //[SerializeField]
        protected Material mat_ = null;

        /// <summary>
        /// 元の色 停止した際に子の色に戻す
        /// </summary>
        private Color defaultColor_;

        /// <summary>
        /// 初期化
        /// </summary>
        protected override void Init() {

            mat_ = renderer_.material;

            defaultColor_ = mat_.color;

            mat_.color = from_;

            if (IsUseCurve == false) {
                fromTweener_ = mat_.DOColor(from_, duration_).SetEase(ease_);
                toTweener_ = mat_.DOColor(to_, duration_).SetEase(ease_);
            }
            else if (IsUseCurve == true) {
                fromTweener_ = mat_.DOColor(from_, duration_).SetEase(curve_);
                toTweener_ = mat_.DOColor(to_, duration_).SetEase(curve_);
            }
            else {
                Debug.LogWarning("easing is not set !!!");
            }
        }

        public override void Stop() {
            base.Stop();

            mat_.color = defaultColor_;
        }
    }
}