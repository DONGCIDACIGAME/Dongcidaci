using System.Collections.Generic;

public enum EffectTgt
{
    // 对敌人
    Enemy = 0,
    // 对自己
    Self,
    // 仅队友不包含自己
    TeamWithoutSelf,
    // 对包含自己的队友
    TeamIncludeSelf,
    // 除自己外的其它单位
    AnyOthers,
    // 所有对象包含自己
    All
}


[System.Serializable]
public class SkillEffectData
{
    /// <summary>
    /// 效果的释放对象
    /// </summary>
    public EffectTgt rlsTgt = EffectTgt.Enemy;

    /// <summary>
    /// 效果类型
    /// </summary>
    public int effectType;

    /// <summary>
    /// 效果具体的配置值字典
    /// </summary>
    public Dictionary<string, float> effectValueDict;

    /// <summary>
    /// 碰撞的形状
    /// </summary>
    public Convex2DShapeType hitShapeType = Convex2DShapeType.Rect;

    /// <summary>
    /// 碰撞形状宽
    /// </summary>
    public float hitsizeX;

    /// <summary>
    /// 碰撞形状高
    /// </summary>
    public float hitSizeY;

    /// <summary>
    /// 碰撞形状偏移x
    /// </summary>
    public float hitOffsetX;

    /// <summary>
    /// 碰撞形状偏移y
    /// </summary>
    public float hitOffsetY;

}
