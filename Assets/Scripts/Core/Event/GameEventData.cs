using System;

public class GameEventData
{
    public int listenerId { get; private set; }
    public Delegate callback { get; private set; }

    public GameEventData(int listenerId, Delegate callback)
    {
        this.listenerId = listenerId;
        this.callback = callback;
    }
}
