using UnityEngine;

public class KeyboardInputHandle_Attack : AgentKeyboardInputHandle
{
    public KeyboardInputHandle_Attack(Agent agt) : base(agt)
    {

    }

    public override string GetHandleName()
    {
        return InputDef.KeyboardInputHandle_Attack;
    }


    public override void OnMeter(int meterIndex)
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

        if (!towards.Equals(mAgent.GetTowards()))
        {
            mAgent.MoveControl.TurnTo(towards);
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        if (mAgent == null)
            return;

        AgentCommandBuffer cmds = mAgent.CommandBufferPool.PopAgentCommandBuffer();

        float tolerance = Mathf.Min(GamePlayDefine.AttackMeterCheckTolerance, GamePlayDefine.EmptyStatusMaxTime);
        bool inMeterTrigger = MeterManager.Ins.IsInMeterWithTolorance(MeterManager.Ins.MeterIndex, tolerance);
        if(inMeterTrigger)
        {
            cmds.AddCommand(AgentCommandDefine.EMPTY);
        }
        else
        {
            cmds.AddCommand(AgentCommandDefine.IDLE);
        }

        if (Input.GetKeyDown(InputDef.LightAttackKeyCode) && MeterManager.Ins.CheckTriggerCurrentMeter(GamePlayDefine.AttackMeterCheckTolerance))
        {
            cmds.AddCommand(AgentCommandDefine.ATTACK_LIGHT);
        }

        if (Input.GetKeyDown(InputDef.HardAttackKeyCode) && MeterManager.Ins.CheckTriggerCurrentMeter(GamePlayDefine.AttackMeterCheckTolerance))
        {
            cmds.AddCommand(AgentCommandDefine.ATTACK_HARD);
        }

        if (Input.GetKeyDown(InputDef.DashKeyCode) && MeterManager.Ins.CheckTriggerCurrentMeter(GamePlayDefine.DashMeterCheckTolerance))
        {
            cmds.AddCommand(AgentCommandDefine.DASH);
        }

        // 检测到移动的按键
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            // 添加移动指令
            cmds.AddCommand(AgentCommandDefine.RUN);
        }

        mAgent.OnCommands(cmds);
    }
}

