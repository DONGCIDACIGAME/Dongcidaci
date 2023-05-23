using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 需要实时更新的地图元素
/// </summary>
public interface IUpdateMapElements
{
    public void OnUpdate(float deltaTime);
}

/// <summary>
/// 会产生移动的地图元素
/// </summary>
public interface IMoveMapElements
{

}