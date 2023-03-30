using System.Collections.Generic;

namespace GameEngine
{
    public class GameEvent
    {
        private string mEvtName;

        private Dictionary<int, GameEventAction> mHandles = new Dictionary<int, GameEventAction>();

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
        public void AddListener(int listenerUniqueKey, GameEventAction callback)
        {
            if(mHandles.ContainsKey(listenerUniqueKey))
            {
                Log.Error(LogLevel.Critical, "GameEvent AddListener Failed, key already exist");
                return;
            }

            mHandles.Add(listenerUniqueKey, callback);
        }

        public void OnTrigger(GameEventArgs[] args)
        {
            foreach(GameEventAction handle in mHandles.Values)
            {
                handle(args);
            }
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="listenerUniqueKey"></param>
        public void RemoveListener(int listenerUniqueKey)
        {
            if(mHandles.ContainsKey(listenerUniqueKey))
            {
                mHandles.Remove(listenerUniqueKey);
            }
        }


        /// <summary>
        /// dipose
        /// </summary>
        public void Dispose()
        {
            mHandles.Clear();
        }
    }
}