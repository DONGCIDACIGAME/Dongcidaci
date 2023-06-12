using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class GameEvent
    {
        private string mEvtName;

        private Dictionary<int, Delegate> mCallbacks = new Dictionary<int, Delegate>();

        public GameEvent(string evtName)
        {
            this.mEvtName = evtName;
        }

        public string GetEventName()
        {
            return mEvtName;
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="listenerUniqueKey">事件监听的唯一索引，推荐使用监听者的hashcode</param>
        /// <param name="callback">监听者听到事件时的行为</param>
        /// <param name="workingScopes">事件的生效作用域</param>
        public void AddListener(int listenerUniqueKey, Delegate callback)
        {
            if(mCallbacks.ContainsKey(listenerUniqueKey))
            {
                Log.Error(LogLevel.Critical, "GameEvent AddListener Failed, key already exist");
                return;
            }

            mCallbacks.Add(listenerUniqueKey, callback);
        }

        public void OnTrigger()
        {
            foreach(Delegate obj in mCallbacks.Values)
            {
                Callback cb = obj as Callback;
                if(cb != null)
                {
                    cb();
                }
            }
        }

        public void OnTrigger<T1>(T1 arg1)
        {
            foreach (Delegate obj in mCallbacks.Values)
            {
                Callback<T1> cb = obj as Callback<T1>;
                if (cb != null)
                {
                    cb(arg1);
                }
            }
        }


        public void OnTrigger<T1, T2>(T1 arg1, T2 arg2)
        {
            foreach (Delegate obj in mCallbacks.Values)
            {
                Callback<T1, T2> cb = obj as Callback<T1, T2>;
                if (cb != null)
                {
                    cb(arg1, arg2);
                }
            }
        }


        public void OnTrigger<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            foreach (Delegate obj in mCallbacks.Values)
            {
                Callback<T1, T2, T3> cb = obj as Callback<T1, T2, T3>;
                if (cb != null)
                {
                    cb(arg1, arg2, arg3);
                }
            }
        }

        public void OnTrigger<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            foreach (Delegate obj in mCallbacks.Values)
            {
                Callback<T1, T2, T3, T4> cb = obj as Callback<T1, T2, T3, T4>;
                if (cb != null)
                {
                    cb(arg1, arg2, arg3, arg4);
                }
            }
        }

        public void OnTrigger<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            foreach (Delegate obj in mCallbacks.Values)
            {
                Callback<T1, T2, T3, T4, T5> cb = obj as Callback<T1, T2, T3, T4, T5>;
                if (cb != null)
                {
                    cb(arg1, arg2, arg3, arg4, arg5);
                }
            }
        }

        public void OnTrigger<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            foreach (Delegate obj in mCallbacks.Values)
            {
                Callback<T1, T2, T3, T4, T5, T6> cb = obj as Callback<T1, T2, T3, T4, T5, T6>;
                if (cb != null)
                {
                    cb(arg1, arg2, arg3, arg4, arg5, arg6);
                }
            }
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="listenerUniqueKey"></param>
        public void RemoveListener(int listenerUniqueKey)
        {
            if(mCallbacks.ContainsKey(listenerUniqueKey))
            {
                mCallbacks.Remove(listenerUniqueKey);
            }
        }


        /// <summary>
        /// dipose
        /// </summary>
        public void Dispose()
        {
            mCallbacks.Clear();
        }
    }
}