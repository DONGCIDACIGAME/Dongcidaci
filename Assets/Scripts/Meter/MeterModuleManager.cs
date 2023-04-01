using GameEngine;

public abstract class MeterModuleManager<T> : ModuleManager<T>, IMeterHandler
    where T : new()
{
    public abstract uint GetMeterOffset();

    public abstract void OnMeter(int curMeterIndex);
}
