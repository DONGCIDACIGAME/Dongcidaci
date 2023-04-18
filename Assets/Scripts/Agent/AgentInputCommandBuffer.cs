using UnityEngine;

public class AgentInputCommandBuffer
{
    private byte cmdList;
    private Vector3[] directionList;

    public AgentInputCommandBuffer()
    {
        this.cmdList = 0;
        this.directionList = new Vector3[8];
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
    public void AddInputCommand(AgentInputCommand cmd)
    {
        if (cmd == null)
            return;

        cmdList |= cmd.CmdType;
        int index = GetBufferIndex(cmd.CmdType);
        if(index >= 0 && index < 8)
        {
            directionList[index] = cmd.Towards;
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

    public bool PeekCommand(out byte cmdType, out Vector3 towards)
    {
        cmdType = AgentCommandDefine.EMPTY;
        towards = GamePlayDefine.InputDirection_NONE;

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
        }
    }
}
