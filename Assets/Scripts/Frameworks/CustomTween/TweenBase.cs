#define DISABLE_REVERSE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.Events;

namespace Frameworks.Tween {

    /// <summary>
    /// DOTween をラップして Hierarchy である程度簡単な動きを作成できるように実装
    /// </summary>
    public abstract class TweenBase : MonoBehaviour {

        /// <summary>
        /// 開始時間が 0 だと不具合が発生したので微小数を設定しておく
        /// </summary>
        static protected float FIX_FROM_DURATION = 0.0001f;

        /// <summary>
        /// true...Start() 時に起動
        /// </summary>
        [SerializeField]
        protected bool atStart_ = false;
        public bool AtStart => atStart_;

        /// <summary>
        /// 識別名
        /// なんのための Tween なのかがわからないので識別名兼役割として設定
        /// </summary>
        [SerializeField]
        protected string name_ = string.Empty;
        public string Name => name_;

        /// <summary>
        /// true...無限ループ
        /// </summary>
        [SerializeField]
        protected bool isLoop_ = false;

#if !DISABLE_REVERSE
        // 現状不安定
        /// <summary>
        /// true...逆再生
        /// </summary>
        [SerializeField]
        protected bool isReverse_ = false;
#endif

        /// <summary>
        /// DOTween の Ease か AnimationCurve のどちらか設定されているほうでイージング処理を行う
        /// 両方設定されている場合優先は DOTween の Ease
        /// </summary>
        [SerializeField]
        protected Ease ease_ = Ease.Unset;

        [SerializeField]
        protected AnimationCurve curve_ = null;

        /// <summary>
        /// true...Ease が設定されておらず AnimationCurve が設定されている
        /// </summary>
        protected bool IsUseCurve => ease_ == Ease.Unset && curve_ != null;

        /// <summary>
        /// アニメーション時間
        /// </summary>
        [SerializeField]
        protected float duration_;
        public float Duration { get => duration_; }

        /// <summary>
        /// tween sequence
        /// </summary>
        protected Sequence sequence_ = null;

        /// <summary>
        /// 開始 Tweener
        /// </summary>
        protected Tweener fromTweener_ = null;

        /// <summary>
        /// 終了 Tweener
        /// </summary>
        protected Tweener toTweener_ = null;

        /// <summary>
        /// アニメーション終了時呼び出し
        /// </summary>
        protected UnityAction finishedCallback_ = null;
        public UnityAction Finished { set { finishedCallback_ = value; } }
        
        protected virtual void Start() {

            // オブジェクト生成とともに Tween を起動
            if (atStart_ == true) {
                Create();
            }
        }

#if false
        protected virtual void OnDestroy() {

            if (sequence_ != null)
                sequence_ = null;

            finishedCallback_ = null;
        }
#endif
        private void OnDisable() {
            // Tween破棄
            if (DOTween.instance != null) {
                sequence_?.Kill();
            }
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        protected virtual void Update() { }

        /// <summary>
        /// 初期化処理
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// 生成処理
        /// </summary>
        private void Create() {

            Init();

#if !DISABLE_REVERSE
            if (isReverse_ == false) {
                // 通常再生シーケンスの作成
                CreateSequence();
            }
            else {
                CreateReverseSequence();
            }
#else

            CreateSequence();

#endif

        }

        /// <summary>
        /// 通常再生シーケンスの作成
        /// </summary>
        protected virtual void CreateSequence() {
            sequence_ = DOTween.Sequence().Append(
                    toTweener_
                ).OnComplete(() => {
                    finishedCallback_?.Invoke();
                }).SetLoops(
                    isLoop_ == true ? -1 : 0
                );
        }

        /// <summary>
        /// 通常再生あと逆再生するシーケンスを作成
        /// </summary>
        protected virtual void CreateReverseSequence() {
            sequence_ = DOTween.Sequence().Append(
                    fromTweener_
                ).OnComplete(() => {
                    finishedCallback_?.Invoke();
                }).SetLoops(
                    isLoop_ == true ? -1 : 0
                );
        }

        /// <summary>
        /// アニメーションがプレイ中かどうか
        /// </summary>
        /// <returns>true...プレイ中</returns>
        public bool IsPlaying() {
            if (sequence_ != null && sequence_.IsActive() == true) {
                if (sequence_.IsPlaying() == true) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 任意再生
        /// </summary>
        /// <param name="finished"></param>
        public virtual void Play(UnityAction finished) {

            //gameObject.SetActive(true);

            // Start() 時再生でこのメソッドを利用する際に
            // 設定した finishedCallback_ が null で上書きされないように null でないときのみ上書き
            finishedCallback_ = finished;

            if (sequence_ != null) {
                sequence_.Kill();
            }

            // 新規で作り直す
            Create();
        }

        public virtual void Stop() {
            if (sequence_ != null) {
                sequence_.Kill();
            }
        }
    }
}