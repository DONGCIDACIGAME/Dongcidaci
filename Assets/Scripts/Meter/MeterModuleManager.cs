using GameEngine;

public abstract class MeterModuleManager<T> : ModuleManager<T>
    where T : new()
{
    public abstract void OnMeter(int curMeterIndex);
}
