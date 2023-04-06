public class AgentCommandBuffer
{
    private byte _command;

    public void AddCommand(byte command)
    {
        _command |= command;
    }

    /// <summary>
    /// 判断是否有指令
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public bool HasCommand(byte command)
    {
        return (_command & command) == command;
    }

   public byte PeekCommand()
    {
        for (int i = 7; i >= 0; i--)
        {
            byte command = (byte)((1 << i) & _command);
            if (command > 0)
                return command;
        }

        return AgentCommandDefine.EMPTY;
    }

    /// <summary>
    /// 清除指令集合
    /// TODO: 目前是全清buffer，是不对的，应该有个缓存，如果不是刚好卡在节拍上按下了攻击键，就会被清理为idle状态
    /// </summary>
    public void ClearBuffer()
    {
        _command = 0;
    }
}

