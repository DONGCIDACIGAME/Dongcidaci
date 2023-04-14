using UnityEngine;

public abstract class WithMeter : MonoBehaviour, IGameUpdate, IMeterHandler
{
    /// <summary>
    /// 时长（本节拍时长）
    /// </summary>
    protected float meterDuration;
    /// <summary>
    /// 计时器
    /// </summary>
    protected float timeRecord;

    /// <summary>
    /// 缩放曲线 
    /// 横轴表示时间的归一化
    /// 纵轴表示缩放进度的归一化
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
