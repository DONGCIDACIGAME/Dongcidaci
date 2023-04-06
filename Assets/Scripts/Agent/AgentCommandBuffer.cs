public class AgentCommandBuffer
{
    private byte _command;

    public void AddCommand(byte command)
    {
        _command |= command;
    }

    /// <summary>
    /// �ж��Ƿ���ָ��
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
    /// ���ָ���
    /// TODO: Ŀǰ��ȫ��buffer���ǲ��Եģ�Ӧ���и����棬������Ǹպÿ��ڽ����ϰ����˹��������ͻᱻ����Ϊidle״̬
    /// </summary>
    public void ClearBuffer()
    {
        _command = 0;
    }
}

