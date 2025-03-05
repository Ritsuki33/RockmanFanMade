public class Singleton<T> where T : class, new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    /// <summary>
    /// 明示的にインスタンスを作成
    /// </summary>
    public static void CreateInstance()
    {
        if (instance == null)
        {
            instance = new T();
        }
    }

    public static void DestroyInstance()
    {
        instance = null;
    }

    protected Singleton()
    {
        if (instance != null)
        {
            throw new System.Exception($"An instance of {typeof(T)} already exists.");
        }
    }
}
