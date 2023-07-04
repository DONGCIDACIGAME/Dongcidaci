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
