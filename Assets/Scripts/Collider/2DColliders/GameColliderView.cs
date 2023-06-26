using GameEngine;
using UnityEditor;
using UnityEngine;

public class GameColliderView : MonoBehaviour
{
    // 暂时不考虑运行时通过该组件更新碰撞大小


    public ConvexCollider2DType colliderType = ConvexCollider2DType.Rect;

    /// <summary>
    /// 碰撞盒大小
    /// </summary>
    public Vector2 size;

    /// <summary>
    /// 碰撞盒偏移
    /// </summary>
    public Vector2 offset;


    





    /// <summary>
    /// 编辑器下的辅助功能
    /// </summary>
    #region Editor
#if UNITY_EDITOR
    public bool DrawDizmos;

    public Color color = Color.red;

    public int polyVertexCount = 8;

    private void OnDrawGizmos()
    {
        if (!MapDef.GlobalDrawMapDizmos || !DrawDizmos)
            return;

        Gizmos.color = color;
        float lineThickness = 2f;
        Handles.color = color;

        if (this.colliderType == ConvexCollider2DType.Rect)
        {
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
            //Gizmos.DrawLine(lb, rb);
            Handles.DrawLine(lb, rb, lineThickness);
            //Gizmos.DrawLine(rb, rt);
            Handles.DrawLine(rb, rt, lineThickness);
            //Gizmos.DrawLine(rt, lt);
            Handles.DrawLine(rt, lt, lineThickness);
            //Gizmos.DrawLine(lt, lb);
            Handles.DrawLine(lt, lb, lineThickness);
        }

        // 圆的计算测试
        if (colliderType == ConvexCollider2DType.Ellipse && size.x == size.y)
        {
            // circle
            // 1 get vertexs
            var originVetexs = new Vector2[polyVertexCount];
            float stepAngle = 360f / (float)polyVertexCount;
            var baseV3 = new Vector3(-size.x / 2f, 0, 0);
            for (int i = 0; i < originVetexs.Length; i++)
            {
                if (i == 0)
                {
                    originVetexs[i] = new Vector2(baseV3.x + offset.x, baseV3.z + offset.y);
                    continue;
                }

                var newV3 = Quaternion.AngleAxis(stepAngle * i, Vector3.up) * baseV3;
                originVetexs[i] = new Vector2(newV3.x + offset.x, newV3.z + offset.y);
            }

            // 根据 anchor angle 旋转
            float arc = -this.transform.rotation.eulerAngles.y / 180 * Mathf.PI;
            for (int i = 0; i < originVetexs.Length; i++)
            {
                originVetexs[i] = new Vector2(
                    originVetexs[i].x * Mathf.Cos(arc) - originVetexs[i].y * Mathf.Sin(arc),
                    originVetexs[i].x * Mathf.Sin(arc) + originVetexs[i].y * Mathf.Cos(arc));
            }

            // 加上锚点的位置
            var anchorPos = new Vector2(transform.position.x, transform.position.z);
            for (int i = 0; i < originVetexs.Length; i++)
            {
                originVetexs[i] += anchorPos;
            }

            // 画线
            for (int i = 0; i < originVetexs.Length; i++)
            {
                int nextIndex = i + 1;
                if (nextIndex >= originVetexs.Length)
                {
                    //last point
                    nextIndex = 0;
                }
                //retEdges[i, 0] = originVetexs[i];
                //retEdges[i, 1] = originVetexs[nextIndex];
                Handles.DrawLine(new Vector3(originVetexs[i].x,0, originVetexs[i].y), new Vector3(originVetexs[nextIndex].x, 0, originVetexs[nextIndex].y), lineThickness);
            }

        }





    }

    
#endif
    #endregion
}
