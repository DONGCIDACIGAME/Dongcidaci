using UnityEngine;
using GameEngine;

public abstract class MyMonoBehaviour : MonoBehaviour, IGameUpdate
{
    public MyMonoBehaviour()
    {
        UpdateCenter.Ins.RegisterUpdater(this);
    }

    public virtual void OnGameUpdate(float deltaTime)
    {

    }
}
