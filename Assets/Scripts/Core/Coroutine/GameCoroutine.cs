using System.Collections;

namespace GameEngine
{
    public class GameCoroutine
    {
        private IEnumerator _routine;

        public GameCoroutine(IEnumerator routine)
        {
            _routine = routine;
        }

        public bool MoveNext(float deltaTime)
        {
            if(_routine == null)
            {
                return false;
            }

            bool ret = true;
            var wait = _routine.Current as IWait;
            if(wait != null)
            {
                wait.Tick(deltaTime);
                ret = wait.CanMoveNext();
            }

            if(ret)
            {
                return _routine.MoveNext();
            }
            else
            {
                return true;
            }
        }

        public void Dispose()
        {
            _routine = null;
        }
    }
}

