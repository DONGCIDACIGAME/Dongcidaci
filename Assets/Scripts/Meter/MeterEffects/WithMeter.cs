using UnityEngine;

public abstract class WithMeter : MonoBehaviour, IGameUpdate, IMeterHandler
{
    /// <summary>
    /// ʱ����������ʱ����
    /// </summary>
    protected float meterDuration;
    /// <summary>
    /// ��ʱ��
    /// </summary>
    protected float timeRecord;

    /// <summary>
    /// �������� 
    /// �����ʾʱ��Ĺ�һ��
    /// �����ʾ���Ž��ȵĹ�һ��
    /// </summary>
    public AnimationCurve Curve;

    public abstract void OnUpdate(float deltaTime);
    protected virtual void Initialize() { }
    protected virtual void Dispose() { }
       

    private void Start()
    {
        UpdateCenter.Ins.RegisterUpdater(this);
        MeterManager.Ins.RegisterMeterHandler(this);
        Initialize();
    }

    private void OnDestroy()
    {
        UpdateCenter.Ins.UnregisterUpdater(this);
        MeterManager.Ins.UnregiseterMeterHandler(this);
        Dispose();
    }

    public abstract void OnMeter(int meterIndex);
}
