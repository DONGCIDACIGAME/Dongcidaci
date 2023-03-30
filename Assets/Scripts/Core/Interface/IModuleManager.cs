namespace GameEngine
{
    public interface IModuleManager : IGameUpdate, IGameLateUpdate
    {
        void __Initialize__();
        void Initialize();
        void Dispose();
        void __Dispose__();
    }
}
