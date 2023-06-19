using UnityEngine;

public static class MapHelper
{
    public static float ToArc(float angle)
    {
        return angle / 180 * Mathf.PI;
    }

    /// <summary>
    /// 一个点A绕着另一个点B旋转（绕Y轴）角度angle后在点B坐标系下的本地坐标
    /// </summary>
    /// <param name="localPos"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector3 RotateByYAxis(Vector3 localPos, float angle)
    {
        // 转为弧度
        float arc = ToArc(angle);
        return new Vector3(
            localPos.x * Mathf.Cos(arc) - localPos.z * Mathf.Sin(arc),
            localPos.y,
            localPos.x * Mathf.Sin(arc) + localPos.z * Mathf.Cos(arc)
        );
    }
}
