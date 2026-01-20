using UnityEngine;

namespace TriMorph.Core.Patterns
{
    /// <summary>
    /// Class quản lý việc chuyển đổi giữa các IState.
    /// Đảm bảo nguyên tắc Single Responsibility: Chỉ lo việc đổi State.
    /// </summary>
    public class StateMachine
    {
        // Trạng thái hiện tại (Read-only từ bên ngoài để an toàn)
        public IState CurrentState { get; private set; }

        // Khởi tạo trạng thái đầu tiên
        public void Initialize(IState startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        // Chuyển đổi sang trạng thái mới
        public void ChangeState(IState newState)
        {
            // Ngăn việc tự chuyển sang chính mình gây lỗi lặp
            if (CurrentState == newState) return;

            // Quy trình chuẩn: Thoát cũ -> Gán mới -> Vào mới
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        // Cần được gọi trong Update() của Monobehaviour
        public void Update()
        {
            CurrentState?.Tick();
        }

        // Cần được gọi trong FixedUpdate() của Monobehaviour
        public void FixedUpdate()
        {
            CurrentState?.FixedTick();
        }
    }
}
