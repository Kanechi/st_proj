using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// レコードテーブル基底
/// </summary>
/// <typeparam name="RecordT"></typeparam>
public interface IRecordTable<RecordT>
{
    List<RecordT> RecordList { get; }

    RecordT Get(uint key);
}

/// <summary>
/// レコードテーブル
/// </summary>
/// <typeparam name="RecordT"></typeparam>
public abstract class RecordTable<RecordT> : IRecordTable<RecordT> {
    [SerializeField]
    protected List<RecordT> m_recordList = new List<RecordT>();

    public List<RecordT> RecordList => m_recordList;

    public abstract RecordT Get(uint key);
}

/// <summary>
/// エディター用レコードテーブル
/// </summary>
/// <typeparam name="RecordT"></typeparam>
public abstract class EditorRecordTable<RecordT> : ScriptableObject, IRecordTable<RecordT> {
    
    [SerializeField]
    protected List<RecordT> m_recordList = new List<RecordT>();

    public List<RecordT> RecordList => m_recordList;

    public abstract RecordT Get(uint key);
}