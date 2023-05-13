using UnityEngine;

public static class InputDef
{
    public const string KeyboardInput = "KeyboardInput";

    public const string KeyboardInputHandle_Idle = "KeyboardInputHandle_Idle";
    public const string KeyboardInputHandle_Run = "KeyboardInputHandle_Run";
    public const string KeyboardInputHandle_Dash = "KeyboardInputHandle_Dash";
    public const string KeyboardInputHandle_Attack = "KeyboardInputHandle_Attack";
    public const string KeyboardInputHandle_BeHit = "KeyboardInputHandle_BeHit";
    public const string KeyboardInputHandle_Transfer = "KeyboardInputHandle_Transfer";

    public static KeyCode DashKeyCode = KeyCode.Space;
    public static KeyCode AttackShortKeyCode = KeyCode.J;
    public static KeyCode AttackLongKeyCode = KeyCode.K;

}
