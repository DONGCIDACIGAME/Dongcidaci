using System;

public class PostEvent
{
    public string EventName { get; private set; }
    public Action Callback { get; private set; }


    public PostEvent(string evtName, Action callback)
    {
        EventName = evtName;
        Callback = callback;
    }
}
