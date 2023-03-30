using System.Collections.Generic;

public interface IInputState : IState
{
    string GetStateName();
    bool CheckInputControlEnable(string ctlName);
    HashSet<string> GetValidInputCtlNames();
    /// <summary>
    /// 状态的优先级
    /// </summary>
    /// <returns></returns>
    int GetStatePriority();
}
