using UnityEngine;

public class AgentInputCommand
{
    public byte CmdType { get; private set; }
    public int TriggerIndex { get; private set; }
    public Vector3 Towards { get; private set; }

    public void Initialize(byte cmdType, int triggerIndex,Vector3 towards)
    {
        this.CmdType = cmdType;
        this.TriggerIndex = triggerIndex;
        this.Towards = towards;
    }

    public bool Equals(AgentInputCommand other)
    {
        if (other == null)
            return false;

        return CmdType == other.CmdType && TriggerIndex == other.TriggerIndex && Towards.Equals(other.Towards);
    }

    public void Clear()
    {
        this.CmdType = AgentCommandDefine.EMPTY;
        this.Towards = GamePlayDefine.InputDirection_NONE;
    }

    public void Copy(AgentInputCommand other)
    {
        if (other == null)
            return;

        CmdType = other.CmdType;
        TriggerIndex = other.TriggerIndex;
        Towards = other.Towards;
    }
}
