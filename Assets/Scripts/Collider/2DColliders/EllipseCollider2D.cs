using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

public class EllipseCollider2D : ConvexCollider2D
{
    //对椭圆精度的描述，必须大于4
    private const int polyVertexCount = 8;

    public override Vector2[] GetSortedVertexs()
    {
        if(this._size.x == this._size.y)
        {
            // circle
            return GetSortedVeterxsInCricle();
        }



        return null;
    }

    private Vector2[] GetSortedVeterxsInCricle()
    {
        //计算未绕轴旋转的实际的点的位置
        var originVetexs = new Vector2[polyVertexCount];
        float stepAngle = 360f / (float)polyVertexCount;
        var baseV3 = new Vector3(-Size.x / 2f, 0,0);
        for (int i=0;i<originVetexs.Length;i++)
        {
            if (i == 0)
            {
                originVetexs[i] = new Vector2(baseV3.x + Offset.x,baseV3.z + Offset.y);
                continue;
            }

            var newV3 = Quaternion.AngleAxis(stepAngle * i, Vector3.up) * baseV3;
            originVetexs[i] = new Vector2(newV3.x + Offset.x, newV3.z + Offset.y);
        }

        // 根据 anchor angle 旋转
        float arc = -AnchorAngle / 180 * Mathf.PI;
        for (int i = 0; i < originVetexs.Length; i++)
        {
            originVetexs[i] = new Vector2(
                originVetexs[i].x * Mathf.Cos(arc) - originVetexs[i].y * Mathf.Sin(arc),
                originVetexs[i].x * Mathf.Sin(arc) + originVetexs[i].y * Mathf.Cos(arc));
        }

        // 加上锚点的位置
        var anchorPos = new Vector2(AnchorPos.x, AnchorPos.z);
        for (int i = 0; i < originVetexs.Length; i++)
        {
            originVetexs[i] += anchorPos;
        }

        return originVetexs;
    }



    public override void Recycle()
    {
        Dispose();
        GamePoolCenter.Ins.EllipseCollider2DPool.Push(this);
    }



}
