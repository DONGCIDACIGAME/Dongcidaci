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

        if (mPool.Contains(cmd))
            return;

        mPool.Push(cmd);
    }

    public AgentInputCommand CreateAgentInputCommandCopy(AgentInputCommand cmd)
    {
        if (cmd == null)
            return null;

        AgentInputCommand newCmd = PopAgentInputCommand();
        newCmd.Copy(cmd);
        return newCmd;
    }

    public AgentInputCommand PopAgentInputCommand()
    {
        if(mPool.TryPop(out AgentInputCommand cmd))
        {
            if (cmd != null)
            {
                return cmd;
            }
        }

        return new AgentInputCommand();
    }
}
