using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using UniRx;
using System.Collections;

using UnityEngine.Networking;

using MiniJSON;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Sirenix.OdinInspector;


#if UNITY_EDITOR
/// <summary>
/// 基本となるレコードテーブルエディター
/// </summary>
/// <typeparam name="TableT"></typeparam>
public abstract class BaseRecordTableEditor<TableT> : Editor
{
    protected TableT targetTable_;

    static string s_labelString = @"
下記のボタンを使用してください。
Save した場合本データと差異が生じるので注意して下さい。
読み込んだデータをデバッグ用途でカスタマイズする際は
";

    public override void OnInspectorGUI()
    {
        OnInspectorHeaderGUI();

        GUILayout.Space(5);

        GUILayout.Label(s_labelString);

        GUILayout.Space(5);

        // アセットの保存
        if (GUILayout.Button("Save Asset"))
            EditorUtility.SetDirty(target);

        using (new EditorGUILayout.HorizontalScope())
        {
            // アセットの追加
            if (GUILayout.Button("Add"))
                OnAddButton();
            // アセットのクリア
            else if (GUILayout.Button("Clear"))
                OnClearButton();
            // アセットの削除
            else if (GUILayout.Button("Remove"))
                OnRemoveButton();
        }

        base.OnInspectorGUI();
    }

    protected abstract void OnInspectorHeaderGUI();
    protected abstract bool OnImportButton(string text);
    protected abstract void OnAddButton();
    protected abstract void OnClearButton();
    protected abstract void OnRemoveButton();
}
#endif


#if UNITY_EDITOR

/// <summary>
/// エクセルもしくは CSV からのコピペによるレコードテーブル作成用エディター
/// </summary>
/// <typeparam name="TableT"></typeparam>
public abstract class BaseCopyAndPastRecordTableEditor<TableT> : BaseRecordTableEditor<TableT>
{
    public enum eImportType
    {
        None,
        Excel,
        CSV,
    }
    protected eImportType importType_ = eImportType.None;

    protected override void OnInspectorHeaderGUI()
    {

        GUILayout.Label(
            "Excel もしくは CSV からクリップボードにコピーしたデータを\n" +
            "ダイレクトでインポートします。"
        );

        GUILayout.Space(5);

        importType_ = (eImportType)EditorGUILayout.EnumPopup("Import Type", (eImportType)importType_);

        if (GUILayout.Button("Import"))
        {
            if (OnImportButton(GUIUtility.systemCopyBuffer))
                Debug.Log("Import success");
        }
    }
}

public abstract class CopyAndPastRecordTableEditor<RecordT> : BaseCopyAndPastRecordTableEditor<EditorRecordTable<RecordT>> where RecordT : IJsonParser, new()
{
    private char[] typeTable_ = new char[] { ' ', '\t', ',' };
    protected override bool OnImportButton(string text)
    {
        if (importType_ == eImportType.None)
        {
            Debug.Log("import type is none !!!");
            return false;
        }
        if (targetTable_ != null && targetTable_.RecordList.Count() > 0)
            targetTable_.RecordList.Clear();
        List<string> rows = new List<string>(text.Replace("\\n", "\n").Split('\n'));
        if (importType_ == eImportType.Excel)
        {
            rows.Remove(rows.Last());
        }
        string key = rows.First();
        char delim = typeTable_[(int)importType_];
        var keys = key.Split(delim);
        keys[keys.Count() - 1] = keys[keys.Count() - 1].Replace("\r", "");
        rows.Remove(rows.First());
        foreach (var values in rows)
        {
            var vals = values.Split(delim);
            vals[vals.Count() - 1] = vals[vals.Count() - 1].Replace("\r", "");
            Dictionary<string, object> inst = new Dictionary<string, object>();
            for (int i = 0; i < vals.Count(); ++i)
            {
                inst.Add(keys[i], vals[i].ToString());
            }
            var data = new RecordT();
            data.Parse(inst);
            this.targetTable_.RecordList.Add(data);
        }
        return true;
    }

    protected override void OnAddButton() => this.targetTable_.RecordList.Add(new RecordT());

    protected override void OnClearButton()
    {
        if (this.targetTable_.RecordList.Count == 0)
            return;
        this.targetTable_.RecordList.Clear();
    }

    protected override void OnRemoveButton()
    {
        if (this.targetTable_.RecordList.Count == 0)
            return;
        this.targetTable_.RecordList.Remove(this.targetTable_.RecordList.Last());
    }
}
#endif




#if UNITY_EDITOR
/// <summary>
/// 静的アクセス用レコードテーブルエディター
/// </summary>
/// <typeparam name="TableT"></typeparam>
public abstract class BaseStaticAccessRecordTableEditor<TableT> : BaseRecordTableEditor<TableT>
{
    private bool isTouchImportBtn_ = false;
    protected abstract string Url { get; set; }
    private IEnumerator enumerator_ = null;
    private IEnumerator Request()
    {

        using (UnityWebRequest request = UnityWebRequest.Get(Url))
        {
            yield return request.SendWebRequest();
            while (request.isDone == false)
                yield return 0;
            while (request.downloadHandler.isDone == false)
                yield return 0;
            var responseText = "";
            do
            {
                responseText = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                yield return 0;
            } while (string.IsNullOrEmpty(responseText));
            OnImportButton(responseText);
            enumerator_ = null;
            isTouchImportBtn_ = false;
        }
    }

    protected override void OnInspectorHeaderGUI()
    {
        GUILayout.Label("外部 json を解析して、アセットデータとして作成します。");

        GUILayout.Space(5);
        if (enumerator_ != null)
            enumerator_.MoveNext();

        EditorGUI.BeginDisabledGroup(enumerator_ != null);
        Url = GUILayout.TextArea(Url);

        if (GUILayout.Button("Import"))
        {
            if (isTouchImportBtn_ == true)
                return;
            isTouchImportBtn_ = true;
            enumerator_ = Request();
        }

        EditorGUI.EndDisabledGroup();
    }
}

public abstract class StaticAccessRecordTableEditor<RecordT> : BaseStaticAccessRecordTableEditor<EditorRecordTable<RecordT>>
    where RecordT : IJsonParser, new()
{
    protected abstract IList<RecordT> Parse(Dictionary<string, object> response);

    protected override bool OnImportButton(string text)
    {
        if (this.targetTable_.RecordList.Count > 0)
            this.targetTable_.RecordList.Clear();
        var response = Json.Deserialize(text) as Dictionary<string, object>;
        IList<RecordT> responseData = Parse(response);
        foreach (var data in responseData)
        {
            this.targetTable_.RecordList.Add(data);
        }
        EditorUtility.SetDirty(targetTable_);
        return true;
    }

    protected override void OnAddButton() => this.targetTable_.RecordList.Add(new RecordT());

    protected override void OnClearButton()
    {
        if (this.targetTable_.RecordList.Count == 0)
            return;
        this.targetTable_.RecordList.Clear();
    }

    protected override void OnRemoveButton()
    {
        if (this.targetTable_.RecordList.Count == 0)
            return;
        this.targetTable_.RecordList.Remove(this.targetTable_.RecordList.Last());
    }
}

#endif
