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
        /// <summary>
        /// 所有事件
        /// </summary>
        private Dictionary<string, GameEvent> mEventMap;

        /// <summary>
        /// 所有缓存的Post的事件
        /// </summary>
        private Dictionary<string, Action> mToPostWith0ArgEvents;
        private Dictionary<string, Action> mToPostWith1ArgEvents;
        private Dictionary<string, Action> mToPostWith2ArgEvents;
        private Dictionary<string, Action> mToPostWith3ArgEvents;
        private Dictionary<string, Action> mToPostWith4ArgEvents;
        private Dictionary<string, Action> mToPostWith5ArgEvents;
        private Dictionary<string, Action> mToPostWith6ArgEvents;

        /// <summary>
        /// 所有待清除的Post事件
        /// </summary>
        private HashSet<string> mToRemovePostWith0ArgEvents;
        private HashSet<string> mToRemovePostWith1ArgEvents;
        private HashSet<string> mToRemovePostWith2ArgEvents;
        private HashSet<string> mToRemovePostWith3ArgEvents;
        private HashSet<string> mToRemovePostWith4ArgEvents;
        private HashSet<string> mToRemovePostWith5ArgEvents;
        private HashSet<string> mToRemovePostWith6ArgEvents;

        public override void Initialize()
        {
            mEventMap = new Dictionary<string, GameEvent>();

            mToPostWith0ArgEvents = new Dictionary<string, Action>();
            mToPostWith1ArgEvents = new Dictionary<string, Action>();
            mToPostWith2ArgEvents = new Dictionary<string, Action>();
            mToPostWith3ArgEvents = new Dictionary<string, Action>();
            mToPostWith4ArgEvents = new Dictionary<string, Action>();
            mToPostWith5ArgEvents = new Dictionary<string, Action>();
            mToPostWith6ArgEvents = new Dictionary<string, Action>();

            mToRemovePostWith0ArgEvents = new HashSet<string>();
            mToRemovePostWith1ArgEvents = new HashSet<string>();
            mToRemovePostWith2ArgEvents = new HashSet<string>();
            mToRemovePostWith3ArgEvents = new HashSet<string>();
            mToRemovePostWith4ArgEvents = new HashSet<string>();
            mToRemovePostWith5ArgEvents = new HashSet<string>();
            mToRemovePostWith6ArgEvents = new HashSet<string>();
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
                evt = new GameEvent();
                evt.Initialize(evtName);
                mEventMap.Add(evtName, evt);
            }

            evt.AddListener(listenerId, callback);
        }

        #region Fire
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
        #endregion

        #region Post
        public void Post(string evtName)
        {
            if (mToPostWith0ArgEvents.ContainsKey(evtName))
                mToPostWith0ArgEvents.Remove(evtName);

            mToPostWith0ArgEvents.Add(evtName, () =>
            {
                Fire(evtName);
            });
        }

        public void Post<T1>(string evtName, T1 arg1)
        {
            if (mToPostWith1ArgEvents.ContainsKey(evtName))
                mToPostWith1ArgEvents.Remove(evtName);

            mToPostWith1ArgEvents.Add(evtName, () =>
            {
                Fire(evtName, arg1);
            });
        }

        public void Post<T1, T2>(string evtName, T1 arg1, T2 arg2)
        {
            if (mToPostWith2ArgEvents.ContainsKey(evtName))
                mToPostWith2ArgEvents.Remove(evtName);

            mToPostWith2ArgEvents.Add(evtName, () =>
            {
                Fire(evtName, arg1,arg2);
            });
        }

        public void Post<T1, T2, T3>(string evtName, T1 arg1, T2 arg2, T3 arg3)
        {
            if (mToPostWith3ArgEvents.ContainsKey(evtName))
                mToPostWith3ArgEvents.Remove(evtName);

            mToPostWith3ArgEvents.Add(evtName, () =>
            {
                Fire(evtName, arg1, arg2, arg3);
            });
        }

        public void Post<T1, T2, T3, T4>(string evtName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (mToPostWith4ArgEvents.ContainsKey(evtName))
                mToPostWith4ArgEvents.Remove(evtName);

            mToPostWith4ArgEvents.Add(evtName, () =>
            {
                Fire(evtName, arg1, arg2, arg3, arg4);
            });
        }

        public void Post<T1, T2, T3, T4, T5>(string evtName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (mToPostWith5ArgEvents.ContainsKey(evtName))
                mToPostWith5ArgEvents.Remove(evtName);

            mToPostWith5ArgEvents.Add(evtName, () =>
            {
                Fire(evtName, arg1, arg2, arg3, arg4, arg5);
            });
        }

        public void Post<T1, T2, T3, T4, T5, T6>(string evtName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (mToPostWith6ArgEvents.ContainsKey(evtName))
                mToPostWith6ArgEvents.Remove(evtName);

            mToPostWith6ArgEvents.Add(evtName, () =>
            {
                Fire(evtName, arg1, arg2, arg3, arg4, arg5, arg6);
            });
        }
        #endregion


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



        private void __RemovePostedEvents(Dictionary<string, Action> toPostEvents, HashSet<string> toRemoveEvents)
        {
            foreach (string evtName in toRemoveEvents)
            {
                if(toPostEvents.ContainsKey(evtName))
                {
                    toPostEvents.Remove(evtName);
                }
            }
        }


        private bool __Post(ref int cnt, Dictionary<string, Action> toPostEvents, HashSet<string> toRemoveEvents)
        {
            foreach (KeyValuePair<string, Action> kv in toPostEvents)
            {
                string evtName = kv.Key;
                Action act = kv.Value;
                toRemoveEvents.Add(evtName);
                act();
                cnt++;
                if (cnt >= EventDef.PostEventExcuteSingleFrame)
                {
                    return false;
                }
            }

            return true;
        }


        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            int cnt = 0;

            // 返回结果表示这一帧是否把所有待Post的事件全部执行完了，这个结果目前没有用
            _ = __Post(ref cnt, mToPostWith0ArgEvents, mToRemovePostWith0ArgEvents)
                && __Post(ref cnt, mToPostWith1ArgEvents, mToRemovePostWith1ArgEvents)
                && __Post(ref cnt, mToPostWith2ArgEvents, mToRemovePostWith2ArgEvents)
                && __Post(ref cnt, mToPostWith3ArgEvents, mToRemovePostWith3ArgEvents)
                && __Post(ref cnt, mToPostWith4ArgEvents, mToRemovePostWith4ArgEvents)
                && __Post(ref cnt, mToPostWith5ArgEvents, mToRemovePostWith5ArgEvents)
                && __Post(ref cnt, mToPostWith6ArgEvents, mToRemovePostWith6ArgEvents);
        }

        public override void OnLateUpdate(float deltaTime)
        {
            base.OnLateUpdate(deltaTime);

            foreach (GameEvent evt in mEventMap.Values)
            {
                evt.OnLateUpdate(deltaTime);
            }

            __RemovePostedEvents(mToPostWith0ArgEvents, mToRemovePostWith0ArgEvents);
            __RemovePostedEvents(mToPostWith1ArgEvents, mToRemovePostWith1ArgEvents);
            __RemovePostedEvents(mToPostWith2ArgEvents, mToRemovePostWith2ArgEvents);
            __RemovePostedEvents(mToPostWith3ArgEvents, mToRemovePostWith3ArgEvents);
            __RemovePostedEvents(mToPostWith4ArgEvents, mToRemovePostWith4ArgEvents);
            __RemovePostedEvents(mToPostWith5ArgEvents, mToRemovePostWith5ArgEvents);
            __RemovePostedEvents(mToPostWith6ArgEvents, mToRemovePostWith6ArgEvents);
        }

        public override void Dispose()
        {
            mEventMap = null;
            mToPostWith0ArgEvents = null;
            mToPostWith1ArgEvents = null;
            mToPostWith2ArgEvents = null;
            mToPostWith3ArgEvents = null;
            mToPostWith4ArgEvents = null;
            mToPostWith5ArgEvents = null;
            mToPostWith6ArgEvents = null;

            mToRemovePostWith0ArgEvents = null;
            mToRemovePostWith1ArgEvents = null;
            mToRemovePostWith2ArgEvents = null;
            mToRemovePostWith3ArgEvents = null;
            mToRemovePostWith4ArgEvents = null;
            mToRemovePostWith5ArgEvents = null;
            mToRemovePostWith6ArgEvents = null;
        }
    }
}
