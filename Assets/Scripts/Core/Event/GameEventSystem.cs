using System.Collections.Generic;

namespace GameEngine
{
    /// <summary>
    /// 事件系统
    /// </summary>
    public class GameEventSystem : ModuleManager<GameEventSystem>
    {
        private Dictionary<string, GameEvent> mEventMap;

        public override void Initialize()
        {
            mEventMap = new Dictionary<string, GameEvent>();
        }

        public override void Dispose()
        {
            mEventMap.Clear();
        }



        public void AddEventListen(object listener, string evtName, GameEventAction callback)
        {
            if (string.IsNullOrEmpty(evtName))
            {
                Log.Error(LogLevel.Critical, "AddListener Failed, invalid event name null or empty!");
                return;
            }

            if(listener == null)
            {
                Log.Error(LogLevel.Critical, "AddListener Failed, listener is null! event name:{0}", evtName);
                return;
            }


            if (callback == null)
            {
                Log.Error(LogLevel.Critical, "AddListener Failed, listener callback is null! event name:{0}", evtName);
                return;
            }

            int listenerUniqueKey = listener.GetHashCode();
            GameEvent evt;
            if (!mEventMap.TryGetValue(evtName, out evt))
            {
                evt = new GameEvent(evtName);
                mEventMap.Add(evtName, evt);
            }

            evt.AddListener(listenerUniqueKey, callback);
        }

        public void Fire(string evtName, params GameEventArgs[] args)
        {
            if (mEventMap.TryGetValue(evtName, out GameEvent evt))
            {
                evt.OnTrigger(args);
            }
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="evtName">事件名</param>
        public void RemoveEventListen(object listener,string evtName)
        {
            if (listener == null)
            {
                Log.Error(LogLevel.Critical, "GameEventSystem RemoveListener Failed, listener is null!");
                return;
            }

            if (mEventMap.TryGetValue(evtName, out GameEvent evt))
            {
                int listenerUniqueKey = listener.GetHashCode();
                evt.RemoveListener(listenerUniqueKey);
            }
        }
    }
}
