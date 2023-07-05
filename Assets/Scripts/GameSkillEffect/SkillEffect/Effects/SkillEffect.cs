using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

public abstract class SkillEffect : IRecycle
{
    protected Agent _eftUser;
    protected SkillEffectData _initSkEftData;
    protected IConvex2DShape _eftCollideShape;

    public abstract bool InitSkEft(Agent user, SkillEffectData initData);
    public abstract void TriggerSkEft();

    public abstract void Dispose();
    public abstract void Recycle();
    public abstract void RecycleReset();

    
}
