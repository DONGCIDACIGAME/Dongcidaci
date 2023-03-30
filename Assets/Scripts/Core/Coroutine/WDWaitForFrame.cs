namespace GameEngine
{
    public class WDWaitForFrame : IWait
    {
        private int _waitFrame;
        private int _counter;
        public WDWaitForFrame(int frame)
        {
            _counter = 0;
            _waitFrame = frame;
        }
        public bool CanMoveNext()
        {
            return _counter >= _waitFrame;
        }

        public void Tick(float deltaTime)
        {
            _counter++;
        }
    }
}
