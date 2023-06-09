using UnityEngine;
using GameEngine;
using System.Collections.Generic;

public class AgentCommandBuffer : IGameDisposable
{
    private byte cmdList;
    private Vector3[] directionList;
    private int[] triggerMeterList;
    private Dictionary<string, object>[] argsList;

    public AgentCommandBuffer()
    {
        this.cmdList = 0;
        this.directionList = new Vector3[8];
        this.triggerMeterList = new int[8];
        this.argsList = new Dictionary<string, object>[8]
        {
            new Dictionary<string, object>(),
            new Dictionary<string, object>(),
            new Dictionary<string, object>(),
            new Dictionary<string, object>(),
            new Dictionary<string, object>(),
            new Dictionary<string, object>(),
            new Dictionary<string, object>(),
            new Dictionary<string, object>()
        };
    }

    public bool HasCommand()
    {
        return cmdList != AgentCommandDefine.EMPTY;
    }

    private int GetBufferIndex(byte cmdType)
    {
        for(int i = 7; i >= 0; i--)
        {
            if((cmdType >> i) > 0)
            {
                return i;
            }
        }

        return -1;
    }
    public void AddInputCommand(byte cmdType, Vector3 towards, int triggerMeter, Dictionary<string,object> args)
    {
        // 记录指令类型
        cmdList |= cmdType;

        // 获取该指令的缓存index
        int index = GetBufferIndex(cmdType);
        if(index >= 0 && index < 8)
        {
            // 记录指令朝向
            directionList[index] = towards;
            // 记录指令触发节拍
            triggerMeterList[index] = triggerMeter;
            // 记录指令参数
            if(args != null && args.Count > 0)
            {
                Dictionary<string, object> _args = this.argsList[index];
                // 清除原来的参数
                _args.Clear();
                // 复制参数
                foreach (KeyValuePair<string,object> kv in args)
                {
                    _args[kv.Key] = kv.Value;
                }
            }
        }
    }

    public void RemoveCmd(byte cmdType)
    {
        cmdList &= (byte)(~cmdType);

        int index = GetBufferIndex(cmdType);
        if (index >= 0 && index < 8)
        {
            directionList[index] = DirectionDef.none;
            triggerMeterList[index] = 0;
            argsList[index].Clear();
        }
    }

    public bool PeekCommand(out byte cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string,object> args)
    {
        cmdType = AgentCommandDefine.EMPTY;
        towards = DirectionDef.none;
        triggerMeter = 0;
        args = null;

        for (int i = 7; i >= 0; i--)
        {
            byte _cmdType = (byte)((1 << i) & cmdList);
            if (_cmdType > 0)
            {
                cmdType = _cmdType;
                int index = GetBufferIndex(cmdType);
                if(index >= 0 && index < 8)
                {
                    towards = directionList[index];
                    triggerMeter = triggerMeterList[index];
                }
                return true;
            }
        }

        return false;
    }

    public void ClearCommandBuffer()
    {
        cmdList = 0;
        for(int i = 0; i < 8; i++)
        {
            directionList[i] = DirectionDef.none;
            triggerMeterList[i] = 0;
            argsList[i].Clear();
        }
    }

    public void Dispose()
    {
        cmdList = 0;
        directionList = null;
        triggerMeterList = null;
        argsList = null;
    }
}
