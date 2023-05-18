using GameEngine;
using UnityEngine;

public class AgentInputCommand : IRecycle
{
    public byte CmdType { get; private set; }
    public int TriggerMeter { get; private set; }
    public Vector3 Towards { get; private set; }

    public void Initialize(byte cmdType, int triggerMeter,Vector3 towards)
    {
        this.CmdType = cmdType;
        this.TriggerMeter = triggerMeter;
        this.Towards = towards;
    }

    public bool Equals(AgentInputCommand other)
    {
        if (other == null)
            return false;

        return CmdType == other.CmdType && TriggerMeter == other.TriggerMeter && Towards.Equals(other.Towards);
    }

    public void Clear()
    {
        this.CmdType = AgentCommandDefine.EMPTY;
        this.Towards = GamePlayDefine.InputDirection_NONE;
    }

    public AgentInputCommand Copy(AgentInputCommand other)
    {
        if (other == null)
            return null;

        CmdType = other.CmdType;
        TriggerMeter = other.TriggerMeter;
        Towards = other.Towards;
        return this;
    }

    public void Recycle()
    {
        Clear();
        GamePoolCenter.Ins.AgentInputCommandPool.Push(this);
    }
}
