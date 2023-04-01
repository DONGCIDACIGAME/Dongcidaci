
using System;

public class MeterTimer
{
    private int loopMeter;
    private int curLoopRecord;

    /// <summary>
    /// Ŀ��ѭ������
    /// </summary>
    private int targetLoopTime;

    /// <summary>
    /// ��ǰѭ������
    /// </summary>
    private int curLoopTime;


    /// <summary>
    /// ��ʱ���ص�
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

    public void OnMeter(int meterIndex)
    {
        curLoopRecord++;

        // ��������ѭ��������
        if (curLoopRecord > loopMeter)
        {
            if (targetLoopTime > 0)
            {
                curLoopTime++;
            }

            // ���õ�ǰѭ����ʱ���¼
            curLoopRecord -= loopMeter;

            // ִ�лص�
            if (cb != null)
            {
                cb();
            }
        }
    }
}
