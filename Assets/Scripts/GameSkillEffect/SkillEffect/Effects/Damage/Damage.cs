using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DongciDaci;

namespace GameSkillEffect
{
    public abstract class Damage : SkillEffect
    {
        protected int _dmgValue;
        public int DmgValue { get { return _dmgValue; } set { _dmgValue = value; } }

        public override bool InitSkEft(Agent user, SkillEffectData initData,SkEftBaseCfg eftCfg)
        {
            this._initSkEftData = initData;
            this._eftUser = user;
            this._eftBsCfg = eftCfg;

            //1 初始化这个伤害值
            if (initData.intValueDict.ContainsKey("value") == false)
            {
                Log.Error(LogLevel.Critical, "Init DmgEft error no value in dict");
                return false;
            }

            this._dmgValue = Mathf.RoundToInt(initData.intValueDict["value"]);

            // 2 init collide shape
            this._eftCollideShape = GameColliderHelper.GetRegularShapeWith(
                initData.hitShape.shapeType,
                new Vector2(initData.hitShape.offsetX, initData.hitShape.offsetY),
                new Vector2(initData.hitShape.sizeX, initData.hitShape.sizeY)
                );

            return true;
        }

        public override void TriggerSkEft()
        {
            Log.Logic(LogLevel.Info, "TriggerSkEft AttackDamage Trigger");
            var tgtAgents = SkEftHelper.GetHitAgents(_eftUser,_eftCollideShape,_initSkEftData.rlsTgt);
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




    }



}


