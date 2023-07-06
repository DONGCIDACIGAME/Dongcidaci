using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class AgentInputCommand : IRecycle
{
    /// <summary>
    /// 指令类型
    /// </summary>
    public byte CmdType { get; private set; }

    /// <summary>
    /// 触发在第几拍
    /// </summary>
    public int TriggerMeter { get; private set; }

    /// <summary>
    /// 指令朝向
    /// </summary>
    public Vector3 Towards { get; private set; }

    /// <summary>
    /// 触发在第几帧
    /// </summary>
    public long Frame { get; private set; }

    public void Initialize(byte cmdType, int triggerMeter, long frame, Vector3 towards)
    {
        this.CmdType = cmdType;
        this.TriggerMeter = triggerMeter;
        this.Towards = towards;
        this.Frame = frame;
    }

    /// <summary>
    /// 判断指令是否一样
    /// 只判断到所在节拍
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(AgentInputCommand other)
    {
        if (other == null)
            return false;

        return CmdType == other.CmdType && TriggerMeter == other.TriggerMeter && Towards.Equals(other.Towards);
    }

    /// <summary>
    /// 严格判断指令是否一样
    /// 只判断到所在帧
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool AbsoluteEquals(AgentInputCommand other)
    {
        if(Equals(other))
        {
            return this.Frame == other.Frame;
        }

        return false;
    }

    public void Dispose()
    {
        this.CmdType = AgentCommandDefine.EMPTY;
        this.Towards = DirectionDef.none;
        this.TriggerMeter = -1;
        this.Frame = -1;
    }

    public AgentInputCommand Copy(AgentInputCommand other)
    {
        if (other == null)
            return null;

        CmdType = other.CmdType;
        TriggerMeter = other.TriggerMeter;
        Towards = other.Towards;
        Frame = other.Frame;
        return this;
    }

    public void Recycle()
    {
        RecycleReset();
        GamePoolCenter.Ins.AgentInputCommandPool.Push(this);
    }

    public void RecycleReset()
    {
        this.CmdType = AgentCommandDefine.EMPTY;
        this.Towards = DirectionDef.none;
        this.TriggerMeter = -1;
        this.Frame = -1;
    }
}
