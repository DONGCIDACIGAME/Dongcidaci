using System.Collections.Generic;

public class AgentCommandBufferPool
{
    private Stack<AgentCommandBuffer> mPool;

    public AgentCommandBufferPool()
    {
        mPool = new Stack<AgentCommandBuffer>();
    }

    public void PushAgentCommandBuffer(AgentCommandBuffer buffer)
    {
        if (buffer == null)
            return;
        mPool.Push(buffer);
    }


    public AgentCommandBuffer PopAgentCommandBuffer()
    {
        if (mPool.Count == 0)
            return new AgentCommandBuffer();

        AgentCommandBuffer buffer = mPool.Pop();
        if (buffer == null)
            return new AgentCommandBuffer();

        buffer.ClearBuffer();
        return buffer;
    }
}
