using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 具有碰撞体的地图实体
/// </summary>
public interface IColliderMapEntity
{

}

/// <summary>
/// 需要实时更新的地图元素
/// </summary>
public interface IUpdateMapEntity
{
    public void OnUpdate(float deltaTime);
}

/// <summary>
/// 会产生移动的地图元素
/// </summary>
public interface IMoveMapEntity
{

}



public interface IMapEvents
{
    public void TriggerEvents();

}