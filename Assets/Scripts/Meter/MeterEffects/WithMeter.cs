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
    /// 3拍音乐的触发设置
    /// </summary>
    public int[] Trigger_3Beat;

    /// <summary>
    /// 4拍音乐的触发设置
    /// </summary>
    public int[] Trigger_4Beat;

    /// <summary>
    /// 8拍音乐的触发设置
    /// </summary>
    public int[] Trigger_8Beat;

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

    protected bool CheckTrigger(int meterIndex)
    {
        int rhythmType = MeterManager.Ins.GetCurrentMusicRhythmType();
        if (rhythmType == MeterDefine.RhythmType_Unknown)
            return false;

        if(rhythmType == MeterDefine.RhythmType_3Beat && Trigger_3Beat != null && Trigger_3Beat.Length == 3)
        {
            int localIndex = meterIndex % 3;
            return Trigger_3Beat[localIndex] == 1;
        }

        if (rhythmType == MeterDefine.RhythmType_4Beat && Trigger_4Beat != null && Trigger_4Beat.Length == 4)
        {
            int localIndex = meterIndex % 4;
            return Trigger_4Beat[localIndex] == 1;
        }

        if (rhythmType == MeterDefine.RhythmType_8Beat && Trigger_8Beat != null && Trigger_8Beat.Length == 8)
        {
            int localIndex = meterIndex % 8;
            return Trigger_8Beat[localIndex] == 1;
        }

        return false;
    }

    private void OnDestroy()
    {
        UpdateCenter.Ins.UnregisterUpdater(this);
        MeterManager.Ins.UnregiseterMeterHandler(this);
        Dispose();
    }

    public abstract void OnMeter(int meterIndex);
}
