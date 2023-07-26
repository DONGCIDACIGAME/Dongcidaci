public abstract class InputHandle : IInputHandle
{
    protected bool mEnable;
    public bool CheckEnable()
    {
        return mEnable;
    }

    public void SetEnable(bool enable)
    {
        mEnable = enable;
    }

    public abstract string GetHandleName();

    public abstract void OnUpdate(float deltaTime);

    public abstract void OnMeterEnter(int meterIndex);

    public abstract void OnMeterEnd(int meterIndex);

    public abstract void OnDisplayPointBeforeMeterEnter(int meterIndex);
}