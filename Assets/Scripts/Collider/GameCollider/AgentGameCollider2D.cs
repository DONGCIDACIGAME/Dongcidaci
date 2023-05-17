using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentGameCollider2D : GameCollider2D
{
    public AgentGameCollider2D(GameColliderData2D colliderData, Transform tgtTransform) :base(colliderData, tgtTransform)
    {

    }


    public override void OnColliderEnter(IGameCollider other)
    {
        throw new System.NotImplementedException();
    }
}
