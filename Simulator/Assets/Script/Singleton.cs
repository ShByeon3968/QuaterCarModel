using UnityEngine;


/// <summary>
/// ΩÃ±€≈Ê
/// </summary>
public class Singleton<T> where T : class
{
    private T Instance = null;

    public Singleton(T obj) { Instance = obj; }
    public T Get { get { return Instance; } }
    public bool isExist { get { return Get != null; } }
}

/// <summary>
/// MonoBehaviour ΩÃ±€≈Ê
/// </summary>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T Instance;
    private static bool applicationQuit = false;

    public static T instance
    {
        get
        {
            if (Instance == null && !applicationQuit)
            {
                Instance = (T)FindObjectOfType(typeof(T));

                if (Instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).ToString());
                    Instance = obj.AddComponent(typeof(T)) as T;
                }
            }

            return Instance;
        }
    }


    private void OnApplicationQuit()
    {
        Instance = null;
        applicationQuit = true;
    }

    private void OnDestroy()
    {
        Instance = null;
        //applicationQuit = true;
    }
}

