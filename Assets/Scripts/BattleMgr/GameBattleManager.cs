using GameEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBattleManager : ModuleManager<GameBattleManager>
{
    public bool OnExcuteCombo(Agent user, ComboData rlsCombo)
    {

        return true;
    }

    public bool OnExcuteComboStep(Agent user, ComboStep rlsComboStep)
    {

        return true;
    }


    public void OnExcuteComboEffect(Agent user, SkillEffectData effectData)
    {
        Log.Logic(LogLevel.Info, "{0} excute effect {1}", user.GetAgentId(), effectData.effectType);

        var rlsSkEft = SkEftDefine.GetSkEftBy(user, effectData);
        if (rlsSkEft == null) return;
        rlsSkEft.TriggerSkEft();
    }


    /// <summary>
    /// 当释放伤害给到对象
    /// </summary>
    /// <param name="user"></param>
    /// <param name="tgt"></param>
    /// <param name="dmg"></param>
    public void OnApplyDmgToTgt(Agent user,Agent tgt, DmgEft dmg)
    {

    }





    public override void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public override void Initialize()
    {
        throw new System.NotImplementedException();
    }


}
