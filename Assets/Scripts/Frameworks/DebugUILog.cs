using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUILog : MonoBehaviour
{
    /// <summary>
    /// true...改行あり
    /// デフォルト true
    /// </summary>
    [SerializeField]
    private bool isNewLine_ = true;

    /// <summary>
    /// 表示するテキスト
    /// </summary>
    [SerializeField]
    private Text text_ = null;

    private void Awake()
    {
        Application.logMessageReceived += OnLogMessage;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived += OnLogMessage;
    }

    private void OnLogMessage(string i_logText, string i_stackTrace, LogType i_type)
    {
        if (string.IsNullOrEmpty(i_logText))
        {
            return;
        }

        if (text_ == null)
            return;

        switch (i_type)
        {
            case LogType.Error:
            case LogType.Assert:
            case LogType.Exception:
                i_logText = string.Format("<color=red>{0}</color>", i_logText);
                break;
            case LogType.Warning:
                i_logText = string.Format("<color=yellow>{0}</color>", i_logText);
                break;
            default:
                break;
        }

        if (isNewLine_ == true)
        {

            var text = text_.text;

            text_.text = "";

            text_.text = i_logText + System.Environment.NewLine + text;
        }
        else
        {
            text_.text = i_logText;
        }
    }
}
