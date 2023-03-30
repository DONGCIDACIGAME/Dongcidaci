using System.Collections.Generic;

namespace GameEngine
{
    public class GameScopeManager : ModuleManager<GameScopeManager>
    {
        private Dictionary<string, GameScope> mAllScopes = new Dictionary<string, GameScope>();


        public override void Dispose()
        {
            if(mAllScopes != null)
                mAllScopes.Clear();
        }

        public override void Initialize()
        {
            if(mAllScopes == null)
                mAllScopes = new Dictionary<string, GameScope>();
        }

        public void RegisterScope(GameScope scope)
        {
            if (scope == null)
                return;

            string scopeFullName = scope.GetScopeFullName();

            if (string.IsNullOrEmpty(scopeFullName))
                return;

            if (!mAllScopes.ContainsKey(scopeFullName))
            {
                mAllScopes.Add(scopeFullName, scope);
            }
        }

        public void UnregisterScope(GameScope scope)
        {
            if (scope == null)
                return;

            string scopeName = scope.GetScopeFullName();
            if (mAllScopes.ContainsKey(scopeName))
            {
                mAllScopes.Remove(scopeName);
            }
        }
    }
}
