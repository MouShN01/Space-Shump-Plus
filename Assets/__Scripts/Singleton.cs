using UnityEngine;

namespace __Scripts
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if (_instance == null)
                        SetupInstance();
                }

                return _instance;
            }
        }

        public void Awake()
        {
            RemoveDuplicates();
        }

        private static void SetupInstance()
        {
            _instance = (T)FindObjectOfType(typeof(T));
            if (_instance == null)
            {
                GameObject gO = new GameObject();
                gO.name = typeof(T).Name;
                _instance = gO.GetComponent<T>();
                DontDestroyOnLoad(gO);
            }
        }

        private void RemoveDuplicates()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
