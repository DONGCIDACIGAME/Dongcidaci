using UnityEngine;

public abstract class BehaviourWithMeter : MonoBehaviour, IGameUpdate, IMeterHandler
{

    /// <summary>
    /// 是否允许Update
    /// </summary>
    public bool UpdateEnable = true;

    /// <summary>
    /// 拍子是否触发
    /// </summary>
    protected bool meterTriggered;

    /// <summary>
    /// 是否使用自定义节拍触发
    /// </summary>
    public bool UseCustomMeterTrigger;

    /// <summary>
    /// 自定义的节拍触发
    /// </summary>
    public int[] CustomBehaviourMeterTrigger;

    /// <summary>
    /// 计时器
    /// </summary>
    protected float timeRecord;

    public abstract void OnUpdate(float deltaTime);
    protected virtual void Initialize() { }
    protected virtual void Dispose() { }
       

    private void Start()
    {
        UpdateCenter.Ins.RegisterUpdater(this);
        MeterManager.Ins.RegisterMeterHandler(this);
        Initialize();
    }

    protected bool CheckTrigger(int meterIndex)
    {
        if(UseCustomMeterTrigger)
        {
            if(CustomBehaviourMeterTrigger == null || CustomBehaviourMeterTrigger.Length == 0)
            {
                return false;
            }

            int localIndex = meterIndex % CustomBehaviourMeterTrigger.Length;
            return CustomBehaviourMeterTrigger[localIndex] == 1;
        }
        else
        {
            return MeterManager.Ins.CheckTriggerSceneBehaviour(meterIndex);
        }
    }

    private void OnDestroy()
    {
        UpdateCenter.Ins.UnregisterUpdater(this);
        MeterManager.Ins.UnregiseterMeterHandler(this);
        Dispose();
    }

    public abstract void OnMeter(int meterIndex);
}
