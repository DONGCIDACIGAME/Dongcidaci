using System.Collections.Generic;

public class EmptyInputState : InputState
{
    public EmptyInputState(int priority) : base(priority)
    {
        validInputCtlNames = new HashSet<string>
        {
            
        };
    }

    public override string GetStateName()
    {
        return "EMPTY_STATE";
    }
}
