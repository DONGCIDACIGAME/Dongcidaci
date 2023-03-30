public class AgentStateAnimQueue
{
    private AgentAnimStateInfo[] mAnimStates;
    private int curStateIndex;



    public AgentStateAnimQueue(AgentAnimStateInfo[] animStates)
    {
        mAnimStates = animStates;
        curStateIndex = 0;
    }


}
