
namespace GameSkillEffect
{
    [System.Serializable]
    public class SkEftDataCollection
    {
        /// <summary>
        /// 这一击释放的特效配置
        /// </summary>
        public FXConfigData rlsFxCfg;

        /// <summary>
        /// 这一击击中敌人时，敌人身上的特效
        /// </summary>
        public FXConfigData hitFxCfg;

        /// <summary>
        /// 这一击所携带的所有技能效果
        /// </summary>
        public SkillEffectData[] effects;
    }



}


