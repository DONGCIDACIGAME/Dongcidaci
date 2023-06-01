using GameEngine;
using UnityEngine;

public interface IGameCollider
{
    /// <summary>
    /// 当碰撞体触发时
    /// </summary>
    /// <param name="other"></param>
    public void OnColliderEnter(IGameCollider other);

    /// <summary>
    /// 获取碰撞的处理对象
    /// </summary>
    /// <returns></returns>
    public ICollideProcessor GetCollideProcessor();

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
/// 碰撞的处理机
/// </summary>
public interface ICollideProcessor
{
    /// <summary>
    /// 处理机处理碰撞
    /// </summary>
    /// <param name="tgtColliderProcessor"></param>
    public void HandleCollideTo(ICollideProcessor tgtColliderProcessor);

    /// <summary>
    /// 获取处理机实体
    /// </summary>
    /// <returns></returns>
    public Entity GetProcessorEntity();
}


