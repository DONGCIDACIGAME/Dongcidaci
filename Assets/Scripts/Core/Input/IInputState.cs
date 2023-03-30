using System.Collections.Generic;

public interface IInputState : IState
{
    string GetStateName();
    bool CheckInputControlEnable(string ctlName);
    HashSet<string> GetValidInputCtlNames();
    /// <summary>
    /// ״̬�����ȼ�
    /// </summary>
    /// <returns></returns>
    int GetStatePriority();
}
