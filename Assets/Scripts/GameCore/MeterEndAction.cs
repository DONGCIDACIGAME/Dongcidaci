public class MeterEndAction
{
    private int mMeterIndex;
    private System.Action mAction;

    public MeterEndAction(int meterIndex, System.Action action)
    {
        mMeterIndex = meterIndex;
        mAction = action;
    }

    public bool CheckMeter(int meterIndex)
    {
        return meterIndex == mMeterIndex;
    }

    public void Excute()
    {
        if (mAction != null)
            mAction();
    }

    public void CheckAndExcute(int meterIndex)
    {
        if (CheckMeter(meterIndex))
        {
            Excute();
        }
    }
}

