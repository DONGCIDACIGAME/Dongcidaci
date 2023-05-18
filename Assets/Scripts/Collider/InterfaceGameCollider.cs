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



}


public interface IGameCollider2D : IGameCollider
{
    public bool CheckPosInCollider(Vector2 pos);
    public bool CheckCollapse(Rect rect);
}


/// <summary>
/// 需要处理碰撞事件的对象继承的接口
/// </summary>
public interface IHandleCollider
{
    public void HandleColliderOccur(GameCollider2D otherCollider);



}



