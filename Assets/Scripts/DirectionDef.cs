using UnityEngine;

public static class DirectionDef
{
    public static Vector3 none = Vector3.zero;
    public static Vector3 up = new Vector3(0, 0, 1);
    public static Vector3 down = new Vector3(0, 0, -1);
    public static Vector3 left = new Vector3(-1, 0, 0);
    public static Vector3 right = new Vector3(1, 0, 0);


    /// <summary>
    /// 实时朝向
    /// </summary>
    public static int RealTowards = 0;
    /// <summary>
    /// 固定朝向
    /// </summary>
    public static int FixedTowards = 1;
}
