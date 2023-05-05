using System.Collections.Generic;
using GameEngine;
using UnityEngine;

public interface IAgentStatus: IGameUpdate, IMeterHandler, IGameDisposable
{
    void OnEnter(Dictionary<string,object> context);
    string GetStatusName();
    void OnCommand(AgentInputCommand cmd);
    void OnComboMove(Combo combo, ComboMove comboMove, Vector3 towards);
    void OnExit();
}
