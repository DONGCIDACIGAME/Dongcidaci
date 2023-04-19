using GameEngine;
public interface IInputHandle
{
    void OnUpdate(float deltaTime);
    void OnMeter(int meterIndex);
    string GetHandleName();
    void SetEnable(bool enable);
    bool CheckEnable();
}
