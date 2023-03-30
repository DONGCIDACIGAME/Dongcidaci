using System.Collections.Generic;

public static class InputStateDefine
{
    private static int index = 0;
    public static IInputState EMPTY_STATE = new EmptyInputState(index++);
    public static IInputState GAMEDEMO_STATE = new GameDemoInputState(index++);
}
