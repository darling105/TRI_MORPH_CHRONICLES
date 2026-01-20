using UnityEngine;
using UnityEngine.Events;

namespace TriMorph.Core.Events
{
    /// <summary>
    /// Kênh sự kiện không có tham số.
    /// Dùng cho các việc như: GameStart, GamePause, PlayerJump, v.v.
    /// Tạo file bằng cách: Chuột phải -> TriMorph -> Events -> Void Event Channel
    /// </summary>
    [CreateAssetMenu(menuName = "TriMorph/Events/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        // UnityAction nhẹ hơn UnityEvent và chuẩn C# hơn cho Code-based listener
        public UnityAction OnEventRaised;

        public void RaiseEvent()
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke();
            else
                Debug.LogWarning($"Event {name} được gọi nhưng không có ai lắng nghe.");
        }
    }
}
