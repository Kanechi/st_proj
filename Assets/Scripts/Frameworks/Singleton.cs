
public abstract class Singleton<T> where T : Singleton<T>, new()
{
    protected Singleton() { }

    static protected object s_mtx_ = new object();

    static protected volatile T s_instance_ = null;

    static public T Instance 
    {
        get
        {
            lock (s_mtx_) 
            {
                if (s_instance_ == null)
                    s_instance_ = new T();
                return s_instance_;
            }
        }
    }

    static public void Remove()
    {
        lock (s_mtx_)
        {
            if (s_instance_ != null)
                s_instance_ = null;
        }
    }
}
