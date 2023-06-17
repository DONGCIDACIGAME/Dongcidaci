using System.Collections.Generic;

namespace GameEngine
{
    public class InputManager : MeterModuleManager<InputManager>
    {
        /// <summary>
        /// 存储了所有的inputcontrol，每个InputControl代表一类输入
        /// 1. 键盘
        /// 2. 鼠标
        /// 3. 手柄
        /// ...
        /// </summary>
        private Dictionary<string, IInputControl> mInputControlMap;

        /// <summary>
        /// 是否允许输入
        /// </summary>
        private bool mInputEnable;


        protected override void BindEvents()
        {
            mEventListener.Listen("DisableGameInput", () =>
            {
                SetInputUpdateEnable(false);
            });


            mEventListener.Listen("EnableGameInput", () =>
            {
                SetInputUpdateEnable(true);
            });
        }

        public override void Initialize()
        {
            if (mInputControlMap == null)
            {
                mInputControlMap = new Dictionary<string, IInputControl>();
            }

            MeterManager.Ins.RegisterMeterHandler(this);
            mInputEnable = true;
        }

        public override void Dispose()
        {
            mInputControlMap.Clear();
            MeterManager.Ins.UnregiseterMeterHandler(this);
        }

        public InputManager()
        {

        }

        private void SetInputUpdateEnable(bool enable)
        {
            mInputEnable = enable;
        }

        public bool HasInputControl(string inputName)
        {
            return mInputControlMap.ContainsKey(inputName);
        }

        public IInputControl GetInputControl(string inputName)
        {
            if (mInputControlMap.TryGetValue(inputName, out IInputControl ctl))
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
            if (inputCtl == null)
            {
                Log.Error(LogLevel.Critical, "RegisterInputControl failed,inputCtl is null!");
                return;
            }

            string ctlName = inputCtl.GetInputControlName();
            if (string.IsNullOrEmpty(ctlName))
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

            if (mInputControlMap.TryGetValue(ctlName, out IInputControl inputCtl))
            {
                inputCtl.Dispose();
                mInputControlMap.Remove(ctlName);
            }
            Log.Logic(LogLevel.Info, "Unregister Input Control {0} Ok", ctlName);
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!mInputEnable)
                return;

            foreach (KeyValuePair<string, IInputControl> kv in mInputControlMap)
            {
                IInputControl inputCtl = kv.Value;
                if (inputCtl != null)
                {
                    inputCtl.OnUpdate(deltaTime);
                }
            }

        }

        public override void OnMeterEnter(int meterIndex)
        {
            foreach (KeyValuePair<string, IInputControl> kv in mInputControlMap)
            {
                IInputControl inputCtl = kv.Value;
                if (inputCtl != null)
                {
                    inputCtl.OnMeterEnter(meterIndex);
                }
            }
        }

        public override void OnMeterEnd(int meterIndex)
        {
            foreach (KeyValuePair<string, IInputControl> kv in mInputControlMap)
            {
                IInputControl inputCtl = kv.Value;
                if (inputCtl != null)
                {
                    inputCtl.OnMeterEnd(meterIndex);
                }
            }
        }
    }
}

