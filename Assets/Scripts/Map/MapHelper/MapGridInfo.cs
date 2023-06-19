using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapGridInfo
{
    public int colNum;
    public int rowNum;
    public float cellWidth;
    public float cellHeight;

    public float rotateAngle;

    public MapGridInfo(int colNumber, int rowNumber, float cellWidth, float cellHeight,float rotateAngle)
    {
        this.colNum = colNumber;
        this.rowNum = rowNumber;
        this.cellWidth = cellWidth;
        this.cellHeight = cellHeight;
        this.rotateAngle = rotateAngle;
    }

    /// <summary>
    /// 根据行数和列数获取当前的地图块索引
    /// </summary>
    /// <param name="colIndex"></param>
    /// <param name="rowIndex"></param>
    /// <returns>-1 数组越界</returns>
    public int GetIndexByColAndRow(int colIndex, int rowIndex)
    {
        if (colIndex > colNum - 1 || rowIndex > rowNum - 1) return -1;
        // 横向从左到右排序
        //return rowIndex * colNum + colIndex;
        // 纵向从下到上
        return colIndex * rowNum + rowIndex;
    }

    /**
    /// <summary>
    /// 根据行与列范围，转换成所有的索引
    /// </summary>
    /// <param name="colsRange"></param>
    /// <param name="rowsRange"></param>
    /// <returns></returns>
    public int[] GetIndexWithColsAndRows(ValueTuple<int, int> colsRange, ValueTuple<int, int> rowsRange)
    {
        List<int> ret = new List<int>();
        if (colsRange.Item1 > colsRange.Item2 || colsRange.Item2 > this.colNum - 1) return null;
        if (rowsRange.Item1 > rowsRange.Item2 || rowsRange.Item2 > this.rowNum - 1) return null;
        for (int i = colsRange.Item1; i < colsRange.Item2; i++)
        {
            for (int k = rowsRange.Item1; k < rowsRange.Item2; k++)
            {
                ret.Add(k * colNum + i);
            }
        }

        return ret.ToArray();
    }

    /// <summary>
    /// 根据地图块索引获取当前的区域信息
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Rect GetRectByMapIndex(int mapIndex)
    {
        var crtRowIndex = mapIndex / this.colNum;
        var crtColIndex = mapIndex % this.colNum;

        var tgtRect = new Rect(new Vector2(crtColIndex * cellWidth, crtRowIndex * cellHeight), new Vector2(this.cellWidth, this.cellHeight));
        return tgtRect;
    }

    */


    /// <summary>
    /// 获取某个位置的地图网格索引
    /// 已经考虑网格的旋转
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int GetMapIndex(Vector3 worldPos)
    {
        //计算绕轴旋转的真实位置
        var realPos = MapHelper.RotateByYAxis(worldPos,-rotateAngle);

        if (realPos.x < 0 || realPos.z < 0) return -1;
        if (this.cellWidth == 0 || this.cellHeight == 0) return -1;
        var colIndex = Mathf.FloorToInt(realPos.x / this.cellWidth);
        var rowIndex = Mathf.FloorToInt(realPos.z / this.cellHeight);
        return GetIndexByColAndRow(colIndex,rowIndex);
    }



}


