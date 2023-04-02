using System.Collections.Generic;

public interface IAgentStatus: IGameUpdate, IMeterHandler
{
    void OnEnter(Dictionary<string,object> context);
    string GetStatusName();
    void OnAction(int action);
    void OnExit();
}
