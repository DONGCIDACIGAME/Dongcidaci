using UnityEngine;

public static class InputDef
{
    public const string KeyboardInput = "KeyboardInput";

    public const string AgentKeyboardInputHandle_Idle               = "AgentKeyboardInputHandle_Idle";
    public const string AgentKeyboardInputHandle_Run                = "AgentKeyboardInputHandle_Run";
    public const string AgentKeyboardInputHandle_MeterRun           = "AgentKeyboardInputHandle_MeterRun";
    public const string AgentKeyboardInputHandle_Dash               = "AgentKeyboardInputHandle_Dash";
    public const string AgentKeyboardInputHandle_InstantAttack      = "AgentKeyboardInputHandle_InstantAttack";
    public const string AgentKeyboardInputHandle_MeterAttack        = "AgentKeyboardInputHandle_MeterAttack";
    public const string AgentKeyboardInputHandle_Charging           = "AgentKeyboardInputHandle_Charging";
    public const string AgentKeyboardInputHandle_ChargingAttack     = "AgentKeyboardInputHandle_ChargingAttack";
    public const string AgentKeyboardInputHandle_BeHit              = "AgentKeyboardInputHandle_BeHit";
    public const string AgentKeyboardInputHandle_Transfer           = "AgentKeyboardInputHandle_Transfer";
    public const string AgentKeyboardInputHandle_Transition         = "AgentKeyboardInputHandle_Transition";


    public const string MouseInput = "MouseInput";

    public const string AgentMouseInputHandle_Attack = "AgentMouseInputHandle_Attack";
    public const string MouseInputHandle_CommonInput = "MouseInputHandle_CommonInput";



    public const string JoystickInput = "JoystickInput";

    public static KeyCode DashKeyCode = KeyCode.Space;
    public static KeyCode InstantAttackKeyCode = KeyCode.J;
    public static KeyCode ChargingAttackKeyCode = KeyCode.K;

}
