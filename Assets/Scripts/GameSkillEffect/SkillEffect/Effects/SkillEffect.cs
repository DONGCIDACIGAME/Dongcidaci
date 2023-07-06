using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;
using DongciDaci;

public abstract class SkillEffect : IRecycle
{
    /// <summary>
    /// 这个效果的释放者
    /// </summary>
    protected Agent _eftUser;

    /// <summary>
    /// 这个效果的初始化数据
    /// </summary>
    protected SkillEffectData _initSkEftData;

    /// <summary>
    /// 这个效果的碰撞形状定义
    /// </summary>
    protected IConvex2DShape _eftCollideShape;

    /// <summary>
    /// 这个效果的固定基础配置数据
    /// </summary>
    protected SkEftBaseCfg _eftBsCfg;

    public abstract bool InitSkEft(Agent user, SkillEffectData initData,SkEftBaseCfg eftBsCfg);
    public abstract void TriggerSkEft();

    public abstract void Dispose();
    public abstract void Recycle();
    public abstract void RecycleReset();

    
}
