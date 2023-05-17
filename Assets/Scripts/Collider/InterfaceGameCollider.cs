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






