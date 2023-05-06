using System.Collections.Generic;
using GameEngine;
using UnityEngine;

public interface IAgentStatus: IGameUpdate, IMeterHandler, IGameDisposable
{
    void OnEnter(Dictionary<string,object> context);
    string GetStatusName();
    void OnNormalCommand(AgentInputCommand cmd);
    void OnComboCommand(AgentInputCommand cmd, TriggerableCombo combo);
    void OnExit();
}
