public interface IInputControl
{
    string GetInputControlName();
    void InputControlUpdate(float deltaTime);
    void InputControlOnMeter(int meterIndex);
    //void Initialize();
    //void Dispose();
}
