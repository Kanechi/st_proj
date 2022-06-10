using Frameworks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ポップアップウィンドウ基底
/// 
/// ちょい古いので使用は無しで
/// </summary>
public class BasePopupWindow : MonoBehaviour
{
    // true...ウィンドウを開いている
    public bool IsOpen { get; set; } = false;

    // 開いた際の処理
    public UnityAction OpendCallback { get; set; } = null;

    // 閉じた際の処理
    public UnityAction ClosedCallback { get; set; } = null;

    // tween アニメーション管理「
    private TweenController m_tweenCtrl = null;

    private void Start()
    {
        m_tweenCtrl = GetComponent<TweenController>();

        m_tweenCtrl.Setup();
    }

    /// <summary>
    /// 開く処理
    /// </summary>
    protected virtual bool Open() {

        if (IsOpen == true)
            return false;
        IsOpen = true;

        // アニメーションがある場合
        if (m_tweenCtrl != null && m_tweenCtrl.TweenCount > 0) {
            m_tweenCtrl.Play("Opened", () => OpendCallback?.Invoke());
            return true;
        }

        OpendCallback?.Invoke();

        return true;
    }

    /// <summary>
    /// 閉じる処理
    /// </summary>
    public virtual void Close() {

        if (IsOpen == false)
            return;
        IsOpen = false;

        ClosedCallback?.Invoke();

        // アニメーションがある場合
        if (m_tweenCtrl != null && m_tweenCtrl.TweenCount > 0) {
            m_tweenCtrl.Play("Closed");
            return;
        }
    }
}