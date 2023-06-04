using UnityEngine;

public class KeyboardInputHandle_Dash : AgentKeyboardInputHandle
{
    public KeyboardInputHandle_Dash(Agent agt) : base(agt)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.KeyboardInputHandle_Dash;
    }

    public override void OnMeterEnter(int meterIndex)
    {
        // 在节拍处检测方向，可以在节拍处改变攻击的方向
        Vector3 towards = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            towards += DirectionDef.up;
        }

        if (Input.GetKey(KeyCode.S))
        {
            towards += DirectionDef.down;
        }

        if (Input.GetKey(KeyCode.A))
        {
            towards += DirectionDef.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            towards += DirectionDef.right;
        }

        mAgent.MoveControl.TurnTo(towards);
    }

    public override void OnMeterEnd(int meterIndex)
    {

    }
    public override void OnUpdate(float deltaTime)
    {
        if (mAgent == null)
            return;

        AgentInputCommand cmd;
        bool hasCmd = GetAttackInputCmd(out cmd) || GetDashInputCommand(out cmd) || GetRunInputCmd(out cmd);
        if(hasCmd)
        {
            mAgent.OnCommand(cmd);
        }
    }
}
