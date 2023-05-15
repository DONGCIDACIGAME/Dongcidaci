public class ComboStepExcutor
{
    private Agent mAgt;

    public ComboStepExcutor(Agent agt)
    {
        mAgt = agt;
    }

    private HitPointInfo[] GetHitPoints()
    {
        return null;
    }


    public void Start(TriggerableCombo combo)
    {
        if(mAgt == null)
        {

            return;
        }

        if(combo == null)
        {

            return;
        }

        ComboStepData stepData = combo.GetCurrentComboStep();
        if(stepData == null)
        {

            return;
        }

        AgentAnimStateInfo stateInfo = AgentHelper.GetStateInfo(mAgt, stepData.stateName, stepData.stateName);
        if(stateInfo == null)
        {

            return;
        }

        // 总时长
        float totalTime = MeterManager.Ins.GetTimeToMeter(stateInfo.stateMeterLen);

        HitPointInfo[] hitPoints = stateInfo.hitPoints;
        if(hitPoints == null || hitPoints.Length == 0)
        {

            return;
        }

        if(stepData.effects == null || stepData.effects.Length == 0)
        {

            return;
        }

        if(hitPoints.Length != stepData.effects.Length)
        {

            return;
        }

        for(int i = 0; i < hitPoints.Length; i++)
        {
            // 第i个hit点的信息
            HitPointInfo hitpoint = hitPoints[i];
            // 第i个hit点的效果
            ComboHitEffect hitEffect = stepData.effects[i];

            for(int j = 0; j < hitEffect.hitEffects.Length; j++)
            {
                RectEffectExcutor excutor = RectEffectExcutorPool.Ins.PopExcutor();
                excutor.Initialize(mAgt, hitpoint.progress * totalTime, hitEffect.hitEffects[j]);
            }
        }

    }
}
