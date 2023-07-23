using System.Collections.Generic;
using GameEngine;
using UnityEngine;

public interface IAgentStatus: IGameUpdate, IMeterHandler, IGameDisposable
{
    void OnEnter(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComnboStep);
    string GetStatusName();
    void OnExit();
}
