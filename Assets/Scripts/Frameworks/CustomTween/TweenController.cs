using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using System.Linq;

namespace Frameworks.Tween {

    /// <summary>
    /// 複数の TweenBase を管理
    /// オブジェクトに取り付けられている Tween をすべて管理
    /// </summary>
    public class TweenController : MonoBehaviour {

        /// <summary>
        /// コンポーネントとして取り付けられている Tween のリスト
        /// </summary>
        //private Dictionary<string, TweenBase> tweenDict_ = new Dictionary<string, TweenBase>();

        private List<TweenBase> tweenList_ = new List<TweenBase>();

        public int TweenCount => tweenList_.Count;

        public void Setup() {

            var tweens = GetComponents<TweenBase>();

            foreach (var tween in tweens) {

                // 同じ名前(ID)も取り付け可能
                tweenList_.Add(tween);
            }
        }

        /// <summary>
        /// Tween の再生
        /// </summary>
        /// <param name="name">識別名</param>
        /// <param name="finished">アニメーション終了時コールバック</param>
        public void Play(string name, UnityAction finished = null) {

            var tweenPlayList = tweenList_.Where(t => t.Name == name).ToList() ;

            TweenBase callbackTarget = null;
            if (tweenPlayList.Count > 0) {
                callbackTarget = tweenPlayList.OrderByDescending(t => t.Duration).First();
            }

            if (tweenPlayList == null)
                Debug.LogWarningFormat("TWEEN ERROR: animation = {0} is none !!!", name);

            if (tweenPlayList.Count == 0)
                Debug.LogWarningFormat("TWEEN ERROR: animation = {0} is none !!!", name);

            foreach (var tween in tweenPlayList) {
                if (tween.IsPlaying() == true)
                    continue;

                if (ReferenceEquals(callbackTarget, tween)) {
                    tween.Play(finished);
                }
                else {
                    tween.Play(null);
                }
            }
        }

        public void StopAll() {

            if (tweenList_ == null)
                return;
            if (tweenList_.Count == 0)
                return;

            foreach (var tween in tweenList_) {
                if (tween.IsPlaying() == true)
                    continue;
                tween.Stop();
            }
        }

        /// <summary>
        /// Tween の停止
        /// </summary>
        /// <param name="name"></param>
        public void Stop(string name) {

            var tweenPlayList = tweenList_.Where(t => t.Name == name).ToList();

            if (tweenPlayList == null)
                Debug.LogWarningFormat("TWEEN ERROR: animation = {0} is none !!!", name);

            if (tweenPlayList.Count == 0)
                Debug.LogWarningFormat("TWEEN ERROR: animation = {0} is none !!!", name);

            foreach (var tween in tweenPlayList) {
                if (tween.IsPlaying() == true)
                    continue;
                tween.Stop();
            }
        }

        /// <summary>
        /// TweenBase の取得
        /// 重複名は対応していないので注意
        /// </summary>
        /// <param name="name">識別名</param>
        /// <returns></returns>
        public TweenBase GetTween(string name) => tweenList_.Find(t => t.Name == name);
    }
}
