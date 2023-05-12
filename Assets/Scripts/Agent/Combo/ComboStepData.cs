[System.Serializable]
public class ComboStepData
{
    /// <summary>
    /// 输入类型
    /// </summary>
    public byte input;

    /// statusName+stateName能够匹配出一个动画状态
    
    /// <summary>
    /// 状态名称
    /// </summary>
    public string statusName;

    /// <summary>
    /// 状态的动画名称
    /// 如果不配置stateName，就使用status的第一个state
    /// </summary>
    public string stateName;

    /// <summary>
    /// 这里的模式只针对效果
    /// 0:独占模式 效果只由combo配置出来
    /// 1:叠加模式 效果为基础效果+combo配置的效果
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
