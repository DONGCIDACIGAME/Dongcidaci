using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class GameEvent : IGameLateUpdate, IDisposable
    {
        /// <summary>
        /// 事件名
        /// </summary>
        private string mEvtName;

        /// <summary>
        /// 存放事件回调的【临时】字典
        /// 因为事件的回调可能会触发绑定新事件，这样就会在遍历mCallbacks时插入新元素导致报错
        /// 故这里开了一个临时存储区mTempCallbacks，和mCallbacks逻辑上并行处理
        /// （必须做逻辑上的无差别并行处理，否则同一帧添加的事件，在这一帧上监听不到）
        /// 在LateUpdate里会将临时存储区mTempCallbacks内的事件 处理转移到mCallbacks
        /// </summary>
        private Dictionary<int, Delegate> mTempCallbacks;

        /// <summary>
        /// 存放事件回调的字典
        /// </summary>
        private Dictionary<int, Delegate> mCallbacks;

        public void Initialize(string evtName)
        {
            this.mEvtName = evtName;
            mCallbacks = new Dictionary<int, Delegate>();
            mTempCallbacks = new Dictionary<int, Delegate>();
        }

        public string GetEventName()
        {
            return mEvtName;
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="listenerUniqueKey">事件监听的唯一索引</param>
        /// <param name="callback">监听者听到事件时的行为</param>
        public void AddListener(int listenerUniqueKey, Delegate callback)
        {
            if (mTempCallbacks.ContainsKey(listenerUniqueKey))
                return;

            mTempCallbacks.Add(listenerUniqueKey, callback);
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

            foreach (Delegate obj in mTempCallbacks.Values)
            {
                Callback cb = obj as Callback;
                if (cb != null)
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

            foreach (Delegate obj in mTempCallbacks.Values)
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

            foreach (Delegate obj in mTempCallbacks.Values)
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

            foreach (Delegate obj in mTempCallbacks.Values)
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

            foreach (Delegate obj in mTempCallbacks.Values)
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

            foreach (Delegate obj in mTempCallbacks.Values)
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

            foreach (Delegate obj in mTempCallbacks.Values)
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

            if(mTempCallbacks.ContainsKey(listenerUniqueKey))
            {
                mTempCallbacks.Remove(listenerUniqueKey);
            }
        }


        /// <summary>
        /// dipose
        /// </summary>
        public void Dispose()
        {
            mCallbacks.Clear();
            mTempCallbacks.Clear();

            mCallbacks = null;
            mTempCallbacks = null;
        }


        /// <summary>
        /// 在LateUpdate中添加事件
        /// </summary>
        public void OnLateUpdate(float deltaTime)
        {
            foreach (KeyValuePair<int, Delegate> kv in mTempCallbacks)
            {
                int listenerUniqueKey = kv.Key;
                Delegate callback = kv.Value;

                if (mCallbacks.ContainsKey(listenerUniqueKey))
                {
                    Log.Error(LogLevel.Critical, "GameEvent AddListener Failed, key already exist");
                    return;
                }

                mCallbacks.Add(listenerUniqueKey, callback);
            }

            mTempCallbacks.Clear();
        }
    }
}