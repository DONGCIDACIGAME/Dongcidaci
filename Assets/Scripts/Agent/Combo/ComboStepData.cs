[System.Serializable]
public class ComboStepData
{
    /// <summary>
    /// 输入类型
    /// </summary>
    public byte input;

    /// <summary>
    /// 播放的动画state
    /// </summary>
    public string animState;

    /// <summary>
    /// 0:独占模式
    /// 1:叠加模式
    /// </summary>
    public int mode;

    /// <summary>
    /// 招式效果
    /// </summary>
    public ComboStepEffect[] effects;

    /// <summary>
    /// 结束combo的标志
    /// </summary>
    public bool endFlag;

    /// <summary>
    /// 节拍长度
    /// </summary>
    public int meterLen;
}
