using UnityEngine;

public class PlayerBaseMeterHandler : IMeterHandler
{
    private Hero mHero;
    public PlayerBaseMeterHandler(Hero hero)
    {
        mHero = hero;
    }


    public void OnMeter(int meterIndex)
    {
        if (mHero == null)
            return;

        mHero.OnMeter(meterIndex);
    }
}
