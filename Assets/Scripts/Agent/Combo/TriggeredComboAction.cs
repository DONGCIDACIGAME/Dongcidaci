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

    /// <summary>
    /// combo name
    /// </summary>
    public string comboName { get; private set; }

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
        this.comboName = string.Empty;
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
    public void Initialize(uint agentId, string comboName, int actionIndex, int triggerMeter, ComboActionData comboAction)
    {
        this.agentId = agentId;
        this.comboName = comboName;
        this.actionIndex = actionIndex;
        this.triggeredMeter = triggeredMeter;
        this.actionData = comboAction;
    }

    public void Dispose()
    {
        this.agentId = 0;
        this.comboName = string.Empty;
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