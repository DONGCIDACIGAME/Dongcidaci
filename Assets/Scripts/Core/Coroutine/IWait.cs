namespace GameEngine
{
    public interface IWait
    {
        bool CanMoveNext();
        void Tick(float deltaTime);
    }
}
