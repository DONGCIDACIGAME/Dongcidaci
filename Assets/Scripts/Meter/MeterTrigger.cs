public class MeterTrigger
{
    /// <summary>
    /// ÿ���Ĵ���һ��
    /// </summary>
    private uint mMeterOffset;

    /// <summary>
    /// ������
    /// </summary>
    private int mMeterRecord;

    /// <summary>
    /// ʹ��ʱ��Ӧ�ȵ���IsTriggered�ж��Ƿ񴥷�������NextMeter����
    /// </summary>
    /// <param name="offset"></param>
    public MeterTrigger(uint offset)
    {
        mMeterOffset = offset;
    }

    public uint GetMeterOffset()
    {
        return mMeterOffset;
    }

    public bool IsTriggered()
    {
        return mMeterRecord % mMeterOffset == 0;
    }

    public void NextMeter()
    {
        mMeterRecord++;

        // ������������Զ�����
        if (mMeterRecord >= mMeterOffset)
        {
            mMeterRecord = 0;
        }
    }
}
