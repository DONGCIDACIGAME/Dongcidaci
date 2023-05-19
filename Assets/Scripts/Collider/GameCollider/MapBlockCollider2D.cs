using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlockCollider2D : GameCollider2D
{
    public MapBlockCollider2D(GameColliderData2D colliderData, Transform tgtTransform, IColliderHandler colliderHandler) : base(colliderData, tgtTransform, colliderHandler)
    {

    }
}
