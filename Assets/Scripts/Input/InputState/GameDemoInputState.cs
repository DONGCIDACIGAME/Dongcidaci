using System.Collections.Generic;

public class GameDemoInputState : InputState
{
    public GameDemoInputState(int priority):base(priority)
    {
        validInputCtlNames = new HashSet<string>()
        {
            InputDef.KeyboardInput,
        };
    }

    public override string GetStateName()
    {
        return "GAME_DEMO_STATE";
    }
}
