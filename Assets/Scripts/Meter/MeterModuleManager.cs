using GameEngine;

public abstract class MeterModuleManager<T> : ModuleManager<T>, IMeterHandler
    where T : new()
{
    public abstract void OnMeterEnd(int meterIndex);

    public abstract void OnMeterEnter(int curMeterIndex);
}
