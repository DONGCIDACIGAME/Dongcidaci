/// <summary>
/// 行为作用在动画状态上的结果
/// </summary>
public struct ActionOnAnimStateResult
{
    public AnimStateStrategy strategy;
    
}

public enum AnimStateStrategy
{
    Keep = 0,
    Change = 1,
}

