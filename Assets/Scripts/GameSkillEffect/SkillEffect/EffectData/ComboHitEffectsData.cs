using System.Collections;
using System.Collections.Generic;

namespace GameSkillEffect
{
    public class ComboHitEffectsData
    {
        public int comboUID;
        public ComboType comboType;
        public bool isLastStep;
        public List<SkillEffect> effectsForRls;

        public ComboHitEffectsData(int uid, ComboType type, bool isLast, List<SkillEffect> effects)
        {
            this.comboUID = uid;
            this.comboType = type;
            this.isLastStep = isLast;
            this.effectsForRls = effects;
        }


    }


}


