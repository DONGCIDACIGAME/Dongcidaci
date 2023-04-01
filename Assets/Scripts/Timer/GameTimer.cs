using System;

public class GameTimer
{
    /// <summary>
    /// 是否处于工作状态
    /// </summary>
    public bool working;

    /// <summary>
    /// 目标循环次数
    /// </summary>
    private int targetLoopTime;

    /// <summary>
    /// 当前循环次数
    /// </summary>
    private int curLoopTime;

    /// <summary>
    /// 定时器单次时长
    /// </summary>
    private float loopDuration;

    /// <summary>
    /// 当前循环中的定时器时间
    /// </summary>
    private float curLoopRecord;

    /// <summary>
    /// 定时器回调
    /// </summary>
    private Action cb;

    public void SetTimer(float time, int loopTime)
    {
        loopDuration = time;
        curLoopRecord = 0;
        targetLoopTime = loopTime;
        curLoopTime = 0;
        working = true;
    }


    public void BindAction(Action cb)
    {
        this.cb = cb;
    }

    public bool CheckEnd()
    {
        return targetLoopTime > 0 && curLoopTime >= targetLoopTime;
    }

    public void OnUpdate(float deltaTime)
    {
        if (!working)
            return;

        curLoopRecord += deltaTime;

        // 超过单次循环时间
        if(curLoopRecord > loopDuration)
        {
            if(targetLoopTime > 0)
            {
                curLoopTime++;
            }

            // 重置当前循环的时间记录
            curLoopRecord -= loopDuration;

            // 执行回调
            if(cb != null)
            {
                cb();
            }
        }
    }

    public void OnLateUpdate(float deltaTime)
    {
        if (CheckEnd())
        {
            working = false;
        }
    }
}
