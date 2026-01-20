using System.Collections.Generic;
using UnityEngine;

namespace TriMorph.Core.Patterns
{
    /// <summary>
    /// Hệ thống Pooling đơn giản, hiệu quả.
    /// Có thể dùng riêng lẻ cho từng loại prefab hoặc dùng chung.
    /// </summary>
    public class ObjectPooler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameObject prefabToPool;
        [SerializeField] private int initialPoolSize = 10;
        [SerializeField] private bool expandable = true; // Cho phép mở rộng nếu hết?

        private List<GameObject> pooledObjects;

        private void Awake()
        {
            pooledObjects = new List<GameObject>();
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateNewObject();
            }
        }

        private GameObject CreateNewObject()
        {
            GameObject obj = Instantiate(prefabToPool, transform);
            obj.SetActive(false); // Mặc định tắt
            pooledObjects.Add(obj);
            return obj;
        }

        /// <summary>
        /// Lấy một object từ bể chứa.
        /// </summary>
        public GameObject GetPooledObject()
        {
            // 1. Tìm object đang tắt (inactive)
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }

            // 2. Nếu hết object và cho phép mở rộng -> Tạo thêm
            if (expandable)
            {
                return CreateNewObject();
            }

            // 3. Nếu không thì trả về null
            return null;
        }

        /// <summary>
        /// Helper: Trả tất cả về pool (dùng khi Reset game)
        /// </summary>
        public void DeactivateAll()
        {
            foreach (var item in pooledObjects)
            {
                item.SetActive(false);
            }
        }
    }
}
