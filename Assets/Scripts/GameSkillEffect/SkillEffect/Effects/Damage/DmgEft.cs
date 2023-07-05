using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSkillEffect
{
    public class DmgEft : SkillEffect
    {
        private int _dmgValue;
        public int DmgValue { get { return _dmgValue; } set { _dmgValue = value; } }

        public override bool InitSkEft(Agent user, SkillEffectData initData)
        {
            this._initSkEftData = initData;
            this._eftUser = user;

            //1 初始化这个伤害值
            if (initData.effectValueDict.ContainsKey("value") == false)
            {
                Log.Error(LogLevel.Critical, "Init DmgEft error no value in dict");
                return false;
            }

            this._dmgValue = Mathf.RoundToInt(initData.effectValueDict["value"]);

            // 2 init collide shape
            this._eftCollideShape = GameColliderHelper.GetRegularShapeWith(
                initData.hitShapeType,
                new Vector2(initData.hitOffsetX, initData.hitOffsetY),
                new Vector2(initData.hitSizeX, initData.hitSizeY)
                );

            return true;
        }

        public override void TriggerSkEft()
        {
            Log.Logic(LogLevel.Info, "TriggerSkEft Damage Trigger");
            var tgtAgents = GetHitAgents();
            if (tgtAgents == null || tgtAgents.Count == 0)
            {
                Log.Logic(LogLevel.Info, "TriggerSkEft Damage -- no tgt in damage area");
                return;
            }

            foreach (var tgtAgt in tgtAgents)
            {
                this._eftUser.SkillEftHandler.OnApplyDamage(tgtAgt,this);
            }

        }


        public override void Dispose()
        {
            this._eftCollideShape = null;
            this._eftUser = null;
            this._initSkEftData = null;
        }

        public override void Recycle()
        {
            Dispose();
            GameSkEftPool.Ins.Push<DmgEft>(this);
        }

        public override void RecycleReset()
        {
            Dispose();
        }

    }
}


