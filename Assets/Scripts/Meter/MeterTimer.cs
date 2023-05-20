
using System;

public class MeterTimer : IMeterHandler
{
    private int loopMeter;
    private int curLoopRecord;

    /// <summary>
    /// 目标循环次数
    /// </summary>
    private int targetLoopTime;

    /// <summary>
    /// 当前循环次数
    /// </summary>
    private int curLoopTime;


    /// <summary>
    /// 定时器回调
    /// </summary>
    private Action cb;

    public void SetTimer(int meterOffset, int loopTime)
    {
        loopMeter = meterOffset;
        curLoopRecord = 0;
        targetLoopTime = loopTime;
        curLoopTime = 0;
    }

    public void SetTimer(int meterOffset, int loopTime, Action cb)
    {
        SetTimer(meterOffset, loopTime);
        this.cb = cb;
    }

    public bool CheckEnd()
    {
        return targetLoopTime > 0 && curLoopTime >= targetLoopTime;
    }

    public void OnMeterEnter(int meterIndex)
    {
        curLoopRecord++;
        // 超过单次循环节拍数
        if (curLoopRecord >= loopMeter)
        {
            if (targetLoopTime > 0)
            {
                curLoopTime++;
            }

            // 重置当前循环的时间记录
            curLoopRecord -= loopMeter;

            // 执行回调
            if (cb != null)
            {
                cb();
            }
        }
    }

    public void OnMeterEnd(int meterIndex)
    {
        
    }
}
