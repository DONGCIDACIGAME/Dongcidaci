using System.Collections.Generic;

namespace GameEngine
{
    public abstract class ModuleManager<T> : Singleton<T>, IModuleManager
        where T: new()
    {
        protected GameEventListener mEventListener;

        protected ModuleManager() {}

        public void __Initialize__()
        {
            mEventListener = new GameEventListener();
        }

        public abstract void Initialize();

        public void BindEvent(string evtName, GameEventAction action)
        {
            if(mEventListener == null)
            {
                Log.Error(LogLevel.Critical ,"ModuleManager Bind Event Failed, event listener not initialized!");
                return;
            }

            mEventListener.BindEvent(evtName, action);
        }


        public abstract void Dispose();

        public void __Dispose__()
        {
            if (mEventListener != null)
                mEventListener.ClearEvents();
        }

        public virtual void OnUpdate(float deltaTime) { }

        public virtual void OnLateUpdate(float deltaTime) { }


    }
}

