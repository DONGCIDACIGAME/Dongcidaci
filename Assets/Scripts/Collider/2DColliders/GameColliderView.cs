using GameEngine;
using UnityEditor;
using UnityEngine;

public class GameColliderView : MonoBehaviour
{
    // 暂时不考虑运行时通过该组件更新碰撞大小

    /// <summary>
    /// 这个碰撞体view的形状
    /// </summary>
    public Convex2DShapeType shapeType = Convex2DShapeType.Rect;

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

    public int polyCountForShow = 8;

    private void OnDrawGizmos()
    {
        if (!MapDef.GlobalDrawMapDizmos || !DrawDizmos)
            return;

        Gizmos.color = color;
        float lineThickness = 2f;
        Handles.color = color;

        Vector2[] colliderVertexs = null;

        if (this.shapeType == Convex2DShapeType.Rect)
        {
            colliderVertexs = GameColliderHelper.GetRectVertexs(this.transform.position,transform.eulerAngles.y,offset,size);
        }

        // 圆的计算测试
        if (shapeType == Convex2DShapeType.Circle ||(shapeType == Convex2DShapeType.Ellipse && size.x == size.y))
        {
            // circle
            colliderVertexs = GameColliderHelper.GetCircleVertexs(this.transform.position, transform.eulerAngles.y, offset, size.x/2f, polyCountForShow);
        }

        if (shapeType == Convex2DShapeType.Ellipse && size.x != size.y)
        {
            colliderVertexs = GameColliderHelper.GetEllipseVertexs(this.transform.position, transform.eulerAngles.y, offset, size, polyCountForShow);
        }


        if (colliderVertexs == null || colliderVertexs.Length == 0) return;
        // 画线
        for (int i = 0; i < colliderVertexs.Length; i++)
        {
            int nextIndex = i + 1;
            if (nextIndex >= colliderVertexs.Length)
            {
                //last point
                nextIndex = 0;
            }

            Handles.DrawLine(new Vector3(colliderVertexs[i].x, 0, colliderVertexs[i].y), new Vector3(colliderVertexs[nextIndex].x, 0, colliderVertexs[nextIndex].y), lineThickness);
        }


    }

    
#endif
    #endregion
}
