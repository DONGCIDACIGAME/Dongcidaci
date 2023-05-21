using GameEngine;

public class TriggeredComboAction : IRecycle
{
    /// <summary>
    /// combo action data
    /// </summary>
    public ComboActionData actionData { get; private set; }

    /// <summary>
    /// agent id
    /// </summary>
    public uint agentId { get; private set; }

    public ComboData comboData { get; private set; }

    /// <summary>
    /// which meter is the combo action triggered
    /// </summary>
    public int triggeredMeter { get; private set; }


    /// <summary>
    /// action index in combo
    /// </summary>
    public int actionIndex { get; private set; }

    public TriggeredComboAction()
    {
        this.agentId = 0;
        this.comboData = null;
        this.actionIndex = -1;
        this.actionData = null;
        this.triggeredMeter = -1;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="agentId"></param>
    /// <param name="comboName"></param>
    /// <param name="actionIndex"></param>
    /// <param name="comboAction"></param>
    public void Initialize(uint agentId, ComboData comboData, int actionIndex, int triggerMeter, ComboActionData comboAction)
    {
        this.agentId = agentId;
        this.comboData = comboData;
        this.actionIndex = actionIndex;
        this.triggeredMeter = triggeredMeter;
        this.actionData = comboAction;
    }

    public void Dispose()
    {
        this.agentId = 0;
        this.comboData = null;
        this.actionIndex = -1;
        this.triggeredMeter = -1;
        this.actionData = null;
    }

    public void Recycle()
    {
        Dispose();

        GamePoolCenter.Ins.TriggeredComboActionPool.Push(this);
    }
}