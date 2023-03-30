using System.Collections.Generic;

namespace GameEngine
{
    public interface IGameEventListen
    {
        void BindEvent(string evtName, GameEventAction action);
        void ClearEvents();
    }
}
