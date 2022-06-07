using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メイン使用している UI Canvas を外部シングルトン利用できるようにするためのクラス
/// 2022/02/24
///     ゲームマネージャーに統合
/// </summary>
public class CanvasManager : SingletonMonoBehaviour<CanvasManager> {

    [SerializeField]
    private Canvas m_canvas;
    public Canvas Current => m_canvas;

    public void Clear() {
        m_canvas = null;
    }

    /// <summary>
    /// ワールド座標からキャンバス座標に変換
    /// </summary>
    /// <param name="canvasTransform">変換するキャンバスのトランスフォーム</param>
    /// <returns></returns>
    public Vector2 ChangePosFromWorldToCanvas(Vector3 worldPos) {

        var viewportPt = Camera.main.WorldToViewportPoint(worldPos);

        var screenSize = Current.GetComponent<RectTransform>().sizeDelta;

        // 位置サイズをスクリーン座標として計算
        viewportPt.x = (viewportPt.x - 0.5f) * screenSize.x;
        viewportPt.y = (viewportPt.y - 0.5f) * screenSize.y;

        return viewportPt;
    }
}
