using System.Collections.Generic;

public abstract class InputControl : IInputControl
{
    protected Dictionary<string, IInputHandle> mHandleDic;

    public virtual void Initialize()
    {
        mHandleDic = new Dictionary<string, IInputHandle>();
    }

    public virtual void Dispose()
    {
        mHandleDic = null;
    }

    public abstract string GetInputControlName();

    public virtual void RegisterInputHandle(string handleName, IInputHandle handle)
    {
        if (handle == null)
            return;

        if (!mHandleDic.ContainsKey(handleName))
        {
            mHandleDic.Add(handleName, handle);
        }
    }

    public virtual void UnregisterInputHandle(string handleName)
    {
        if (mHandleDic.ContainsKey(handleName))
        {
            mHandleDic.Remove(handleName);
        }
    }

    public virtual void SetInputHandleEnable(string handleName, bool enable)
    {
        if (mHandleDic.TryGetValue(handleName, out IInputHandle handle))
        {
            handle.SetEnable(enable);
        }
    }

    public virtual void OnUpdate(float deltaTime)
    {
        foreach (KeyValuePair<string, IInputHandle> kv in mHandleDic)
        {
            IInputHandle handle = kv.Value;
            if (handle.CheckEnable())
            {
                handle.OnUpdate(deltaTime);
            }
        }
    }

    public virtual void OnMeterEnter(int meterIndex)
    {
        foreach (KeyValuePair<string, IInputHandle> kv in mHandleDic)
        {
            IInputHandle handle = kv.Value;
            if (handle.CheckEnable())
            {
                handle.OnMeterEnter(meterIndex);
            }
        }
    }

    public virtual void OnMeterEnd(int meterIndex)
    {
        foreach (KeyValuePair<string, IInputHandle> kv in mHandleDic)
        {
            IInputHandle handle = kv.Value;
            if (handle.CheckEnable())
            {
                handle.OnMeterEnd(meterIndex);
            }
        }
    }

    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        foreach (KeyValuePair<string, IInputHandle> kv in mHandleDic)
        {
            IInputHandle handle = kv.Value;
            if (handle.CheckEnable())
            {
                handle.OnDisplayPointBeforeMeterEnter(meterIndex);
            }
        }
    }
}
