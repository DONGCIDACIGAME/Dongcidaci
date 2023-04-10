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

    public abstract void OnMeter(int meterIndex);

    public abstract void OnUpdate(float deltaTime); 
}