using UnityEngine;

public interface IInputControl
{
    string GetInputControlName();
    void InputControlUpdate(float deltaTime);
    void Initialize();
    //void Dispose();
}
