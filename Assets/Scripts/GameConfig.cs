
public class GameConfig
{
    public int PlayerMode = GamePlayerMode.SINGLE_PLAYER;
    public int DifficultyMode = GameDifficuty.SIMPLE;

    public int Player1ControlMode = PlayerControlSource.KEYBOARD;
    public int Player2ControlMode = PlayerControlSource.KEYBOARD;
}

public static class GamePlayerMode
{
    public static int SINGLE_PLAYER = 0;
    public static int DOUBLE_PLAYER = 1;
}

public static class PlayerControlSource
{
    public static int KEYBOARD = 0;
    public static int JOYSTICK = 1;
    public static int TOUCH = 2;
}

public static class GameDifficuty
{
    public static int SIMPLE = 0;
    public static int NORMAL = 1;
    public static int HARD = 2;
    public static int EXTREMHARD = 3;
}


public static class GameCommonConfig
{
    public static float AgentTurnSpeed = 1080f;
}
