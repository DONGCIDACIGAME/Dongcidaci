using UnityEngine;
using GameEngine;
using System.Collections.Generic;

public class AgentCommandBuffer : IGameDisposable
{
    private int cmdList;
    private Vector3[] directionList;
    private int[] triggerMeterList;
    private Dictionary<string, object>[] argsList;
    private TriggeredComboStep[] comboStepList;

    public AgentCommandBuffer()
    {
        this.cmdList = 0;
        this.directionList = new Vector3[32];
        this.triggerMeterList = new int[32];
        this.argsList = new Dictionary<string, object>[32];
        this.comboStepList = new TriggeredComboStep[32];

        for (int i = 0;i<32;i++)
        {
            directionList[i] = DirectionDef.none;
            triggerMeterList[i] = 0;
            argsList[i] = new Dictionary<string, object>();
            comboStepList[i] = null;
        }
    }

    public bool HasCommand()
    {
        return cmdList != AgentCommandDefine.EMPTY;
    }

    private int GetBufferIndex(int cmdType)
    {
        for(int i = 31; i >= 0; i--)
        {
            if((cmdType >> i) > 0)
            {
                return i;
            }
        }

        return -1;
    }
    public void AddInputCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string,object> args, TriggeredComboStep comboStep)
    {
        // 记录指令类型
        cmdList |= cmdType;

        // 获取该指令的缓存index
        int index = GetBufferIndex(cmdType);
        if(index >= 0 && index < 32)
        {
            // 记录指令朝向
            directionList[index] = towards;
            // 记录指令触发节拍
            triggerMeterList[index] = triggerMeter;
            // 记录指令触发的combo招式
            comboStepList[index] = comboStep;
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

    public void RemoveCmd(int cmdType)
    {
        cmdList &= (~cmdType);

        int index = GetBufferIndex(cmdType);
        if (index >= 0 && index < 32)
        {
            directionList[index] = DirectionDef.none;
            triggerMeterList[index] = 0;
            argsList[index].Clear();
            comboStepList[index] = null;
        }
    }

    public bool PeekCommand(int afterMeter, out int cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string,object> args, out TriggeredComboStep comboStep)
    {
        cmdType = AgentCommandDefine.EMPTY;
        towards = DirectionDef.none;
        comboStep = null;
        triggerMeter = 0;
        args = null;

        for (int i = 32; i >= 0; i--)
        {
            int _cmdType = ((1 << i) & cmdList);
            if (_cmdType == 0)
                continue;

            int index = GetBufferIndex(_cmdType);
            if (index < 0 || index >= 32)
                continue;

            int _triggerMeter = triggerMeterList[index];
            if (_triggerMeter <= afterMeter)
                continue;

            cmdType = _cmdType;
            triggerMeter = _triggerMeter;
            towards = directionList[index];
            args = new Dictionary<string, object>();
            // 复制参数
            foreach (KeyValuePair<string, object> kv in argsList[index])
            {
                args[kv.Key] = kv.Value;
            }
            comboStep = comboStepList[index];
            return true;
        }

        return false;
    }

    public void ClearCommandBuffer()
    {
        cmdList = 0;
        for(int i = 0; i < 32; i++)
        {
            directionList[i] = DirectionDef.none;
            triggerMeterList[i] = 0;
            argsList[i].Clear();
            comboStepList[i] = null;
        }
    }

    public void Dispose()
    {
        cmdList = 0;
        directionList = null;
        triggerMeterList = null;
        argsList = null;
        comboStepList = null;
    }
}
