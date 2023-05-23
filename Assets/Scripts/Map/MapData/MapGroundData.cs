using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapGroundType
{
    // 固定的地板
    FixGround,
    // 可移动的地板
    MoveGround,
}


[System.Serializable]
public class MapGroundData
{
    /// <summary>
    /// 这个地板的类型
    /// </summary>
    public MapGroundType groundType;

    /// <summary>
    /// 地板的预制体名
    /// </summary>
    public string groundPrefabName;

    /// <summary>
    /// 初始的位置信息
    /// </summary>
    public Vector3 initPos;

    /// <summary>
    /// 地板初始位置占用的地图格子索引
    /// </summary>
    public int[] occupyMapIndex;

    /// <summary>
    /// 地板的移动路径数组
    /// </summary>
    public Vector3[] movePosArr;

    /// <summary>
    /// 地板每次移动间隔的拍子数
    /// </summary>
    public int meterIntervalPerMove;

}
