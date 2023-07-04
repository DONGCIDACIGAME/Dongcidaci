
namespace GameSkillEffect
{
    public static class SkEftDefine
    {
        #region Skill Effect Type Define

        public const int Damage = 0;




        #endregion


        public static SkillEffect GetSkEftBy(Agent user, SkillEffectData skEftData)
        {
            if (skEftData.effectType == Damage)
            {
                GameSkEftPool.Ins.Pop<DmgEft>(out DmgEft newDmg);
                newDmg.InitSkEft(user,skEftData);
                return newDmg;
            }



            return null;
        }

    }
}

