using GameEngine;

public class TriggeredComboStep : IRecycle
{
    /// <summary>
    /// combo action data
    /// </summary>
    public ComboStep comboStep { get; private set; }

    /// <summary>
    /// agent id
    /// </summary>
    public uint agentId { get; private set; }

    /// <summary>
    /// which combo is the combo step belong to
    /// </summary>
    public ComboData comboData { get; private set; }

    /// <summary>
    /// which meter is the combo step triggered
    /// </summary>
    public int triggeredMeter { get; private set; }

    /// <summary>
    /// which meter the combo will end
    /// </summary>
    public int endMeter { get; private set; }


    /// <summary>
    /// step index in combo
    /// </summary>
    public int stepIndex { get; private set; }

    public TriggeredComboStep()
    {
        this.agentId = 0;
        this.comboData = null;
        this.stepIndex = -1;
        this.comboStep = null;
        this.triggeredMeter = -1;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="agentId"></param>
    /// <param name="comboName"></param>
    /// <param name="stepIndex"></param>
    /// <param name="comboStep"></param>
    public void Initialize(uint agentId, ComboData comboData, int stepIndex, int triggerMeter, int endMeter, ComboStep comboStep)
    {
        this.agentId = agentId;
        this.comboData = comboData;
        this.stepIndex = stepIndex;
        this.triggeredMeter = triggeredMeter;
        this.endMeter = endMeter;
        this.comboStep = comboStep;
    }

    public void Dispose()
    {
        this.agentId = 0;
        this.comboData = null;
        this.stepIndex = -1;
        this.triggeredMeter = -1;
        this.endMeter = -1;
        this.comboStep = null;
    }

    public void Recycle()
    {
        Dispose();

        GamePoolCenter.Ins.TriggeredComboActionPool.Push(this);
    }
}