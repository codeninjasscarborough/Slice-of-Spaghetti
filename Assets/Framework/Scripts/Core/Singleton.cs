using UnityEngine;

namespace CardGame
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<T>();
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }

    public abstract class PureSingleton<T> where T : class, new()
    {
        static T _instance;
        public static T Instance => _instance ??= new T();

        protected PureSingleton() { }

        public static void ResetInstance() => _instance = null;
    }
}
