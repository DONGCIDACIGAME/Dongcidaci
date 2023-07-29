using UnityEngine;

public class AgentKeyboardInputHandle_Dash : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_Dash(Hero hero) : base(hero)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_Dash;
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

        mHero.MoveControl.TurnTo(towards);
    }
}
