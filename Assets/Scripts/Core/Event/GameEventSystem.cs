using System;
using System.Collections.Generic;


/// <summary>
/// TODO:
/// 1.目前所有的事件对所有的监听者都开放广播，可能会产生一些性能问题
///   -后续可能会给事件一些监听域，事件可以选择对某些监听域内的监听者进行广播
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
        /// Post的事件也是有序的，根据提交Post的顺序决定执行的顺序
        /// 对于同一个事件名，后提交的Post不会覆盖前一个Post
        /// </summary>
        private Queue<PostEvent> mToPostEvents;


        public override void Initialize()
        {
            mEventMap = new Dictionary<string, GameEvent>();
            mToPostEvents = new Queue<PostEvent>();
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

        public void Fire<T1, T2, T3, T4, T5, T6, T7>(string evtName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (mEventMap.TryGetValue(evtName, out GameEvent evt))
            {
                evt.OnTrigger(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }
        }
        #endregion

        #region Post
        public void Post(string evtName)
        {
            mToPostEvents.Enqueue(new PostEvent(evtName, () =>
            {
                Fire(evtName);
            }));
        }

        public void Post<T1>(string evtName, T1 arg1)
        {
            mToPostEvents.Enqueue(new PostEvent(evtName, () =>
            {
                Fire(evtName, arg1);
            }));
        }

        public void Post<T1, T2>(string evtName, T1 arg1, T2 arg2)
        {
            mToPostEvents.Enqueue(new PostEvent(evtName, () =>
            {
                Fire(evtName, arg1,arg2);
            }));
        }

        public void Post<T1, T2, T3>(string evtName, T1 arg1, T2 arg2, T3 arg3)
        {
            mToPostEvents.Enqueue(new PostEvent(evtName, () =>
            {
                Fire(evtName, arg1, arg2, arg3);
            }));
        }

        public void Post<T1, T2, T3, T4>(string evtName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            mToPostEvents.Enqueue(new PostEvent(evtName, () =>
            {
                Fire(evtName, arg1, arg2, arg3, arg4);
            }));
        }

        public void Post<T1, T2, T3, T4, T5>(string evtName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            mToPostEvents.Enqueue(new PostEvent(evtName, () =>
            {
                Fire(evtName, arg1, arg2, arg3, arg4, arg5);
            }));
        }

        public void Post<T1, T2, T3, T4, T5, T6>(string evtName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            mToPostEvents.Enqueue(new PostEvent(evtName, () =>
            {
                Fire(evtName, arg1, arg2, arg3, arg4, arg5, arg6);
            }));
        }

        public void Post<T1, T2, T3, T4, T5, T6, T7>(string evtName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            mToPostEvents.Enqueue(new PostEvent(evtName, () =>
            {
                Fire(evtName, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }));
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


        public override void OnGameUpdate(float deltaTime)
        {
            base.OnGameUpdate(deltaTime);

            int cnt = 0;
            while(cnt <= EventDef.MaxPostEventInOneFrame)
            {
                if(!mToPostEvents.TryDequeue(out PostEvent toPost))
                    break;

                toPost.Callback();
            }
        }

        public override void OnLateUpdate(float deltaTime)
        {
            base.OnLateUpdate(deltaTime);

            foreach (GameEvent evt in mEventMap.Values)
            {
                evt.OnLateUpdate(deltaTime);
            }
        }

        public override void Dispose()
        {
            mEventMap = null;
            mToPostEvents = null;
        }
    }
}
