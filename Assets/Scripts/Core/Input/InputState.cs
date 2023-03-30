using System.Collections.Generic;

public abstract class InputState : IInputState
{
    protected int mPriority;
    public InputState(int priority)
    {
        mPriority = priority;
    }

    protected HashSet<string> validInputCtlNames;
    public virtual HashSet<string> GetValidInputCtlNames()
    {
        return validInputCtlNames;
    }

    public bool CheckInputControlEnable(string ctlName)
    {
        HashSet<string> validInputCtlNames = GetValidInputCtlNames();

        if (validInputCtlNames == null)
            return false;

        return validInputCtlNames.Contains(ctlName);
    }

    public abstract string GetStateName();

    public virtual int GetStatePriority()
    {
        return mPriority;
    }

    public virtual void OnDispose()
    {
        
    }

    public virtual void OnEnter()
    {

    }

    public virtual  void OnExit()
    {

    }
}
