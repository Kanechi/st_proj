using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Frameworks.Tween;
using UnityEngine.UI;

/// <summary>
/// 基底ウィンドウ
///     閉じるボタン
///     Tween
///     開示中かどうかのフラグ
/// </summary>
public abstract class WindowBase : MonoBehaviour {

    /// <summary>
	/// 閉じるボタン
    /// 画像で存在
	/// </summary>
    [SerializeField]
    protected Button closeBtn_ = null;
    public Button CloseBtn { get => closeBtn_; }

    /// <summary>
    /// tween controller
    /// </summary>
    [SerializeField]
    protected TweenController tweenCtrl_ = null;

    /// <summary>
    /// true...開示中
    /// </summary>
    public bool IsOpen { get; set; } = false;

    protected virtual void Start() {

        if (tweenCtrl_ == null) {
            tweenCtrl_ = gameObject.GetComponent<TweenController>();
        }    
    }

    /// <summary>
	/// 開くときの処理
    /// アニメーションが必要なければこのメソッド呼び出しの上に記載すればよい
	/// </summary>
	/// <param name="opened">開いてアニメーションも終了した際の処理</param>
    public virtual bool Open(UnityAction opened = null) {

#if false
        // ローカライズが必要な場合ここでテキストのローカライズを行う
        var tenchodata = TenchoUserDataAggregate.Instance.GetRecord();
        if (tenchodata != null) {
            ChangeTextFont.Instance.ChangeFont(LocalizeFontRecordTable.Instance.GetRecord((uint)tenchodata.LocalizeType).LocalizeFont);
        }
#endif
        if (IsOpen == true)
            return false;
        IsOpen = true;

        gameObject.SetActive(true);

        if (tweenCtrl_ != null) {
            tweenCtrl_.Play("Open", () => {
                opened?.Invoke();
            });
        }
        else {
            opened?.Invoke();
        }
        return true;
    }

    /// <summary>
	/// 閉じるときの処理
	/// </summary>
	/// <param name="closed"></param>
    public virtual void Close(UnityAction closed = null) {
        if (IsOpen == false)
            return;
        IsOpen = false;

        //TouchCommandManager.Instance.EnableFlag.Up(eEnableTouchFlag.Scroll);

        if (tweenCtrl_ != null) {
            tweenCtrl_.Play("Close", () => {
                closed?.Invoke();
                gameObject.SetActive(false);
            });
        }
        else {
            closed?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
