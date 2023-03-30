namespace GameEngine
{
    public class WDWaitForSeconds : IWait
    {
        private float _waitTime;
        private float _timer;
        public WDWaitForSeconds(float seconds)
        {
            _timer = 0;
            _waitTime = seconds;
        }

        public bool CanMoveNext()
        {
            return _timer >= _waitTime;
        }

        public void Tick(float deltaTime)
        {
            _timer += deltaTime;
        }
    }
}
