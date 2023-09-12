using UnityEngine;

public abstract class MonoBehaviourSingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        instance = null;
    }



    internal static T instance = null;

    protected static bool IsApplicationQuit { get; private set; } = false;

    protected virtual bool IsHideAndDontSave => false;



    private void OnApplicationQuit()
    {
        IsApplicationQuit = true;
    }

    protected void ObjectSetting()
    {
        DontDestroyOnLoad(gameObject);

        if (IsHideAndDontSave)
        {
            gameObject.hideFlags = HideFlags.HideAndDontSave;
        }
    }
}

public abstract class MonoBehaviourSingleton<T> : MonoBehaviourSingletonBase<T> where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (IsApplicationQuit)
            {
                return null;
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            ObjectSetting();
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
}

public abstract class MonoBehaviourAutoCreateSingleton<T> : MonoBehaviourSingletonBase<T> where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (IsApplicationQuit)
            {
                return null;
            }

            if (instance == null)
            {
                GameObject singletonGameObject = new GameObject();
                singletonGameObject.name = typeof(T).Name;
                instance = singletonGameObject.AddComponent<T>();

                MonoBehaviourAutoCreateSingleton<T> singletonInfo = instance as MonoBehaviourAutoCreateSingleton<T>;
                singletonInfo.ObjectSetting();
            }

            return instance;
        }
    }
}
