using GameEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBattleManager : ModuleManager<GameBattleManager>
{
    public override void Initialize()
    {
        return;
    }

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


    public void OnApplyDmgToTgt(Agent dmgUser,Agent dmgTgt, DmgEft dmg)
    {

    }

    public void OnGetDmgFromUser(Agent dmgReceiver, Agent dmgUser, DmgEft dmg)
    {

    }






    public override void Dispose()
    {
        throw new System.NotImplementedException();
    }

    


}
