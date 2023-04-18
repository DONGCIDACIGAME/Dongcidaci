using UnityEngine;

public class AgentInputCommand
{
    public byte CmdType { get; private set; }
    public Vector3 Towards { get; private set; }

    public void Initialize(byte cmdType, Vector3 towards)
    {
        this.CmdType = cmdType;
        this.Towards = towards;
    }

    public bool Equals(AgentInputCommand other)
    {
        if (other == null)
            return false;

        return CmdType == other.CmdType && Towards.Equals(other.Towards);
    }

    public void Clear()
    {
        this.CmdType = AgentCommandDefine.EMPTY;
        this.Towards = GamePlayDefine.InputDirection_NONE;
    }
}
