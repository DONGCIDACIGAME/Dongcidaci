using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class GameEventListener
    {
        private int mListenerId;
        private Dictionary<string, Delegate> mEventTable;

        public GameEventListener()
        {
            mEventTable = new Dictionary<string, Delegate>();
            mListenerId = GameEventSystem.Ins.PopEventListenId();
        }

        public int GetListenerId()
        {
            return mListenerId;
        }

        private bool BindEventCheck(string evtName, Delegate cb)
        {
            if (string.IsNullOrEmpty(evtName))
            {
                Log.Error(LogLevel.Normal, "BindEventCheck Failed, null or empty string is invalid event name!");
                return false;
            }

            if (cb == null)
            {
                Log.Error(LogLevel.Critical, "BindEventCheck Failed, cb is not allow to be null!");
                return false;
            }

            if (mEventTable.ContainsKey(evtName))
            {
                Log.Error(LogLevel.Critical, "BindEventCheck Failed, repeat event listen, event name:{0}", evtName);
                return false;
            }

            return true;
        }

        public void Listen(string evtName, Callback cb)
        {
            if (!BindEventCheck(evtName, cb))
                return;

            mEventTable.Add(evtName, cb);

            GameEventSystem.Ins.AddEventListen(mListenerId, evtName, cb);
        }

        public void Listen<T1>(string evtName, Callback<T1> cb)
        {
            if (!BindEventCheck(evtName, cb))
                return;

            mEventTable.Add(evtName, cb);

            GameEventSystem.Ins.AddEventListen(mListenerId, evtName, cb);
        }


        public void Listen<T1, T2>(string evtName, Callback<T1, T2> cb)
        {
            if (!BindEventCheck(evtName, cb))
                return;

            mEventTable.Add(evtName, cb);

            GameEventSystem.Ins.AddEventListen(mListenerId, evtName, cb);
        }


        public void Listen<T1, T2, T3>(string evtName, Callback<T1, T2, T3> cb)
        {
            if (!BindEventCheck(evtName, cb))
                return;

            mEventTable.Add(evtName, cb);

            GameEventSystem.Ins.AddEventListen(mListenerId, evtName, cb);
        }


        public void Listen<T1, T2, T3, T4>(string evtName, Callback<T1, T2, T3, T4> cb)
        {
            if (!BindEventCheck(evtName, cb))
                return;

            mEventTable.Add(evtName, cb);

            GameEventSystem.Ins.AddEventListen(mListenerId, evtName, cb);
        }

        public void Listen<T1, T2, T3, T4, T5>(string evtName, Callback<T1, T2, T3, T4, T5> cb)
        {
            if (!BindEventCheck(evtName, cb))
                return;

            mEventTable.Add(evtName, cb);

            GameEventSystem.Ins.AddEventListen(mListenerId, evtName, cb);
        }

        public void Listen<T1, T2, T3, T4, T5, T6>(string evtName, Callback<T1, T2, T3, T4, T5, T6> cb)
        {
            if (!BindEventCheck(evtName, cb))
                return;

            mEventTable.Add(evtName, cb);

            GameEventSystem.Ins.AddEventListen(mListenerId, evtName, cb);
        }

        public void RemoveEventListen(string evtName)
        {
            if (string.IsNullOrEmpty(evtName))
            {
                Log.Error(LogLevel.Normal, "RemoveEvent Failed, null or empty string is invalid event name!");
                return;
            }

            if (mEventTable == null)
            {
                Log.Error(LogLevel.Critical, "RemoveEvent Failed, Game Event List is null!");
                return;
            }

            if (mEventTable.ContainsKey(evtName))
            {
                mEventTable.Remove(evtName);
                GameEventSystem.Ins.RemoveEventListen(mListenerId, evtName);
            }

        }

        public void ClearAllEventListen()
        {
            if (mEventTable == null)
                return;

            foreach (string evtName in mEventTable.Keys)
            {
                GameEventSystem.Ins.RemoveEventListen(mListenerId, evtName);
            }
        }
    }
}

