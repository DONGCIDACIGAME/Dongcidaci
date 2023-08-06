using System.Collections.Generic;

namespace GameEngine
{
    /// <summary>
    /// 维护当前域的状态，父域子域的关系
    /// </summary>
    public partial class GameScope
    {
        /// <summary>
        /// 
        /// 域的路径名称
        /// </summary>
        private string mScopeName;

        /// <summary>
        /// 域的全路径名称
        /// </summary>
        private string mScopeFullName;

        /// <summary>
        /// 父域
        /// </summary>
        private GameScope mParentScope;

        /// <summary>
        /// 所有子域
        /// </summary>
        private Dictionary<string, GameScope> mChildScopes;

        /// <summary>
        /// 当前域是否在激活状态
        /// </summary>
        private bool mActive;


        /// <summary>
        /// 判断是否激活
        /// 必须所有的父域也在激活状态才是激活
        /// </summary>
        /// <returns></returns>
        public bool IsActive()
        {
            if (!mActive)
                return false;

            GameScope parent = mParentScope;
            while(parent != null)
            {
                if (!parent.IsActive())
                    return false;

                parent = parent.mParentScope;
            }

            return true;
        }

        /// <summary>
        /// 设置本域的激活状态
        /// </summary>
        /// <param name="active"></param>
        public void SetActive(bool active)
        {
            mActive = active;
        }

        private GameScope(string scopeName, GameScope parent)
        {
            if(string.IsNullOrEmpty(scopeName))
            {
                Log.Error(LogLevel.Critical, "Scope constructor error, scope name is null or empty!");
            }

            mScopeName = scopeName;
            mParentScope = parent;

            mChildScopes = new Dictionary<string, GameScope>();
            mActive = true;

            if (mParentScope != null)
            {
                mScopeFullName = mParentScope.mScopeFullName + "." + scopeName;
            }
            else
            {
                mScopeFullName = scopeName;
            }

            GameScopeManager.Ins.RegisterScope(this);
        }

        public bool Equals(GameScope other)
        {
            if (other == null)
                return false;

            return other.GetScopeName().Equals(mScopeName);
        }

        public bool In(GameScope scope)
        {
            GameScope temp = this;
            while(temp != null)
            {
                if(temp.Equals(scope))
                    return true;

                temp = mParentScope;
            }

            return false;
        }

        /// <summary>
        /// 获取子域
        /// </summary>
        /// <param name="scopeName"></param>
        /// <returns></returns>
        public GameScope GetChildScope(string scopeName)
        {
            if(mChildScopes.TryGetValue(scopeName,out GameScope scope))
            {
                return scope;
            }

            return null;
        }

        /// <summary>
        /// 获取当前域的名称
        /// </summary>
        /// <returns></returns>
        public string GetScopeName()
        {
            return mScopeName;
        }

        /// <summary>
        /// 获取域的全路径名称
        /// </summary>
        /// <returns></returns>
        public string GetScopeFullName()
        {
            return mScopeFullName;
        }

        /// <summary>
        /// 创建子域
        /// </summary>
        /// <param name="scopeName"></param>
        /// <returns></returns>
        public GameScope CreateChildScope(string scopeName)
        {
            if(string.IsNullOrEmpty(scopeName))
            {
                Log.Error(LogLevel.Critical, "[{0}] Create Child Scope Failed, scope name is null or empty!", mScopeName);
                return null;
            }

            //Log.Logic(LogLevel.Info, "thisScope:{0}----CreateChildScope:{1}",mScopeFullName, scopeName);

            GameScope scope = new GameScope(scopeName, this);
            mChildScopes.Add(scopeName, scope);
            return scope;
        }

        public void RemoveChildScope(string scopeName)
        {
            if (string.IsNullOrEmpty(scopeName))
            {
                Log.Error(LogLevel.Critical, "[{0}] Remove Child Scope Failed, scope name is null or empty!", mScopeName);
                return;
            }

            //Log.Logic(LogLevel.Info, "thisScope:{0}----CreateChildScope:{1}",mScopeFullName, scopeName);
            if(mChildScopes.ContainsKey(scopeName))
            {
                mChildScopes.Remove(scopeName);
            }
        }

        public void Dispose()
        {
            GameScopeManager.Ins.UnregisterScope(this);

            foreach (GameScope scope in mChildScopes.Values)
            {
                scope.Dispose();
            }

            mParentScope = null;
            mChildScopes = null;
            mScopeName = null;
            mScopeFullName = null;
            mActive = false;
        }
    }
}