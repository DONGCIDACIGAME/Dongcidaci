using UnityEngine;

public interface IGameCollider
{
    /// <summary>
    /// 获取当前碰撞体的类别
    /// </summary>
    /// <returns></returns>
    public GameColliderType GetColliderType();

    /// <summary>
    /// 当碰撞体触发时
    /// </summary>
    /// <param name="other"></param>
    public void OnColliderEnter(IGameCollider other);

    /// <summary>
    /// 获取碰撞的处理对象
    /// </summary>
    /// <returns></returns>
    public ICollideHandler GetColliderHandler();

    /// <summary>
    /// 碰撞体的销毁
    /// </summary>
    public void Dispose();

}


public interface IGameCollider2D : IGameCollider
{
    public bool CheckPosInCollider(Vector2 pos);
    public bool CheckCollapse(Rect rect);
}


/// <summary>
/// 需要处理碰撞事件的对象继承的接口
/// </summary>
public interface ICollideHandler
{
    //public void HandleColliderOccur(GameCollider2D otherCollider);

}

/// <summary>
/// 角色类碰撞对象
/// </summary>
public interface IAgentCollideHandler: ICollideHandler
{

}

/// <summary>
/// 地图障碍类碰撞对象
/// </summary>
public interface IMapBlockCollideHandler : ICollideHandler
{

}

/// <summary>
/// 地图事件碰撞对象
/// </summary>
public interface IMapEventCollideHandler : ICollideHandler
{

}



