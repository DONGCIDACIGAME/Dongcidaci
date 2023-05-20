using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentGameCollider2D : GameCollider2D
{
    public AgentGameCollider2D(GameColliderData2D colliderData, Transform tgtTransform,IAgentCollideHandler collideHandler) :base(colliderData, tgtTransform, collideHandler)
    {

    }

    public override void OnColliderEnter(IGameCollider other)
    {
        var tgtHandler = other.GetColliderHandler();

    }

    public override void OnCollideUpdate(float deltaTime)
    {
        base.OnCollideUpdate(deltaTime);


    }
}
