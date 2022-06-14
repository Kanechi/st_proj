
/// <summary>
/// リクエスト用レコードテーブル解析用ビルダーインターフェース
/// </summary>
/// <typeparam name="RecordT"></typeparam>
public abstract class BaseRecordTableESBuilder<RecordT> {

    static protected string ES_FILE_PATH = "";

    protected string m_saveKey;

    protected BaseRecordTableESBuilder(string key) {
        m_saveKey = key;
    }

    public abstract IRecordTable<RecordT> GetResult();
    public abstract bool Process();
}

/// <summary>
/// Easy Save ロード
/// </summary>
/// <typeparam name="RecordT"></typeparam>
public class ESLoadBuilder<RecordT, TableT> : BaseRecordTableESBuilder<RecordT>
    where TableT : IRecordTable<RecordT>, new () {

    protected IRecordTable<RecordT> m_recordTable = null;

    public ESLoadBuilder(string key) : base(key) { }

    public override IRecordTable<RecordT> GetResult() { return m_recordTable; }

    public override bool Process() {

#if false
        // キーが存在しなかったら false
        if (ES3.KeyExists(m_saveKey, ES_FILE_PATH) == false)
            return false;
        
        // キーが存在したら読み込み
        var array = ES3.Load<RecordT[]>(m_saveKey, ES_FILE_PATH);

        m_recordTable = new TableT();
        m_recordTable.RecordList.AddRange(array);
#endif

        return true;
    }
}

/// <summary>
/// Easy Save セーブ
/// </summary>
/// <typeparam name="RecordT"></typeparam>
public class ESSaveBuilder<RecordT> : BaseRecordTableESBuilder<RecordT> {

    protected IRecordTable<RecordT> m_recordTable = null;

    public ESSaveBuilder(string key, IRecordTable<RecordT> table) : base(key) {
        m_recordTable = table;
    }

    public override IRecordTable<RecordT> GetResult() { return m_recordTable; }

    public override bool Process() {

#if false
        ES3.Save<RecordT[]>(m_saveKey, m_recordTable.RecordList.ToArray(), ES_FILE_PATH);
#endif

        return true;
    }
}

/// <summary>
/// Easy Save 用 ディレクター
/// </summary>
/// <typeparam name="RecordT"></typeparam>
public class RecordTableESDirector<RecordT> {

    private BaseRecordTableESBuilder<RecordT> m_builder = null;

    public RecordTableESDirector(BaseRecordTableESBuilder<RecordT> builder) => m_builder = builder;

    public IRecordTable<RecordT> GetResult() => m_builder.GetResult();

    public void Construct() {

        m_builder.Process();
    }
}
