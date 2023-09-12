using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        instance = null;
    }



    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Addressables.LoadAssetAsync<T>(typeof(T).Name).WaitForCompletion();
            }

            return instance;
        }
    }
}
