using UnityEngine;

/// <summary>
/// Thread-safe, Lazy-loaded Singleton tối ưu cho Unity 6.
/// Tự động xử lý vòng đời, ngăn chặn lỗi "Zombie Object" khi thoát game.
/// </summary>
/// <typeparam name="T">Kiểu dữ liệu của class kế thừa (VD: GameManager)</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Instance lưu trữ private
    private static T _instance;

    // Object dùng để khóa luồng (Thread-safety)
    private static readonly object _lock = new object();

    // Cờ kiểm tra xem ứng dụng có đang tắt không
    private static bool _isQuitting = false;

    /// <summary>
    /// Property để truy cập Instance. 
    /// Gọi: ClassName.Instance
    /// </summary>
    public static T Instance
    {
        get
        {
            // 1. Tối ưu: Nếu đang quit game thì không bao giờ tạo mới nữa để tránh lỗi.
            if (_isQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' đã bị hủy do ứng dụng đang tắt. Trả về null.");
                return null;
            }

            // 2. Thread Safety: Đảm bảo luồng an toàn
            lock (_lock)
            {
                // Nếu đã có instance rồi thì trả về ngay
                if (_instance != null)
                    return _instance;

                // 3. Unity 6 Optimization: Dùng FindFirstObjectByType thay vì FindObjectOfType
                // FindFirstObjectByType nhanh hơn và là chuẩn mới.
                _instance = FindFirstObjectByType<T>();

                // Nếu tìm thấy trong Scene
                if (_instance != null)
                {
                    return _instance;
                }

                // 4. Lazy Instantiation: Nếu chưa có thì tự tạo GameObject mới
                var singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<T>();

                // Đặt tên cho dễ debug trong Inspector
                singletonObject.name = typeof(T).Name + " (Singleton)";

                // 5. Tự động DontDestroyOnLoad (Tùy chọn: Nếu bạn muốn nó sống qua các Scene)
                // Lưu ý: Nếu class con tự xử lý logic này thì có thể override Awake.
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(singletonObject);
                }

                return _instance;
            }
        }
    }

    /// <summary>
    /// Awake ảo để class con có thể override nếu cần, nhưng vẫn đảm bảo logic Singleton.
    /// </summary>
    protected virtual void Awake()
    {
        // Xử lý trường hợp Duplicate: Nếu Instance đã tồn tại và không phải là cái này -> Hủy cái này.
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning($"[Singleton] Phát hiện bản sao của {typeof(T)}. Đang hủy bản sao trên GameObject '{gameObject.name}'.");
            Destroy(gameObject);
            return;
        }

        _instance = this as T;

        // Đảm bảo không bị hủy khi load scene mới (nếu là root object)
        if (transform.parent == null)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Ngăn chặn tạo Instance mới khi ứng dụng đang thoát.
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    /// <summary>
    /// Reset cờ khi disable (hữu ích nếu tắt bật Domain Reload trong Unity Editor Settings)
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _isQuitting = true;
        }
    }
}