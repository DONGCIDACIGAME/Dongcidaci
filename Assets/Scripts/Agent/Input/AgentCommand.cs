using GameEngine;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 整合所有指令类型
/// 1. 输入指令（例如从外设上输入的指令，不带指令参数）
/// 2. 逻辑指令（代码逻辑生成的逻辑指令，可以带指令参数）
/// </summary>
public class AgentCommand : IRecycle
{
    /// <summary>
    /// 指令类型
    /// </summary>
    public int CmdType { get; private set; }

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

    /// <summary>
    /// 指令参数
    /// </summary>
    public Dictionary<string, object> Args;

    public void Initialize(int cmdType, int triggerMeter, long frame, Vector3 towards)
    {
        this.CmdType = cmdType;
        this.TriggerMeter = triggerMeter;
        this.Towards = towards;
        this.Frame = frame;
    }

    public void AddArg(string key, object value)
    {
        if (Args == null)
            Args = new Dictionary<string, object>();

        Args[key] = value;
    }

    /// <summary>
    /// 判断指令是否一样
    /// 只判断到所在节拍
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(AgentCommand other)
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
    public bool AbsoluteEquals(AgentCommand other)
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
        this.Args = null;
    }

    /// <summary>
    /// 浅拷贝
    /// 不拷贝指令的参数数据
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public AgentCommand ShallowCopy(AgentCommand other)
    {
        if (other == null)
            return null;

        CmdType = other.CmdType;
        TriggerMeter = other.TriggerMeter;
        Towards = other.Towards;
        Frame = other.Frame;
        return this;
    }

    /// <summary>
    /// 深拷贝
    /// 指令参数也要拷贝
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public AgentCommand DeepCopy(AgentCommand other)
    {
        if (other == null)
            return null;

        CmdType = other.CmdType;
        TriggerMeter = other.TriggerMeter;
        Towards = other.Towards;
        Frame = other.Frame;
        if(other.Args != null && other.Args.Count > 0)
        {
            foreach(KeyValuePair<string,object> kv in other.Args)
            {
                AddArg(kv.Key, kv.Value);
            }
        }
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

        if (this.Args != null)
            this.Args.Clear();
    }
}
