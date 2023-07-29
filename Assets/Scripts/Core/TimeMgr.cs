using GameEngine;

public class TimeMgr : ModuleManager<TimeMgr>
{
    /// <summary>
    /// 当前所在帧
    /// </summary>
    public long FrameIndex { get; private set; }

    /// <summary>
    /// 当前时间
    /// </summary>
    public float Now { get; private set; }

    public override void Dispose()
    {
        FrameIndex = 0;
        Now = 0;
    }

    public override void Initialize()
    {
        FrameIndex = 0;
        Now = 0;
    }

    public override void OnGameUpdate(float deltaTime)
    {
        base.OnGameUpdate(deltaTime);
        FrameIndex++;
        Now += deltaTime;
    }
}
