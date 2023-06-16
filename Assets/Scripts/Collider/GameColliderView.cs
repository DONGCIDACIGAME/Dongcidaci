using UnityEditor;
using UnityEngine;

public class GameColliderView : MonoBehaviour
{
    // 暂时不考虑运行时通过该组件更新碰撞大小

    /// <summary>
    /// 碰撞盒大小
    /// </summary>
    public Vector2 size;

    /// <summary>
    /// 碰撞盒偏移
    /// </summary>
    public Vector2 offset;


    private void Start()
    {
        
    }



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
        

        Vector3 groundPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 rotation = this.transform.rotation.eulerAngles;

        float halfWidth = size.x / 2f;
        float halfHeight = size.y / 2f;

        // 自身有旋转，计算四个顶点的本地坐标
        Vector3 lb_offset = MapHelper.RotateByYAxis(new Vector3((-halfWidth + offset.x) * this.transform.lossyScale.x, 0, (-halfHeight + offset.y) * this.transform.lossyScale.z), -rotation.y);
        Vector3 rb_offset = MapHelper.RotateByYAxis(new Vector3((halfWidth + offset.x) * this.transform.lossyScale.x, 0, (-halfHeight + offset.y) * this.transform.lossyScale.z), -rotation.y);
        Vector3 rt_offset = MapHelper.RotateByYAxis(new Vector3((halfWidth + offset.x) * this.transform.lossyScale.x, 0, (halfHeight + offset.y) * this.transform.lossyScale.z), -rotation.y);
        Vector3 lt_offset = MapHelper.RotateByYAxis(new Vector3((-halfWidth + offset.x) * this.transform.lossyScale.x, 0, (halfHeight + offset.y) * this.transform.lossyScale.z), -rotation.y);

        // 计算四个定点的世界坐标
        Vector3 lb = groundPos + lb_offset;
        Vector3 rb = groundPos + rb_offset;
        Vector3 rt = groundPos + rt_offset;
        Vector3 lt = groundPos + lt_offset;

        float lineThickness = 2f;
        //Gizmos.DrawLine(lb, rb);
        Handles.DrawLine(lb, rb, lineThickness);
        //Gizmos.DrawLine(rb, rt);
        Handles.DrawLine(rb, rt, lineThickness);
        //Gizmos.DrawLine(rt, lt);
        Handles.DrawLine(rt, lt, lineThickness);
        //Gizmos.DrawLine(lt, lb);
        Handles.DrawLine(lt, lb, lineThickness);
    }
#endif
    #endregion
}
