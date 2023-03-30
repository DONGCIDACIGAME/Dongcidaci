using System.Collections.Generic;

public class GameDemoInputState : InputState
{
    public GameDemoInputState(int priority):base(priority)
    {
        validInputCtlNames = new HashSet<string>()
        {
            InputDef.PlayerKeyboardInputCtlName,
        };
    }

    public override string GetStateName()
    {
        return "GAME_DEMO_STATE";
    }
}
