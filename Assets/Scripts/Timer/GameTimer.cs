using System;

public class GameTimer
{
    /// <summary>
    /// �Ƿ��ڹ���״̬
    /// </summary>
    public bool working;

    /// <summary>
    /// Ŀ��ѭ������
    /// </summary>
    private int targetLoopTime;

    /// <summary>
    /// ��ǰѭ������
    /// </summary>
    private int curLoopTime;

    /// <summary>
    /// ��ʱ������ʱ��
    /// </summary>
    private float loopDuration;

    /// <summary>
    /// ��ǰѭ���еĶ�ʱ��ʱ��
    /// </summary>
    private float curLoopRecord;

    /// <summary>
    /// ��ʱ���ص�
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

        // ��������ѭ��ʱ��
        if(curLoopRecord > loopDuration)
        {
            if(targetLoopTime > 0)
            {
                curLoopTime++;
            }

            // ���õ�ǰѭ����ʱ���¼
            curLoopRecord -= loopDuration;

            // ִ�лص�
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
