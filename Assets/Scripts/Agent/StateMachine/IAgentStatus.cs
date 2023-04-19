using System.Collections.Generic;
using GameEngine;

public interface IAgentStatus: IGameUpdate, IMeterHandler, IGameDisposable
{
    void OnEnter(Dictionary<string,object> context);
    string GetStatusName();
    void OnCommand(AgentInputCommand cmd);
    void OnExit();
}
