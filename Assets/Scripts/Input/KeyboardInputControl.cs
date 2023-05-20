using System.Collections.Generic;

public class KeyboardInputControl : IInputControl
{
    protected Dictionary<string, IInputHandle> mHandleDic;

    public void Initialize()
    {
        mHandleDic = new Dictionary<string, IInputHandle>();
    }

    public void Dispose()
    {
        mHandleDic = null;
    }

    public string GetInputControlName()
    {
        return InputDef.KeyboardInput;
    }

    public void RegisterInputHandle(string handleName, IInputHandle handle)
    {
        if (handle == null)
            return;

        if(!mHandleDic.ContainsKey(handleName))
        {
            mHandleDic.Add(handleName, handle);
        }
    }

    public void UnregisterInputHandle(string handleName)
    {
        if (mHandleDic.ContainsKey(handleName))
        {
            mHandleDic.Remove(handleName);
        }
    }

    public void SetInputHandleEnable(string handleName, bool enable)
    {
        if(mHandleDic.TryGetValue(handleName, out IInputHandle handle))
        {
            handle.SetEnable(enable);
        }
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (KeyValuePair<string, IInputHandle> kv in mHandleDic)
        {
            IInputHandle handle = kv.Value;
            if(handle.CheckEnable())
            {
                handle.OnUpdate(deltaTime);
            }
        }
    }

    public void OnMeterEnter(int meterIndex)
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

    public void OnMeterEnd(int meterIndex)
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
}
