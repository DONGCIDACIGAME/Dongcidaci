using UnityEngine;

public class PlayerBaseMeterHandler : IMeterHandler
{
    private Hero mHero;
    private uint meterOffset;
    private MeterTrigger mMeterTrigger;
    public PlayerBaseMeterHandler(Hero hero)
    {
        mHero = hero;

        if(mHero != null)
        {
            DongciDaci.HeroBaseCfg cfg = mHero.GetAgentCfg();
            mMeterTrigger = new MeterTrigger(cfg.MeterOffset);
            meterOffset = cfg.MeterOffset;
        }
    }


    public void OnMeter(int meterIndex)
    {
        if (mMeterTrigger == null)
            return;

        mMeterTrigger.NextMeter();

        if (mHero == null)
            return;

        if (!mMeterTrigger.IsTriggered())
            return;

        mHero.OnMeter();
    }

    public uint GetMeterOffset()
    {
        return meterOffset;
    }
}
