using System.Collections.Generic;

public interface IAgentStatus
{
    void OnEnter(Dictionary<string,object> context);
    string GetStatusName();
    void OnAction(int action);
    void OnMeter();
    void OnUpdate(float deltaTime);
    void OnExit();
}
