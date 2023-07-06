using System.Collections.Generic;
using GameSkillEffect;

[System.Serializable]
public class SkillEffectData
{
    /// <summary>
    /// 这个配置数据的备注没有实际意义
    /// </summary>
    public string comment;

    /// <summary>
    /// 效果的释放对象
    /// </summary>
    public EffectTgt rlsTgt = EffectTgt.Enemy;

    /// <summary>
    /// 指向效果配置数据的UID
    /// </summary>
    public uint effectCfgUID;

    /// <summary>
    /// string 类型的初始值字典
    /// </summary>
    public Dictionary<string, string> strValueDict;

    /// <summary>
    /// int 类型的初始值字典
    /// </summary>
    public Dictionary<string, int> intValueDict;

    /// <summary>
    /// float 类型的初始值字典
    /// </summary>
    public Dictionary<string, float> floatValueDict;

    /// <summary>
    /// 这个技能效果携带的子技能效果
    /// </summary>
    public SkillEffectData[] subEffects;

    /// <summary>
    /// 技能效果的碰撞形状
    /// </summary>
    public SkEftHitShapeData hitShape;

}
