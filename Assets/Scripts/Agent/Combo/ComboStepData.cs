[System.Serializable]
public class ComboStepData
{
    /// <summary>
    /// 输入类型
    /// </summary>
    public byte input;

    /// <summary>
    /// 状态名称
    /// </summary>
    public string statusName;

    /// <summary>
    /// 状态的动画名称
    /// </summary>
    public string stateName;

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
}
