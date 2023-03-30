using System.Collections.Generic;
using GameEngine;

public class GameEventListener : IGameEventListen
{
    private Dictionary<string, GameEventAction> mGameEvents;
    public GameEventListener()
    {
        mGameEvents = new Dictionary<string, GameEventAction>();
    }


    public void BindEvent(string evtName, GameEventAction action)
    {
        if (string.IsNullOrEmpty(evtName))
        {
            Log.Error(LogLevel.Normal, "BindEvent Failed, null or empty string is invalid event name!");
            return;
        }

        if (action == null)
        {
            Log.Error(LogLevel.Critical, "BindEvent Failed, event action is not allow to be null!");
            return;
        }

        if (mGameEvents == null)
        {
            Log.Error(LogLevel.Critical, "BindEvent Failed,Game Event List is null!");
            return;
        }

        if (mGameEvents.ContainsKey(evtName))
        {
            Log.Error(LogLevel.Normal, "BindEvent Failed, repeated event name:" + evtName);
            return;
        }

        GameEventSystem.Ins.AddEventListen(this, evtName, action);
    }

    public void RemoveEvent(string evtName)
    {
        if (string.IsNullOrEmpty(evtName))
        {
            Log.Error(LogLevel.Normal, "RemoveEvent Failed, null or empty string is invalid event name!");
            return;
        }

        if (mGameEvents == null)
        {
            Log.Error(LogLevel.Critical, "RemoveEvent Failed, Game Event List is null!");
            return;
        }

        if (mGameEvents.ContainsKey(evtName))
        {
            mGameEvents.Remove(evtName);
            GameEventSystem.Ins.RemoveEventListen(this, evtName);
        }

    }

    public void ClearEvents()
    {
        if (mGameEvents == null)
            return;

        foreach (string evtName in mGameEvents.Keys)
        {
            GameEventSystem.Ins.RemoveEventListen(this, evtName);
        }
    }
}
