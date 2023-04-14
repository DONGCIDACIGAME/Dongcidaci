using UnityEngine;

public abstract class WithMeter : MonoBehaviour, IGameUpdate, IMeterHandler
{
    public abstract void OnUpdate(float deltaTime);

    private int updaterIndex;
    private void Start()
    {
        updaterIndex = UpdateCenter.Ins.RegisterUpdater(this);
        MeterManager.Ins.RegisterMeterHandler(this);
    }

    private void OnDestroy()
    {
        UpdateCenter.Ins.UnregisterUpdater(updaterIndex);
        MeterManager.Ins.UnregiseterMeterHandler(this);
    }

    public abstract void OnMeter(int meterIndex);
}
