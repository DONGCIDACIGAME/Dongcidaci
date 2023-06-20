using System;
using System.Collections.Generic;


/// <summary>
/// TODO: 目前所有的事件对所有的监听者都开放广播，可能会产生一些性能问题
/// 后续可能会给事件一些监听域，事件可以选择对某些监听域内的监听者进行广播
/// </summary>
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

        public void AddEventListen(int listenerId, string evtName, Delegate callback)
        {
            if (string.IsNullOrEmpty(evtName))
            {
                Log.Error(LogLevel.Critical, "AddListener Failed, invalid event name null or empty!");
                return;
            }

            if (listenerId == 0)
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

        public void Fire(string evtName)
        {
            if (mEventMap.TryGetValue(evtName, out GameEvent evt))
            {
                evt.OnTrigger();
            }
        }


        public void Fire<T1>(string evtName, T1 arg1)
        {
            if (mEventMap.TryGetValue(evtName, out GameEvent evt))
            {
                evt.OnTrigger(arg1);
            }
        }

        public void Fire<T1, T2>(string evtName, T1 arg1, T2 arg2)
        {
            if (mEventMap.TryGetValue(evtName, out GameEvent evt))
            {
                evt.OnTrigger(arg1, arg2);
            }
        }

        public void Fire<T1, T2, T3>(string evtName, T1 arg1, T2 arg2, T3 arg3)
        {
            if (mEventMap.TryGetValue(evtName, out GameEvent evt))
            {
                evt.OnTrigger(arg1, arg2, arg3);
            }
        }

        public void Fire<T1, T2, T3, T4>(string evtName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (mEventMap.TryGetValue(evtName, out GameEvent evt))
            {
                evt.OnTrigger(arg1, arg2, arg3, arg4);
            }
        }

        public void Fire<T1, T2, T3, T4, T5>(string evtName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (mEventMap.TryGetValue(evtName, out GameEvent evt))
            {
                evt.OnTrigger(arg1, arg2, arg3, arg4, arg5);
            }
        }

        public void Fire<T1, T2, T3, T4, T5, T6>(string evtName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (mEventMap.TryGetValue(evtName, out GameEvent evt))
            {
                evt.OnTrigger(arg1, arg2, arg3, arg4, arg5, arg6);
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

        public override void OnLateUpdate(float deltaTime)
        {
            base.OnLateUpdate(deltaTime);

            foreach(GameEvent evt in mEventMap.Values)
            {
                evt.OnLateUpdate(deltaTime);
            }
        }
    }
}
