using System.Collections.Generic;

namespace GameEngine
{
    /// <summary>
    /// 事件系统
    /// </summary>
    public class GameEventSystem : ModuleManager<GameEventSystem>
    {
        private Dictionary<string, GameEvent> mEventMap;

        private int indexer;

        public override void Initialize()
        {
            mEventMap = new Dictionary<string, GameEvent>();
        }

        public int PopEventListenId()
        {
            return indexer++;
        }

        public override void Dispose()
        {
            mEventMap.Clear();
        }

        public void AddEventListen(int listenerId, string evtName, GameEventAction callback)
        {
            if (string.IsNullOrEmpty(evtName))
            {
                Log.Error(LogLevel.Critical, "AddListener Failed, invalid event name null or empty!");
                return;
            }

            if(listenerId == 0)
            {
                Log.Error(LogLevel.Critical, "AddListener Failed, listener id is invalid event name:{0}", evtName);
                return;
            }


            if (callback == null)
            {
                Log.Error(LogLevel.Critical, "AddListener Failed, listener callback is null! event name:{0}", evtName);
                return;
            }

            GameEvent evt;
            if (!mEventMap.TryGetValue(evtName, out evt))
            {
                evt = new GameEvent(evtName);
                mEventMap.Add(evtName, evt);
            }

            evt.AddListener(listenerId, callback);
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
        public void RemoveEventListen(int listenerId, string evtName)
        {
            if (listenerId == 0)
            {
                Log.Error(LogLevel.Critical, "GameEventSystem RemoveListener Failed, listener id is invalid, event name:{0}!", evtName);
                return;
            }

            if (mEventMap.TryGetValue(evtName, out GameEvent evt))
            {
                evt.RemoveListener(listenerId);
            }
        }
    }
}
