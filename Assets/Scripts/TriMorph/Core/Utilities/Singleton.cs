using UnityEngine;

namespace TriMorph.Core.Utilities
{
    /// <summary>
    /// Base class cho Singleton.
    /// Sử dụng: public class GameManager : Singleton<GameManager> { }
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Tìm object có sẵn trong scene
                    _instance = FindObjectOfType<T>();
                    
                    if (_instance == null)
                    {
                        // Nếu chưa có thì tạo mới (Optional: Tùy game logic)
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                // Nếu muốn tồn tại qua các màn chơi, uncomment dòng dưới:
                // DontDestroyOnLoad(gameObject); 
            }
            else if (_instance != this)
            {
                // Nếu đã có 1 bản sao rồi thì hủy bản sao mới này đi để tránh trùng lặp
                Destroy(gameObject);
            }
        }
    }
}
