using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// アセット管理
/// </summary>
public class AssetManager : Singleton<AssetManager> {

    // リソースマップ
    private Dictionary<string, AsyncOperationHandle> m_map = new Dictionary<string, AsyncOperationHandle>();

    /// <summary>
    /// リソースの取得
    /// </summary>
    public Ty Get<Ty>(string path) where Ty : UnityEngine.Object {
        if (m_map.ContainsKey(path) == false)
            return null;
        return m_map[path].Result as Ty;
    }

    /// <summary>
    /// リソースのクリア
    /// </summary>
    public void Clear() {
        foreach (var handle in m_map)
            Addressables.Release(handle.Value);
        m_map.Clear();
    }

    public void Unload(string path) {
        if (m_map.ContainsKey(path) == false)
            return;
        Addressables.Release(m_map[path]);
        m_map.Remove(path);
    }

    /// <summary>
    /// 非同期読み込み
    /// Addressable Asset System のみ
    /// </summary>
    /// <typeparam name="Ty"></typeparam>
    /// <param name="path"></param>
    /// <param name="completed"></param>
    public async UniTask LoadAsync<Ty>(string path, UnityAction<Ty> completed = null) where Ty : UnityEngine.Object {

        var res = Get<Ty>(path);

        if (res != null)
            completed?.Invoke(res);

        var handle = Addressables.LoadAssetAsync<Ty>(path);

        await handle.Task;

        if (handle.IsDone == true && handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null) {
            m_map.Add(path, handle);
            completed?.Invoke(handle.Result);
            return;
        }
    }
}
