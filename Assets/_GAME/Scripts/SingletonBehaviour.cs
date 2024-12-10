using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
  {
    private static T _instance;
    private static bool _destroying;

    /// <summary>
    /// This will be create a new game object if instance is null
    /// So if you want to check this null, use bool 'Initialized' instead.
    /// </summary>
    public static T Instance
    {
      get
      {
        if ((Object) SingletonBehaviour<T>._instance != (Object) null || SingletonBehaviour<T>._destroying)
          return SingletonBehaviour<T>._instance;
        SingletonBehaviour<T>._instance = Object.FindObjectOfType<T>();
        if ((Object) SingletonBehaviour<T>._instance == (Object) null)
          SingletonBehaviour<T>._instance = new GameObject(typeof (T).Name).AddComponent<T>();
        return SingletonBehaviour<T>._instance;
      }
    }

    /// <summary>
    /// Use for checking whether Instance is null or not.
    /// <para>Use "if (Initialized)" instead of "if (Instance != null)"</para>
    /// </summary>
    public static bool Initialized => (Object) SingletonBehaviour<T>._instance != (Object) null;

    protected virtual bool DontDestroy => false;

    protected virtual void OnAwake()
    {
    }

    private void Awake()
    {
      if ((Object) SingletonBehaviour<T>._instance == (Object) null)
        SingletonBehaviour<T>._instance = this as T;
      else if (SingletonBehaviour<T>._instance.GetInstanceID() != this.GetInstanceID())
      {
        Object.Destroy((Object) this.gameObject);
        return;
      }
      if (this.DontDestroy)
        Object.DontDestroyOnLoad((Object) this.gameObject);
      this.OnAwake();
    }

    /// <summary>Call 'T.Instance.Preload()' at the first application script to preload the service.</summary>
    public virtual void Preload()
    {
    }

    /// <summary>
    /// If you want to override this method, remember to call this base.
    /// </summary>
    protected virtual void OnDestroy()
    {
      SingletonBehaviour<T>._destroying = true;
      if (!((Object) SingletonBehaviour<T>._instance != (Object) null) || SingletonBehaviour<T>._instance.GetInstanceID() != this.GetInstanceID())
        return;
      SingletonBehaviour<T>._instance = default (T);
    }
  }

public class SingletonBehaviourDontDestroy<T> : SingletonBehaviour<T> where T : MonoBehaviour
{
    protected override bool DontDestroy => true;
}