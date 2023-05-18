using GameEngine;

public class ExcutiveComboAction : IRecycle
{
    /// <summary>
    /// combo action data
    /// </summary>
    public ComboActionData actionData;

    /// <summary>
    /// agent id
    /// </summary>
    public uint agentId;

    /// <summary>
    /// combo name
    /// </summary>
    public string comboName;


    /// <summary>
    /// action index in combo
    /// </summary>
    public int actionIndex;

    public ExcutiveComboAction()
    {
        this.agentId = 0;
        this.comboName = string.Empty;
        this.actionIndex = -1;
        this.actionData = null;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="agentId"></param>
    /// <param name="comboName"></param>
    /// <param name="actionIndex"></param>
    /// <param name="comboAction"></param>
    public void Initialize(uint agentId, string comboName, int actionIndex, ComboActionData comboAction)
    {
        this.agentId = agentId;
        this.comboName = comboName;
        this.actionIndex = actionIndex;
        this.actionData = comboAction;
    }

    public void Dispose()
    {
        this.agentId = 0;
        this.comboName = string.Empty;
        this.actionIndex = -1;
        this.actionData = null;
    }

    public void Recycle()
    {
        Dispose();

        GamePoolCenter.Ins.ExcutiveComboActionPool.Push(this);
    }
}