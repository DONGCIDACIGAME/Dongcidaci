using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameVisionView : MonoBehaviour
{
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


    #region Editor
#if UNITY_EDITOR
    public bool DrawGizmos;

    private Color lineColor = Color.blue;
    private float lineThickness = 2f;

    private void OnDrawGizmos()
    {
        if (DrawGizmos == false) return;
        Vector2[] shapeVertexs = null;
        var shape = GameColliderHelper.GetRegularShapeWith(this.shapeType,offset,size);
        shape.AnchorPos = transform.position;
        shape.AnchorAngle = transform.eulerAngles.y;
        shapeVertexs = shape.GetVertexs();

        if (shapeVertexs == null || shapeVertexs.Length == 0) return;
        // 画线
        Handles.color = lineColor;
        for (int i = 0; i < shapeVertexs.Length; i++)
        {
            int nextIndex = i + 1;
            if (nextIndex >= shapeVertexs.Length)
            {
                //last point
                nextIndex = 0;
            }

            Handles.DrawLine(new Vector3(shapeVertexs[i].x, 0, shapeVertexs[i].y), new Vector3(shapeVertexs[nextIndex].x, 0, shapeVertexs[nextIndex].y), lineThickness);
        }
    }


#endif
    #endregion



}
