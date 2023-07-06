
namespace GameSkillEffect
{
    public class AttackDamage : Damage
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
            GameSkEftPool.Ins.Push<AttackDamage>(this);
        }

        public override void RecycleReset()
        {
            Dispose();
        }


    }
}


