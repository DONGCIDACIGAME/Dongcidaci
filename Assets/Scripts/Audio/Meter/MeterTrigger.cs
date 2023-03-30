public class MeterTrigger
{
    /// <summary>
    /// 每几拍触发一次
    /// </summary>
    private uint mMeterOffset;

    /// <summary>
    /// 记拍器
    /// </summary>
    private int mMeterRecord;

    /// <summary>
    /// 使用时，应先调用IsTriggered判断是否触发，再做NextMeter记拍
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

        // 超过触发间隔自动归零
        if (mMeterRecord >= mMeterOffset)
        {
            mMeterRecord = 0;
        }
    }
}
