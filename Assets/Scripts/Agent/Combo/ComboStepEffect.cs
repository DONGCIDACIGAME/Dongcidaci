[System.Serializable]
public class ComboStepEffect
{
    /// <summary>
    /// 这里的模式只针对效果
    /// 0:独占模式 效果只由combo配置出来
    /// 1:叠加模式 效果为基础效果+combo配置的效果
    /// </summary>
    public int mode;

    /// <summary>
    /// 效果类型
    /// </summary>
    public int effectType;

    /// <summary>
    /// 效果数值
    /// </summary>
    public int effectValue;
}
