public class MonsterMeterHandler : IMeterHandler
{
    private int meterOffset;
    private Monster mMonster;
    public MonsterMeterHandler(Monster monster)
    {
        mMonster = monster;
    }

    public int GetMeterOffset()
    {
        return meterOffset;
    }

    public void OnMeterEnter(int meterIndex)
    {
        if (mMonster == null)
            return;

        mMonster.OnMeterEnter(meterIndex);
    }

    public void OnMeterEnd(int meterIndex)
    {
        if (mMonster == null)
            return;

        mMonster.OnMeterEnd(meterIndex);
    }
}
