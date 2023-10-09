public class Singleton<T> where T : class, new()
{
    protected static T instance;

    protected Singleton() { }

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }
}
