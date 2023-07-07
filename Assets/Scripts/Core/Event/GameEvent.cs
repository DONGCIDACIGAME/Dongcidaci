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
        /// 存放待添加的事件处理（监听者+回调）
        /// 不能直接对事件字典进行操作，否则会触发迭代器错误
        /// 为了保证添加事件后可以立刻响应，在监听到事件时也需要并行处理下待添加的事件
        /// 在LateUpdate里会将待添加事件转存至事件字典中
        /// </summary>
        private List<GameEventData> mToAddCallbacksBuffer;

        /// <summary>
        /// 存放待删除的监听者
        /// 不能直接对事件字典进行操作，否则会触发迭代器错误
        /// 为了保证删除的事件不能响应，在监听到事件时做并行处理时，需要判断listenerId不在待删除里面
        /// 在LateUpdate里会删除事件处理字典和待添加事件处理列表中的对应监听者
        /// </summary>
        private HashSet<int> mToRemoveCallbacksBuffer;

        /// <summary>
        /// 存放事件处理的字典
        /// </summary>
        private Dictionary<int, Delegate> mCallbacks;


        public void Initialize(string evtName)
        {
            this.mEvtName = evtName;
            mCallbacks = new Dictionary<int, Delegate>();
            mToAddCallbacksBuffer = new List<GameEventData>();
            mToRemoveCallbacksBuffer = new HashSet<int>();
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
            mToAddCallbacksBuffer.Add(new GameEventData(listenerUniqueKey, callback));
        }

        public void OnTrigger()
        {
            foreach(KeyValuePair<int,Delegate> kv in mCallbacks)
            {
                int listenerId = kv.Key;
                Callback cb = kv.Value as Callback;

                if(!mToRemoveCallbacksBuffer.Contains(listenerId) && cb != null)
                {
                    cb();
                }
            }


            for(int i = 0; i < mToAddCallbacksBuffer.Count;i++)
            {
                GameEventData evt = mToAddCallbacksBuffer[i];

                int listenerId = evt.listenerId;
                Callback cb = evt.callback as Callback;

                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb();
                }
            }
        }

        public void OnTrigger<T1>(T1 arg1)
        {
            foreach (KeyValuePair<int, Delegate> kv in mCallbacks)
            {
                int listenerId = kv.Key;
                Callback<T1> cb = kv.Value as Callback<T1>;
                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb(arg1);
                }
            }

            for (int i = 0; i < mToAddCallbacksBuffer.Count; i++)
            {
                GameEventData evt = mToAddCallbacksBuffer[i];

                int listenerId = evt.listenerId;
                Callback<T1> cb = evt.callback as Callback<T1>;

                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb(arg1);
                }
            }
        }


        public void OnTrigger<T1, T2>(T1 arg1, T2 arg2)
        {
            foreach (KeyValuePair<int, Delegate> kv in mCallbacks)
            {
                int listenerId = kv.Key;
                Callback<T1, T2> cb = kv.Value as Callback<T1, T2>;
                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb(arg1, arg2);
                }
            }

            for (int i = 0; i < mToAddCallbacksBuffer.Count; i++)
            {
                GameEventData evt = mToAddCallbacksBuffer[i];

                int listenerId = evt.listenerId;
                Callback<T1, T2> cb = evt.callback as Callback<T1, T2>;

                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb(arg1, arg2);
                }
            }
        }


        public void OnTrigger<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            foreach (KeyValuePair<int, Delegate> kv in mCallbacks)
            {
                int listenerId = kv.Key;
                Callback<T1, T2, T3> cb = kv.Value as Callback<T1, T2, T3>;
                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb(arg1, arg2, arg3);
                }
            }

            for (int i = 0; i < mToAddCallbacksBuffer.Count; i++)
            {
                GameEventData evt = mToAddCallbacksBuffer[i];

                int listenerId = evt.listenerId;
                Callback<T1, T2, T3> cb = evt.callback as Callback<T1, T2, T3>;

                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb(arg1, arg2, arg3);
                }
            }
        }

        public void OnTrigger<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            foreach (KeyValuePair<int, Delegate> kv in mCallbacks)
            {
                int listenerId = kv.Key;
                Callback<T1, T2, T3, T4> cb = kv.Value as Callback<T1, T2, T3, T4>;
                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb(arg1, arg2, arg3, arg4);
                }
            }

            for (int i = 0; i < mToAddCallbacksBuffer.Count; i++)
            {
                GameEventData evt = mToAddCallbacksBuffer[i];

                int listenerId = evt.listenerId;
                Callback<T1, T2, T3, T4> cb = evt.callback as Callback<T1, T2, T3, T4>;

                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb(arg1, arg2, arg3, arg4);
                }
            }
        }

        public void OnTrigger<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            foreach (KeyValuePair<int, Delegate> kv in mCallbacks)
            {
                int listenerId = kv.Key;
                Callback<T1, T2, T3, T4, T5> cb = kv.Value as Callback<T1, T2, T3, T4, T5>;
                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb(arg1, arg2, arg3, arg4, arg5);
                }
            }

            for (int i = 0; i < mToAddCallbacksBuffer.Count; i++)
            {
                GameEventData evt = mToAddCallbacksBuffer[i];

                int listenerId = evt.listenerId;
                Callback<T1, T2, T3, T4, T5> cb = evt.callback as Callback<T1, T2, T3, T4, T5>;

                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb(arg1, arg2, arg3, arg4, arg5);
                }
            }
        }

        public void OnTrigger<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            foreach (KeyValuePair<int, Delegate> kv in mCallbacks)
            {
                int listenerId = kv.Key;
                Callback<T1, T2, T3, T4, T5, T6> cb = kv.Value as Callback<T1, T2, T3, T4, T5, T6>;
                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb(arg1, arg2, arg3, arg4, arg5, arg6);
                }
            }

            for (int i = 0; i < mToAddCallbacksBuffer.Count; i++)
            {
                GameEventData evt = mToAddCallbacksBuffer[i];

                int listenerId = evt.listenerId;
                Callback<T1, T2, T3, T4, T5, T6> cb = evt.callback as Callback<T1, T2, T3, T4, T5, T6>;

                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb(arg1, arg2, arg3, arg4, arg5, arg6);
                }
            }
        }


        public void OnTrigger<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            foreach (KeyValuePair<int, Delegate> kv in mCallbacks)
            {
                int listenerId = kv.Key;
                Callback<T1, T2, T3, T4, T5, T6, T7> cb = kv.Value as Callback<T1, T2, T3, T4, T5, T6, T7>;
                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                }
            }

            for (int i = 0; i < mToAddCallbacksBuffer.Count; i++)
            {
                GameEventData evt = mToAddCallbacksBuffer[i];

                int listenerId = evt.listenerId;
                Callback<T1, T2, T3, T4, T5, T6, T7> cb = evt.callback as Callback<T1, T2, T3, T4, T5, T6, T7>;

                if (cb != null && !mToRemoveCallbacksBuffer.Contains(listenerId))
                {
                    cb(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                }
            }
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="listenerUniqueKey"></param>
        public void RemoveListener(int listenerUniqueKey)
        {
            mToRemoveCallbacksBuffer.Add(listenerUniqueKey);
        }


        /// <summary>
        /// dipose
        /// </summary>
        public void Dispose()
        {
            mCallbacks.Clear();
            mToAddCallbacksBuffer.Clear();
            mToRemoveCallbacksBuffer.Clear();

            mCallbacks = null;
            mToAddCallbacksBuffer = null;
            mToRemoveCallbacksBuffer = null;
        }


        /// <summary>
        /// 在LateUpdate中添加事件
        /// </summary>
        public void OnLateUpdate(float deltaTime)
        {

            for(int i = mToAddCallbacksBuffer.Count-1; i>= 0; i--)
            {
                GameEventData data = mToAddCallbacksBuffer[i];
                if(mToRemoveCallbacksBuffer.Contains(data.listenerId))
                {
                    mToAddCallbacksBuffer.RemoveAt(i);
                }
            }


            foreach(int listenerUniqueKey in mToRemoveCallbacksBuffer)
            {

                if (mCallbacks.ContainsKey(listenerUniqueKey))
                {
                    mCallbacks.Remove(listenerUniqueKey);
                }
            }


            foreach (GameEventData evt in mToAddCallbacksBuffer)
            {
                int listenerUniqueKey = evt.listenerId;
                Delegate callback = evt.callback;

                if (mCallbacks.ContainsKey(listenerUniqueKey))
                {
                    Log.Error(LogLevel.Critical, "GameEvent AddListener Failed, key already exist");
                    return;
                }

                mCallbacks.Add(listenerUniqueKey, callback);
            }

            mToAddCallbacksBuffer.Clear();
        }
    }
}