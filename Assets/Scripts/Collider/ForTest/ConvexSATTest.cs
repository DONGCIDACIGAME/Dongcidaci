using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConvexSATTest : MonoBehaviour
{
    public Vector2 offset;
    public Vector2 size;
    public Convex2DShapeType shapeType = Convex2DShapeType.Rect;

    public IConvex2DShape convexShape = null;
    [SerializeField] private ConvexSATTest tgtConvexComp = null;

    private void Awake()
    {
        switch (shapeType)
        {
            case Convex2DShapeType.Rect:
                convexShape = new Rect2DShape(
                    transform.position,
                    transform.eulerAngles.y,
                    offset,
                    size
                    );
                break;

            case Convex2DShapeType.Circle:
                convexShape = new Circle2DShape(
                    transform.position,
                    transform.eulerAngles.y,
                    offset,
                    size.x/2f
                    );
                break;
            case Convex2DShapeType.Ellipse:
                convexShape = new Ellipse2DShape(
                    transform.position,
                    transform.eulerAngles.y,
                    offset,
                    size
                    );
                break;
            default:
                convexShape = null;
                break;
        }
    }

#if UNITY_EDITOR

    private Color lineColor = Color.green;
    private Color triggerColor = Color.blue;
    private float lineThickness = 3f;

    private void OnDrawGizmos()
    {
        Vector2[] vertexs = null;

        switch (shapeType)
        {
            case Convex2DShapeType.Rect:
                vertexs = GameColliderHelper.GetRectVertexs(transform.position,transform.eulerAngles.y,offset,size);
                break;
            case Convex2DShapeType.Circle:
                vertexs = GameColliderHelper.GetCircleVertexs(transform.position, transform.eulerAngles.y, offset, size.x/2f);
                break;
            case Convex2DShapeType.Ellipse:
                vertexs = GameColliderHelper.GetEllipseVertexs(transform.position, transform.eulerAngles.y, offset, size);
                break;
            default:
                vertexs = null;
                break;
        }

        if (vertexs == null || vertexs.Length == 0) return;

        //更新位置信息
        if (convexShape!=null)
        {
            convexShape.AnchorPos = transform.position;
            convexShape.AnchorAngle = transform.eulerAngles.y;
        }

        Handles.color = lineColor;
        // 检测碰撞
        if (convexShape != null && tgtConvexComp != null)
        {
            if (GameColliderHelper.CheckCollideSATWithLeaveVector(this.convexShape, tgtConvexComp.convexShape, out Vector2 leaveV2))
            {

                Handles.color = triggerColor;

                Handles.DrawLine(
                    new Vector3(transform.position.x,0,transform.position.z),
                    new Vector3(transform.position.x + leaveV2.x,0,transform.position.z+leaveV2.y),
                    lineThickness); ;
            }
        }

        
        var edges = GameColliderHelper.Get2DShapeEdges(vertexs);
        for (int i =0;i<edges.GetLength(0);i++)
        {
            Handles.DrawLine(new Vector3(edges[i,0].x,0,edges[i,0].y) , new Vector3(edges[i, 1].x, 0, edges[i, 1].y), lineThickness);
        }


    }

#endif



}
