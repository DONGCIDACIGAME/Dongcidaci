using System.Collections.Generic;
using GameEngine;

public class AgentInputCommandPool : Singleton<AgentInputCommandPool>
{
    private Stack<AgentInputCommand> mPool;

    public AgentInputCommandPool()
    {
        mPool = new Stack<AgentInputCommand>();
    }

    public void PushAgentInputCommand(AgentInputCommand cmd)
    {
        if (cmd == null)
            return;
        mPool.Push(cmd);
    }

    public AgentInputCommand PopAgentInputCommand()
    {
        if (mPool.Count == 0)
            return new AgentInputCommand();

        AgentInputCommand cmd = mPool.Pop();
        if (cmd == null)
            return new AgentInputCommand();

        cmd.Clear();
        return cmd;
    }
}
