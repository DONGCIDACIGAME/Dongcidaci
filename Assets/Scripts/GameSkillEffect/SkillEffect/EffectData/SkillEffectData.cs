using System.Collections.Generic;
using GameSkillEffect;



[System.Serializable]
public class SkillEffectData
{
    /// <summary>
    /// 效果的释放对象
    /// </summary>
    public EffectTgt rlsTgt = EffectTgt.Enemy;

    /// <summary>
    /// 指向效果配置数据的UID
    /// </summary>
    public uint effectCfgUID;

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
    public float hitSizeX;

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
