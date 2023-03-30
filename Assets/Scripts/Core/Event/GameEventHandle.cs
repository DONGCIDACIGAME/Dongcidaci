namespace GameEngine
{
    public class GameEventHandle
    {
        private GameEventAction mCallback;
        private GameScope[] mWorkingScopes;

        public GameEventHandle(GameEventAction callback, GameScope[] workingScopes)
        {
            this.mCallback = callback;
            this.mWorkingScopes = workingScopes;
        }

        public bool CheckWorking()
        {
            // 生效的scope是空的，非法
            if (mWorkingScopes == null)
            {
                Log.Error(LogLevel.Critical, "GameEventHandle CheckWorking Error, target working scopes is null!");
                return false;
            }

            // 遍历比对生效scope中有没有目标scope
            for (int i = 0; i < mWorkingScopes.Length; i++)
            {
                GameScope scope = mWorkingScopes[i];
                if(scope.IsActive())
                {
                    return true;
                }
            }

            return false;
        }

        public void Excute(GameEventArgs[] args)
        {
            if (mCallback != null)
            {
                mCallback(args);
            }
        }
    }
}