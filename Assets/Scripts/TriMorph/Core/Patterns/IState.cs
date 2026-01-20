namespace TriMorph.Core.Patterns
{
    /// <summary>
    /// Interface gốc cho mọi trạng thái trong game.
    /// Áp dụng cho: Player State, Enemy AI State, Game Loop State.
    /// </summary>
    public interface IState
    {
        void Enter();       // Gọi 1 lần khi bắt đầu vào trạng thái
        void Tick();        // Gọi liên tục trong Update()
        void FixedTick();   // Gọi liên tục trong FixedUpdate() (cho Physics)
        void Exit();        // Gọi 1 lần khi thoát trạng thái
    }
}
