using UnityEngine;
using GameEngine;
public class AgentInputCommandBuffer : IGameDisposable
{
    private byte cmdList;
    private Vector3[] directionList;
    private int[] triggerMeterList;

    public AgentInputCommandBuffer()
    {
        this.cmdList = 0;
        this.directionList = new Vector3[8];
        this. triggerMeterList = new int[8];
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
    public void AddInputCommand(byte cmdType, Vector3 towards, int triggerMeter)
    {
        cmdList |= cmdType;
        int index = GetBufferIndex(cmdType);
        if(index >= 0 && index < 8)
        {
            directionList[index] = towards;
            triggerMeterList[index] = triggerMeter;
        }
    }

    public void RemoveCmd(byte cmdType)
    {
        cmdList &= (byte)(~cmdType);

        int index = GetBufferIndex(cmdType);
        if (index >= 0 && index < 8)
        {
            directionList[index] = GamePlayDefine.InputDirection_NONE;
        }
    }

    public bool PeekCommand(out byte cmdType, out Vector3 towards, out int triggerMeter)
    {
        cmdType = AgentCommandDefine.EMPTY;
        towards = GamePlayDefine.InputDirection_NONE;
        triggerMeter = 0;

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
            directionList[i] = GamePlayDefine.InputDirection_NONE;
            triggerMeterList[i] = 0;
        }
    }

    public void Dispose()
    {
        cmdList = 0;
        directionList = null;
        triggerMeterList = null;
    }
}
