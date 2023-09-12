using UnityEngine;

public abstract class Singleton<T> where T : class, new()
{
    private static T instance = null;

    public static T Instance 
    {
        get
        {
            if (instance == null) 
            {
                instance = new T();
                Singleton<T> singletonObject = instance as Singleton<T>;
                Debug.Log($"Creat Singleton: {singletonObject}");
                singletonObject?.Initialization();
            }

            return instance;
        }
    }

    protected virtual void Initialization() { }
}
