using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

namespace GameSkillEffect {

    public abstract class SkEftEntity : MapEntityWithCollider
    {
        public abstract void InitSkEftEntity(string eftEntityPrefab);
        //protected abstract void BindSkEntityView();

    }


}


