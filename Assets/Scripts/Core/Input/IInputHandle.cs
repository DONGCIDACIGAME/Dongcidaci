public interface IInputHandle : IMeterHandler
{
    void OnUpdate(float deltaTime);
    string GetHandleName();
    void SetEnable(bool enable);
    bool CheckEnable();
}
