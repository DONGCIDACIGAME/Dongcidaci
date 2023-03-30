namespace GameEngine
{
    public delegate bool WDWaitDelegate();

    public class WDWaitUntil : IWait
    {
        private WDWaitDelegate _waitCheck;

        public WDWaitUntil(WDWaitDelegate wait)
        {
            _waitCheck = wait;
        }

        public bool CanMoveNext()
        {
            if (_waitCheck == null)
                return true;

            return _waitCheck();
        }

        public void Tick(float deltaTime)
        {
            
        }
    }
}
