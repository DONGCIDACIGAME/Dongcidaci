using UnityEngine;

public class GameColliderView : MonoBehaviour
{
    // 碰撞盒大小
    public Vector2 size;

    // 碰撞盒偏移
    public Vector2 offset;

    /// <summary>
    /// 编辑器下的辅助功能
    /// </summary>
    #region Editor
#if UNITY_EDITOR
    public bool DrawDizmos;

    public Color color = Color.red;

    private void OnDrawGizmos()
    {
        if (!MapDef.GlobalDrawMapDizmos || !DrawDizmos)
            return;

        Gizmos.color = color;

        Vector3 pos = this.transform.position;
        Vector3 rotation = this.transform.rotation.eulerAngles;

        float halfWidth = size.x / 2f;
        float halfHeight = size.y / 2f;

        // 自身有旋转，计算四个顶点的本地坐标
        Vector3 lb_offset = MapHelper.RotateByYAxis(new Vector3((-halfWidth + offset.x) * this.transform.lossyScale.x, 0, (-halfHeight + offset.y) * this.transform.lossyScale.z), -rotation.y);
        Vector3 rb_offset = MapHelper.RotateByYAxis(new Vector3((halfWidth + offset.x) * this.transform.lossyScale.x, 0, (-halfHeight + offset.y) * this.transform.lossyScale.z), -rotation.y);
        Vector3 rt_offset = MapHelper.RotateByYAxis(new Vector3((halfWidth + offset.x) * this.transform.lossyScale.x, 0, (halfHeight + offset.y) * this.transform.lossyScale.z), -rotation.y);
        Vector3 lt_offset = MapHelper.RotateByYAxis(new Vector3((-halfWidth + offset.x) * this.transform.lossyScale.x, 0, (halfHeight + offset.y) * this.transform.lossyScale.z), -rotation.y);


        // 计算四个顶点在Scene节点下的本地坐标
        Vector3 lb = pos + lb_offset;
        Vector3 rb = pos + rb_offset;
        Vector3 rt = pos + rt_offset;
        Vector3 lt = pos + lt_offset;

        Gizmos.DrawLine(lb, rb);
        Gizmos.DrawLine(rb, rt);
        Gizmos.DrawLine(rt, lt);
        Gizmos.DrawLine(lt, lb);
    }
#endif
    #endregion
}
