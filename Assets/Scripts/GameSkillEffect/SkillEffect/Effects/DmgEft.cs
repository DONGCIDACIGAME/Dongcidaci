using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgEft : SkillEffect
{
    private int _dmgValue;
    public int DmgValue{get{ return _dmgValue; } set { _dmgValue = value; } }

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
            new Vector2(initData.hitOffsetX,initData.hitOffsetY),
            new Vector2(initData.hitsizeX, initData.hitSizeY)
            );

        return true;
    }

    public override void TriggerSkEft()
    {
        var tgtAgents = GetHitAgents();
        if (tgtAgents == null || tgtAgents.Count == 0) return;



        foreach (var tgtAgt in tgtAgents)
        {
            GameBattleManager.Ins.OnApplyDmgToTgt(_eftUser,tgtAgt,this);
        }

    }


}
