using GameEngine;
using System.Collections.Generic;

//public class GameEventBinder : IGameEventListen
//{
//    private Dictionary<string, GameEventAction> mEvents;
//    private GameScope[] mScopes;

//    public GameEventBinder(Dictionary<string, GameEventAction> events, params GameScope[] scopes)
//    {
//        this.mEvents = events;
//        this.mScopes = scopes;
//    }

//    public void BindEvents()
//    {
//        if (mScopes == null || mScopes.Length == 0 || mEvents == null || mEvents.Count == 0)
//            return;

//        foreach (KeyValuePair<string, GameEventAction> kv in mEvents)
//        {
//            string evtName = kv.Key;
//            GameEventAction action = kv.Value;

//            GameEventSystem.Ins.AddListener(this, evtName, action, mScopes);
//        }
//    }

//    public void ClearEvents()
//    {
//        if (mScopes == null || mScopes.Length == 0 || mEvents == null || mEvents.Count == 0)
//            return;

//        foreach (string evtName in mEvents.Keys)
//        {
//            GameEventSystem.Ins.RemoveListener(this, evtName);
//        }
//    }

//}
