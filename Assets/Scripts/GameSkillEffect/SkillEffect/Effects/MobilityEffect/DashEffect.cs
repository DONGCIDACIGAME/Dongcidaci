
using DongciDaci;

namespace GameSkillEffect
{
    public class DashEffect : MobilityEffect
    {
        public float dashDistance = 0;
        public float dashSpeed = 0;

        public override bool InitSkEft(Agent user, SkillEffectData initData, SkEftBaseCfg eftBsCfg)
        {
            Log.Logic(LogLevel.Normal, "Init DashEffect -- Start");
            // 1 check data 
            if (!initData.floatValueDict.ContainsKey("distance") ||
                !initData.floatValueDict.ContainsKey("speed"))
            {
                return false;
            }

            //2 init data
            _eftUser = user;
            _initSkEftData = initData;
            _eftBsCfg = eftBsCfg;
            dashDistance = initData.floatValueDict["distance"];
            dashSpeed = initData.floatValueDict["speed"];

            return true;
        }

        public override void TriggerSkEft()
        {
            Log.Logic(LogLevel.Normal, "DashEffect -- Trigger, dis{0},speed{1}",dashDistance,dashSpeed);

            if (_eftUser.SkillEftHandler.OnApplyMoveEffect(this))
            {
                // 执行状态的切换
                /**
                AgentCommand dashCmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
                dashCmd.AddArg("distance",dashDistance);
                //dashCmd.AddArg("speed", dashSpeed);
                dashCmd.AddArg("startTime", 0f);
                dashCmd.AddArg("endTime", dashDistance/dashSpeed);
                dashCmd.AddArg("timeType", TimeDefine.TimeType_AbsoluteTime);
                dashCmd.Initialize(AgentCommandDefine.DASH, MeterManager.Ins.MeterIndex, TimeMgr.Ins.FrameIndex, _eftUser.GetTowards());
                _eftUser.OnCommand(dashCmd);
                */

                // 此处不在进行状态的切换，而是直接控制移动
                //_eftUser.MoveControl.MoveTowards(_eftUser.GetTowards(), dashDistance,0.4f);
                _eftUser.MovementExcutorCtl.Start(0f,0.4f, DirectionDef.FixedTowards, _eftUser.GetTowards(),5f);
            }

            Recycle();
        }

        public override void Dispose()
        {
            _eftUser = null;
            _initSkEftData = null;
            _eftBsCfg = null;
            dashDistance = 0;
            dashSpeed = 0;

        }

        
        public override void Recycle()
        {
            Dispose();
            // put into pool
            GameSkEftPool.Ins.Push(this);
        }

        public override void RecycleReset()
        {
            Dispose();
        }

        
    }
}


