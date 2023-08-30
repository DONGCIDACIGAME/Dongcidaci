using System.Collections.Generic;

namespace GameSkillEffect
{
    public class GameEffectsData
    {
        public List<SkillEffect> effectsForRls;
        public FXConfigData rlsFxCfg;
        public FXConfigData hitFxCfg;


        public GameEffectsData(List<SkillEffect> effects,FXConfigData rls,FXConfigData hit)
        {
            this.effectsForRls = effects;
            this.rlsFxCfg = rls;
            this.hitFxCfg = hit;
        }


    }


}


