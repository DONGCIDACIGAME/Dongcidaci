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


    public void OnMeter()
    {
        if (mMeterTrigger == null)
            return;

        mMeterTrigger.NextMeter();

        if (mHero == null)
            return;

        if (!mMeterTrigger.IsTriggered())
            return;


        //mHero.AnimPlayer.UpdateAnimSpeed()
    }

    public uint GetMeterOffset()
    {
        return meterOffset;
    }
}
