using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect
{
    protected SkillEffectData _initSkEftData;
    protected Agent _eftUser;

    public SkillEffect(SkillEffectData initSkEftData,Agent userAgt)
    {
        this._initSkEftData = initSkEftData;
        this._eftUser = userAgt;

        //InitSkEftWithValueStr(initSkEftData.effectValueStr);
        TriggerSkEft();
    }

    public abstract void InitSkEftWithValueStr(string valueStr);
    public abstract void TriggerSkEft();



}
