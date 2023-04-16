using UnityEngine;

public abstract class BehaviourWithMeter : MonoBehaviour, IGameUpdate, IMeterHandler
{

    /// <summary>
    /// �Ƿ�����Update
    /// </summary>
    public bool UpdateEnable = true;

    /// <summary>
    /// �����Ƿ񴥷�
    /// </summary>
    protected bool meterTriggered;

    /// <summary>
    /// �Ƿ�ʹ���Զ�����Ĵ���
    /// </summary>
    public bool UseCustomMeterTrigger;

    /// <summary>
    /// �Զ���Ľ��Ĵ���
    /// </summary>
    public int[] CustomBehaviourMeterTrigger;

    /// <summary>
    /// ��ʱ��
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
