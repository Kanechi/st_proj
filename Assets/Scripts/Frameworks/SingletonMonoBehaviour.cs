using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>, new()
{
    private static T s_instance_;

    public static T Instance
    {
        get
        {
            // 既に存在
            if (s_instance_ != null)
                return s_instance_;

            // hierarchy に存在
            s_instance_ = (T)FindObjectOfType(typeof(T));

            if (s_instance_ != null)
                return s_instance_;

            // どこにも存在していない
            Create();

            return s_instance_;
        }
    }

    public static T Create()
    {
        // クラス名で生成
        GameObject obj = new GameObject(nameof(T));
        s_instance_ = obj.AddComponent<T>();
        return s_instance_;
    }

    protected virtual void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
