
namespace GameSkillEffect
{
    public class EffectDamage : Damage
    {
        public override void Dispose()
        {
            this._dmgValue = 0;
            this._eftCollideShape = null;
            this._eftUser = null;
            this._initSkEftData = null;
        }

        public override void Recycle()
        {
            Dispose();
            GameSkEftPool.Ins.Push<EffectDamage>(this);
        }

        public override void RecycleReset()
        {
            Dispose();
        }


    }
}


