using System.Collections.Generic;
using GameEngine;


/// <summary>
/// TODO:这里的state添加和删除，考虑要改为状态机模式，不要做成列表叠加
/// state应该是一个栈的结构，或者只保留一个当前state，然后做个保底state
/// </summary>
public class InputManager : MeterModuleManager<InputManager>
{
    /// <summary>
    /// 存储了所有的inputcontrol，每个inputcontrol代表一个功能模块的输入控制
    /// 1.UI
    /// 2.角色控制
    /// 3.全局监听的一些功能按键
    /// </summary>
    private Dictionary<string, IInputControl> mInputControlMap;

    /// <summary>
    /// 当前存在所有输入状态
    /// </summary>
    private Stack<IInputState> mStateStatck;

    public override void Initialize()
    {
        if (mInputControlMap == null)
        {
            mInputControlMap = new Dictionary<string, IInputControl>();
        }

        if (mStateStatck == null)
        {
            mStateStatck = new Stack<IInputState>();
        }

        MeterManager.Ins.RegisterBaseMeterHandler(this);
    }

    public override void Dispose()
    {
        mInputControlMap.Clear();
        mStateStatck.Clear();
        MeterManager.Ins.UnregiseterBaseMeterHandler(this);
    }

    public InputManager()
    {
        //EmitterBus.AddListener(ModuleDef.InputModule, "DisableGameInput", (args) =>
        //{
        //    enableUpdate = false;
        //});

        //EmitterBus.AddListener(ModuleDef.InputModule, "EnableGameInput", (args) =>
        //{
        //    enableUpdate = true;
        //});
    }

    public IInputState GetCurrrentInputState()
    {
        if (mStateStatck.Count <= 0)
            return InputStateDefine.EMPTY_STATE;

        return mStateStatck.Peek();
    }

    public void AddState(IInputState state)
    {
        if (state == null)
            return;

        mStateStatck.Push(state);
    }

    public void RemoveState(IInputState state)
    {
        if (state == null)
            return;

        if (mStateStatck.Count == 0)
            return;

        IInputState toRemove = mStateStatck.Peek();
        if(toRemove != null && toRemove.GetStateName().Equals(state.GetStateName()))
        {
            mStateStatck.Pop();
        }
    }

    public bool HasInputControl(string inputName)
    {
        return mInputControlMap.ContainsKey(inputName);
    }

    public IInputControl GetInputControl(string inputName)
    {
        if(mInputControlMap.TryGetValue(inputName,out IInputControl ctl))
        {
            return ctl;
        }

        return null;
    }

    /// <summary>
    /// 注册InputControl
    /// </summary>
    /// <param name="inputName"></param>
    /// <param name="inputCtl"></param>
    public void RegisterInputControl(IInputControl inputCtl)
    {
        if(inputCtl == null)
        {
            Log.Error(LogLevel.Critical, "RegisterInputControl failed,inputCtl is null!");
            return;
        }

        string ctlName = inputCtl.GetInputControlName();
        if(string.IsNullOrEmpty(ctlName))
        {
            Log.Error(LogLevel.Critical, "RegisterInputControl failed,ctl name must not be null or empty!");
            return;
        }

        if (HasInputControl(ctlName))
        {
            Log.Error(LogLevel.Normal, "RegisterInputControl failed,Re-register input \'{0}\'", ctlName);
            return;
        }

        mInputControlMap.Add(ctlName, inputCtl);
        inputCtl.Initialize();
        Log.Logic(LogLevel.Info, "Register Input Control {0} Ok", ctlName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputCtl"></param>
    public void UnregisterInputControl(string ctlName)
    {
        if (string.IsNullOrEmpty(ctlName))
        {
            Log.Error(LogLevel.Critical, "UnregisterInputControl failed,ctl name must not be null or empty!");
            return;
        }

        if(mInputControlMap.TryGetValue(ctlName, out IInputControl inputCtl))
        {
            inputCtl.Dispose();
            mInputControlMap.Remove(ctlName);
        }
        Log.Logic(LogLevel.Info, "Unregister Input Control {0} Ok", ctlName);
    }

    public override void OnUpdate(float deltaTime)
    {
        IInputState state = GetCurrrentInputState();
        if (state == null)
            return;

        foreach (KeyValuePair<string, IInputControl> kv in mInputControlMap)
        {
            string ctlName = kv.Key;

            if (!state.CheckInputControlEnable(ctlName))
                continue;

            IInputControl inputCtl = kv.Value;
            if (inputCtl != null)
            {
                inputCtl.OnUpdate(deltaTime);
            }
        }
        
    }

    public override void OnMeter(int curMeterIndex)
    {
        IInputState state = GetCurrrentInputState();
        if (state == null)
            return;

        foreach (KeyValuePair<string, IInputControl> kv in mInputControlMap)
        {
            string ctlName = kv.Key;

            if (!state.CheckInputControlEnable(ctlName))
                continue;

            IInputControl inputCtl = kv.Value;
            if (inputCtl != null)
            {
                inputCtl.OnMeter(curMeterIndex);
            }
        }
    }
}