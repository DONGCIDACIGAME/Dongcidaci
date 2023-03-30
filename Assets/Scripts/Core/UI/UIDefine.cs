using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIDefine 
{
    public const int Panel_Load_Per_Frame = 1; // 每帧最多加载的面板数量
    public const int Control_Load_Per_Frame = 5; // 每帧最多加载的Control数量


    public static readonly bool UI_EnabelRecycle = false; // 是否允许复用
}
