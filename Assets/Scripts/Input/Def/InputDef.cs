using UnityEngine;

public static class InputDef
{
    public const string KeyboardInput = "KeyboardInput";

    public const string KeyboardInputHandle_Idle = "KeyboardInputHandle_Idle";
    public const string KeyboardInputHandle_Run = "KeyboardInputHandle_Run";
    public const string KeyboardInputHandle_Dash = "KeyboardInputHandle_Dash";
    public const string KeyboardInputHandle_Attack = "KeyboardInputHandle_Attack";
    public const string KeyboardInputHandle_BeHit = "KeyboardInputHandle_BeHit";

    public static KeyCode DashKeyCode = KeyCode.Space;
    public static KeyCode LightAttackKeyCode = KeyCode.J;
    public static KeyCode HardAttackKeyCode = KeyCode.K;

}
