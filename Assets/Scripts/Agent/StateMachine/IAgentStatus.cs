using System.Collections.Generic;
using GameEngine;
using UnityEngine;

public interface IAgentStatus: IGameUpdate, IMeterHandler, IGameDisposable
{
    void OnEnter(byte cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> context);
    string GetStatusName();
    void OnExit();
}
