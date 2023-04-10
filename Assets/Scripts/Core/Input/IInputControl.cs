﻿public interface IInputControl
{
    void Initialize();
    void Dispose();
    string GetInputControlName();
    void RegisterInputHandle(string handleName, IInputHandle handle);
    void UnregisterInputHandle(string handleName);
    void SetInputHandleEnable(string hanelName, bool enable);
    void OnUpdate(float deltaTime);
    void OnMeter(int meterIndex);

}
