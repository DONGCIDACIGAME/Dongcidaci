public static class ComboDefine
{
    /// <summary>
    /// combo触发成功
    /// </summary>
    public const int ComboTriggerResult_Succeed = 0;

    /// <summary>
    /// 不是combo的触发指令
    /// </summary>
    public const int ComboTriggerResult_NotComboTrigger = 1;

    /// <summary>
    /// 是combo的触发指令，但是上一个combo还在执行过程中（没到逻辑完成帧）
    /// </summary>
    public const int ComboTriggerResult_ComboExcuting = 2;

    /// <summary>
    /// 是combo的触发指令，但是没有匹配到combo
    /// </summary>
    public const int ComboTriggerResult_Failed = 3;


    /// <summary>
    /// 覆盖模式
    /// </summary>
    public const int ComboMode_Overwrite = 0;

    /// <summary>
    /// 叠加模式
    /// </summary>
    public const int ComboMode_Overlay= 1;

    /// <summary>
    /// 空招式
    /// </summary>
    public static TriggeredComboStep EmptyComboStep = new TriggeredComboStep();
}
