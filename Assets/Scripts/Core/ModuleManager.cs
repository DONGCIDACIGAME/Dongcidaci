using System.Collections.Generic;

namespace GameEngine
{
    public abstract class ModuleManager<T> : Singleton<T>, IModuleManager
        where T: new()
    {
        protected GameEventListener mEventListener;

        protected ModuleManager() {}

        protected virtual void BindEvents() { }

        public void __Initialize__()
        {
            mEventListener = new GameEventListener();
            BindEvents();
        }

        public abstract void Initialize();

        public abstract void Dispose();

        public void __Dispose__()
        {
            if (mEventListener != null)
                mEventListener.ClearAllEventListen();
        }

        public virtual void OnUpdate(float deltaTime) { }

        public virtual void OnLateUpdate(float deltaTime) { }


    }
}

