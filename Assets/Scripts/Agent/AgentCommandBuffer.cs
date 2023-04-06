public class AgentCommandBuffer
{
    private byte _cmdBuffer;

    public void AddCommand(byte command)
    {
        _cmdBuffer |= command;
    }

    public void AddCommandIfHas(AgentCommandBuffer commands, byte command)
    {
        if (commands == null)
            return;

        if(commands.HasCommand(command))
        {
            AddCommand(command);
        }
    }

    public void AddCommands(params byte[] commands)
    {
        if (commands == null)
            return;
        for(int i =0;i<commands.Length;i++)
        {
            AddCommand(commands[i]);
        }
    }

    public void AddCommands(AgentCommandBuffer commands)
    {
        if (commands == null)
            return;

        byte buffer = commands.GetBuffer();
        _cmdBuffer |= buffer;
    }



    public byte GetBuffer()
    {
        return _cmdBuffer;
    }

    /// <summary>
    /// 判断是否有指令
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public bool HasCommand(byte command)
    {
        return (_cmdBuffer & command) == command;
    }

   public byte PeekCommand()
    {
        for (int i = 7; i >= 0; i--)
        {
            byte command = (byte)((1 << i) & _cmdBuffer);
            if (command > 0)
            {
                _cmdBuffer ^= command;
                return command;
            }
        }

        return AgentCommandDefine.EMPTY;
    }

    /// <summary>
    /// 清除指令集合
    /// TODO: 目前是全清buffer，是不对的，应该有个缓存，如果不是刚好卡在节拍上按下了攻击键，就会被清理为idle状态
    /// </summary>
    public void ClearBuffer()
    {
        _cmdBuffer = 0;
    }
}

