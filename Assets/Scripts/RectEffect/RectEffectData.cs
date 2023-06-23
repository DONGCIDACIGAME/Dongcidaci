public enum EffectTgt
{
    //对敌人
    Enemy = 0,
    //对自己
    Self,
    //对包含自己的队友
    TeamIncludeSelf,
    //除自己外的其它单位
    AnyOthers,
    //所有对象包含自己
    All
}


[System.Serializable]
public class RectEffectData
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
    /// 效果字符串
    /// </summary>
    public string effectValueStr;

    /// <summary>
    /// hit点攻击范围的宽度
    /// </summary>
    public float hitRectWidth;

    /// <summary>
    /// hit点攻击范围的高度
    /// </summary>
    public float hitRectHeight;

    /// <summary>
    /// hit点攻击范围的中心偏移X
    /// </summary>
    public float hitRectOffsetX;

    /// <summary>
    /// hit点攻击范围的中心偏移Y
    /// </summary>
    public float hitRectOffsetY;


}
