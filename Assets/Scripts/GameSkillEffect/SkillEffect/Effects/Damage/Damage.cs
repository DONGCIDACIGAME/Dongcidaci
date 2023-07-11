using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DongciDaci;

namespace GameSkillEffect
{
    public abstract class Damage : SkillEffect
    {
        /// <summary>
        /// 伤害实质上触发加载的特效
        /// 这个特效会随着不同的combo动态变化
        /// </summary>
        public FXConfigData DmgHitFXCfg;


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
            Log.Logic(LogLevel.Info, "TriggerSkEft Damage Trigger");

            var tgtAgents = SkEftHelper.GetHitAgents(_eftUser,_eftCollideShape,_initSkEftData.rlsTgt);
            if (tgtAgents == null || tgtAgents.Count == 0)
            {
                Log.Logic(LogLevel.Info, "TriggerSkEft Damage -- no tgt in damage area");
                Recycle();
                return;
            }

            foreach (var tgtAgt in tgtAgents)
            {
                _eftUser.SkillEftHandler.OnApplyDamage(tgtAgt, this);
            }

            Recycle();

        }


    }



}


