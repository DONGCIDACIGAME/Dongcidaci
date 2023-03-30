using System.Collections.Generic;

public abstract class AgentStatus : IAgentStatus
{
    protected ChangeStatusDelegate ChangeStatus;
    protected Agent mAgent;

    protected string mCurAnimStateName;
    protected int mCurAnimStateLoopTime;
    protected int mCurAnimStateLoopRecord;

    public void Initialize(Agent agt,ChangeStatusDelegate cb)
    {
        ChangeStatus = cb;
        mAgent = agt;
    }

    public abstract string GetStatusName();

    public abstract void OnAction(int action);

    public abstract void OnEnter(Dictionary<string, object> context);

    public abstract void OnExit();

    public abstract void OnUpdate(float deltaTime); 

}
