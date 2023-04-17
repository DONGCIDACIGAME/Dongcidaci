using System.Collections.Generic;
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

    public void Dispose()
    {
        this.CmdType = AgentCommandDefine.EMPTY;
        this.Towards = Vector3.zero;
    }
}
