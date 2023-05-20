using UnityEngine;

public class PlayerMeterHandler : IMeterHandler
{
    private Hero mHero;
    public PlayerMeterHandler(Hero hero)
    {
        mHero = hero;
    }

    public void OnMeterEnd(int meterIndex)
    {
        if (mHero == null)
            return;

        mHero.OnMeterEnd(meterIndex);
    }

    public void OnMeterEnter(int meterIndex)
    {
        if (mHero == null)
            return;

        mHero.OnMeterEnter(meterIndex);
    }
}
