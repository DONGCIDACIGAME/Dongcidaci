using UnityEngine;

public static class GameColliderHelper
{
    /// <summary>
    /// 获取旋转矩形的4个顶点，左上 右上,右下,左下
    /// </summary>
    /// <param name="anchorPos"></param>
    /// <param name="anchorAngle"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static Vector2[] GetRectVertexs(Vector3 anchorPos, float anchorAngle, Vector2 offset, Vector2 size)
    {
        var rectVertexs = new Vector2[4];
        // 1 原始位置
        rectVertexs[0] = new Vector2(-size.x/2f + offset.x, size.y/2f + offset.y);
        rectVertexs[1] = new Vector2(size.x / 2f + offset.x, size.y / 2f + offset.y);
        rectVertexs[2] = new Vector2(size.x / 2f + offset.x, -size.y / 2f + offset.y);
        rectVertexs[3] = new Vector2(-size.x / 2f + offset.x, -size.y / 2f + offset.y);

        // 2 绕 anchorAngle 旋转
        float arc = -anchorAngle / 180 * Mathf.PI;
        for (int i =0;i<rectVertexs.Length;i++)
        {
            rectVertexs[i] = new Vector2(
                rectVertexs[i].x * Mathf.Cos(arc) - rectVertexs[i].y * Mathf.Sin(arc),
                rectVertexs[i].x * Mathf.Sin(arc) + rectVertexs[i].y * Mathf.Cos(arc));
        }

        // 3 加上锚点信息
        for (int i = 0; i < rectVertexs.Length; i++)
        {
            rectVertexs[i] += new Vector2(anchorPos.x,anchorPos.z);
        }

        return rectVertexs;
    }

    /// <summary>
    /// 获取旋转圆的顶点坐标,可以指定需要的精度
    /// 精度至少大于4
    /// </summary>
    /// <param name="anchorPos"></param>
    /// <param name="anchorAngle"></param>
    /// <param name="offset"></param>
    /// <param name="radius"></param>
    /// <param name="vertexCount"></param>
    /// <returns></returns>
    public static Vector2[] GetCircleVertexs(Vector3 anchorPos, float anchorAngle, Vector2 offset, float radius,int vertexCount)
    {
        if (vertexCount < 4) return null;
        var circleVetexs = new Vector2[vertexCount];
        // 1 根据精度计算初始点
        float stepAngle = 360f / (float)vertexCount;
        var baseV3 = new Vector3(-radius, 0, 0);
        for (int i = 0; i < circleVetexs.Length; i++)
        {
            if (i == 0)
            {
                circleVetexs[i] = new Vector2(baseV3.x + offset.x, baseV3.z + offset.y);
                continue;
            }

            var newV3 = Quaternion.AngleAxis(stepAngle * i, Vector3.up) * baseV3;
            circleVetexs[i] = new Vector2(newV3.x + offset.x, newV3.z + offset.y);
        }

        // 2 根据 anchor angle 旋转
        float arc = -anchorAngle / 180 * Mathf.PI;
        for (int i = 0; i < circleVetexs.Length; i++)
        {
            circleVetexs[i] = new Vector2(
                circleVetexs[i].x * Mathf.Cos(arc) - circleVetexs[i].y * Mathf.Sin(arc),
                circleVetexs[i].x * Mathf.Sin(arc) + circleVetexs[i].y * Mathf.Cos(arc));
        }

        // 加上锚点的位置
        var anchorPosV2 = new Vector2(anchorPos.x, anchorPos.z);
        for (int i = 0; i < circleVetexs.Length; i++)
        {
            circleVetexs[i] += anchorPosV2;
        }

        return circleVetexs;
    }

    public static Vector2[] GetEllipseVertexs(Vector3 anchorPos, float anchorAngle, Vector2 offset, Vector2 size, int vertexCount)
    {
        if (vertexCount < 4) return null;
        // 判断是否是圆
        if (size.x == size.y) return GetCircleVertexs(anchorPos,anchorAngle,offset,size.x/2f,vertexCount);
        var ellipseVetexs = new Vector2[vertexCount];
        //x^2/size.x^2 + y^2/size.y^2 = 1

        // 1 计算原始点
        float stepAngle = 360f / (float)vertexCount;
        var baseV3 = new Vector3(-size.x/2f, 0, 0);
        for (int i = 0; i < ellipseVetexs.Length; i++)
        {
            if (i == 0)
            {
                ellipseVetexs[i] = new Vector2(baseV3.x + offset.x, baseV3.z + offset.y);
                continue;
            }

            var newDirV3 = Quaternion.AngleAxis(stepAngle * i, Vector3.up) * baseV3;

            float yOnEllipse = 0;
            float xOnEllipse = 0;
            if (stepAngle * i  == 180 || newDirV3.z == 0f || newDirV3.z ==0)
            {
                xOnEllipse = size.x / 2f;
                yOnEllipse = 0;
            }
            else
            {
                float kSuqare = Mathf.Pow(newDirV3.x / newDirV3.z, 2);
                float aSquare = Mathf.Pow(size.x / 2f, 2);
                float bSquare = Mathf.Pow(size.y / 2f, 2);

                yOnEllipse = Mathf.Sqrt((aSquare * bSquare) / (aSquare + kSuqare * bSquare));
                xOnEllipse = Mathf.Abs(newDirV3.x) / Mathf.Abs(newDirV3.z) * yOnEllipse;

                // 需要根据newDirV3这个向量，求解在椭圆上的具体的点
                if (stepAngle * i <= 90)
                {
                    // x<0,y>0
                    xOnEllipse = -xOnEllipse;


                }
                else if (stepAngle * i > 90 && stepAngle * i <= 180)
                {
                    // x >0, y>0


                }
                else if (stepAngle * i > 180 && stepAngle * i <= 270)
                {
                    // x >0 y<0
                    yOnEllipse = -yOnEllipse;

                }
                else if (stepAngle * i > 270 && stepAngle * i <= 360)
                {
                    // x <0 y<0
                    xOnEllipse = -xOnEllipse;
                    yOnEllipse = -yOnEllipse;
                }
            }

            ellipseVetexs[i] = new Vector2(xOnEllipse + offset.x, yOnEllipse + offset.y);
            //Debug.LogError(ellipseVetexs[i]);
            //Log.Error(LogLevel.Normal,ellipseVetexs[i].ToString());
        }

        /**
        if (size.x > size.y)
        {
            // focus on x
            // x^2/size.x^2 + y^2/size.y^2 = 1
        }
        else
        {
            // focus on y
            // y^2/size.x^2 + x^2/size.y^2 = 1

        }
        */

        // 2 根据 anchor angle 旋转
        float arc = -anchorAngle / 180 * Mathf.PI;
        for (int i = 0; i < ellipseVetexs.Length; i++)
        {
            ellipseVetexs[i] = new Vector2(
                ellipseVetexs[i].x * Mathf.Cos(arc) - ellipseVetexs[i].y * Mathf.Sin(arc),
                ellipseVetexs[i].x * Mathf.Sin(arc) + ellipseVetexs[i].y * Mathf.Cos(arc));
        }

        // 加上锚点的位置
        var anchorPosV2 = new Vector2(anchorPos.x, anchorPos.z);
        for (int i = 0; i < ellipseVetexs.Length; i++)
        {
            ellipseVetexs[i] += anchorPosV2;
        }

        return ellipseVetexs;

    }






    public static Vector2[,] GetRectangleLines(GameCollider2D collider)
    {
        var vertexs = GetRectangleVertexs(collider);

        return new Vector2[4, 2] {
            {vertexs[0],vertexs[1]},
            {vertexs[1],vertexs[2]},
            {vertexs[2],vertexs[3]},
            {vertexs[3],vertexs[0]}
        };
    }

    private static Vector2[,] GetRectangleLines(Vector3 anchorPos, float anchorAngle, Vector2 offset, Vector2 size)
    {
        var vertexs = GetRectangleVertexs(anchorPos,anchorAngle,offset,size);
        return GetRectangleLines(vertexs);
    }

    private static Vector2[,] GetRectangleLines(Vector2[] vertexs)
    {
        return new Vector2[4, 2] {
            {vertexs[0],vertexs[1]},
            {vertexs[1],vertexs[2]},
            {vertexs[2],vertexs[3]},
            {vertexs[3],vertexs[0]}
        };
    }

    private static Vector2[] GetRectangleVertexs(GameCollider2D collider)
    {
        return GetRectangleVertexs(collider.AnchorPos, collider.AnchorAngle, collider.Offset, collider.Size);
    }


    /// <summary>
    /// 获取这个矩形的四个顶点
    /// 这个方法可能有问题!!!
    /// </summary>
    /// <returns>Vector2[] 碰撞体4个顶点的坐标</returns>
    private static Vector2[] GetRectangleVertexs(Vector3 anchorPos, float anchorAngle, Vector2 offset, Vector2 size)
    {

        float halfWidth = size.x / 2f;
        float halfHeight = size.y / 2f;

        float arc = -anchorAngle / 180 * Mathf.PI;
        // 自身有旋转，计算四个顶点的本地坐标
        Vector2 ltOrigin = new Vector2(-halfWidth + offset.x, halfHeight + offset.y);
        Vector2 ltOffset = new Vector2(
            ltOrigin.x * Mathf.Cos(arc) - ltOrigin.y * Mathf.Sin(arc),
            ltOrigin.x * Mathf.Sin(arc) + ltOrigin.y * Mathf.Cos(arc));


        Vector2 lbOrigin = new Vector2(-halfWidth + offset.x, -halfHeight + offset.y);
        Vector2 lbOffset = new Vector2(
            lbOrigin.x * Mathf.Cos(arc) - lbOrigin.y * Mathf.Sin(arc),
            lbOrigin.x * Mathf.Sin(arc) + lbOrigin.y * Mathf.Cos(arc));

        Vector2 rbOrigin = new Vector2(halfWidth + offset.x, -halfHeight + offset.y);
        Vector2 rbOffset = new Vector2(
            rbOrigin.x * Mathf.Cos(arc) - rbOrigin.y * Mathf.Sin(arc),
            rbOrigin.x * Mathf.Sin(arc) + rbOrigin.y * Mathf.Cos(arc));

        Vector2 rtOrigin = new Vector2(halfWidth + offset.x, halfHeight + offset.y);
        Vector2 rtOffset = new Vector2(
            rtOrigin.x * Mathf.Cos(arc) - rtOrigin.y * Mathf.Sin(arc),
            rtOrigin.x * Mathf.Sin(arc) + rtOrigin.y * Mathf.Cos(arc));

        // 计算四个定点的世界坐标
        Vector2 groundPos = new Vector2(anchorPos.x,anchorPos.z);

        Vector2 leftUpPos = groundPos + ltOffset;
        Vector2 leftDownPos = groundPos + lbOffset;
        Vector2 rightDownPos = groundPos + rbOffset;
        Vector2 rightUpPos = groundPos + rtOffset;
        
        return new Vector2[4] { leftUpPos, leftDownPos, rightDownPos, rightUpPos };
    }

    /// <summary>
    /// 获取这个碰撞矩形的面积
    /// </summary>
    /// <returns></returns>
    private static float GetSize(float sizeX, float sizeZ)
    {
        return sizeX * sizeZ;
    }

    private static bool CheckPosInCollider(Vector2 checkPoint, Vector2[] rectangleVertexs)
    {
        var vectorAE = new Vector2(checkPoint.x - rectangleVertexs[0].x, checkPoint.y - rectangleVertexs[0].y);
        var vectorBE = new Vector2(checkPoint.x - rectangleVertexs[1].x, checkPoint.y - rectangleVertexs[1].y);
        var vectorCE = new Vector2(checkPoint.x - rectangleVertexs[2].x, checkPoint.y - rectangleVertexs[2].y);
        var vectorDE = new Vector2(checkPoint.x - rectangleVertexs[3].x, checkPoint.y - rectangleVertexs[3].y);
        var crossMultiAE2BE = vectorAE.x * vectorBE.y - vectorAE.y * vectorBE.x;
        var crossMultiBE2CE = vectorBE.x * vectorCE.y - vectorBE.y * vectorCE.x;
        var crossMultiCE2DE = vectorCE.x * vectorDE.y - vectorCE.y * vectorDE.x;
        var crossMultiDE2AE = vectorDE.x * vectorAE.y - vectorDE.y * vectorAE.x;

        if (crossMultiAE2BE >= 0 && crossMultiBE2CE >= 0 && crossMultiCE2DE >= 0 && crossMultiDE2AE >= 0) return true;
        return false;
    }

    private static bool CheckColllapse(Vector3 srcAnchorPos, float srcAnchorAngle, Vector2 srcOffset, Vector2 srcSize, Vector3 tgtAnchorPos, float tgtAnchorAngle, Vector2 tgtOffset, Vector2 tgtSize)
    {
        // 所有顶点
        var src_vertexs = GetRectangleVertexs(srcAnchorPos, srcAnchorAngle, srcOffset, srcSize);
        var srcColliderLines = GetRectangleLines(src_vertexs);
        var tgt_vertexs = GetRectangleVertexs(tgtAnchorPos, tgtAnchorAngle, tgtOffset, tgtSize);
        var tgtColliderLines = GetRectangleLines(tgt_vertexs);

        // 查找是否存在线条交差的情况
        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < 4; k++)
            {
                // ab crtline ; cd tgtline
                var crtLineVectorAB = new Vector2(srcColliderLines[i, 1].x - srcColliderLines[i, 0].x, srcColliderLines[i, 1].y - srcColliderLines[i, 0].y);
                var lineVectorAC = new Vector2(tgtColliderLines[k, 0].x - srcColliderLines[i, 0].x, tgtColliderLines[k, 0].y - srcColliderLines[i, 0].y);
                var lineVectorAD = new Vector2(tgtColliderLines[k, 1].x - srcColliderLines[i, 0].x, tgtColliderLines[k, 1].y - srcColliderLines[i, 0].y);

                var crossMultiAC2AB = lineVectorAC.x * crtLineVectorAB.y - lineVectorAC.y * crtLineVectorAB.x;
                var crossMultiAD2AB = lineVectorAD.x * crtLineVectorAB.y - lineVectorAD.y * crtLineVectorAB.x;
                // crossAC2AB * crossAD2AB <0 说明cd 在ab的两侧，同理判断ab是否在cd两侧
                var tgtLineVectorCD = new Vector2(tgtColliderLines[k, 1].x - tgtColliderLines[k, 0].x, tgtColliderLines[k, 1].y - tgtColliderLines[k, 0].y);
                var lineVectorCA = new Vector2(srcColliderLines[i, 0].x - tgtColliderLines[k, 0].x, srcColliderLines[i, 0].y - tgtColliderLines[k, 0].y);
                var lineVectorCB = new Vector2(srcColliderLines[i, 1].x - tgtColliderLines[k, 0].x, srcColliderLines[i, 1].y - tgtColliderLines[k, 0].y);
                var crossMultiCA2CD = lineVectorCA.x * tgtLineVectorCD.y - lineVectorCA.y * tgtLineVectorCD.x;
                var crossMultiCB2CD = lineVectorCB.x * tgtLineVectorCD.y - lineVectorCB.y * tgtLineVectorCD.x;

                if (crossMultiAC2AB * crossMultiAD2AB < 0 && crossMultiCA2CD * crossMultiCB2CD < 0)
                {
                    // ab 和 cd 两条线相交
                    return true;
                }
            }
        }

        // added 0522
        // 存在不相交但是一个矩形完全在另一个矩形内部的情况
        // 判断哪个矩形的面积更小
        if (GetSize(srcSize.x, srcSize.y) <= GetSize(tgtSize.x, tgtSize.y))
        {
            // 判断这个碰撞体是否在目标中
            if (CheckPosInCollider(src_vertexs[0], tgt_vertexs) && CheckPosInCollider(src_vertexs[1], tgt_vertexs)
                && CheckPosInCollider(src_vertexs[2], tgt_vertexs) && CheckPosInCollider(src_vertexs[3], tgt_vertexs))
            {
                return true;
            }
        }
        else
        {
            // 判断目标矩形是否在这个碰撞体中
            if (CheckPosInCollider(tgt_vertexs[0], src_vertexs) && CheckPosInCollider(tgt_vertexs[1], src_vertexs)
                && CheckPosInCollider(tgt_vertexs[2], src_vertexs) && CheckPosInCollider(tgt_vertexs[3], src_vertexs))
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 检测和其它碰撞体的碰撞
    /// </summary>
    /// <param name="tgtCollider"></param>
    /// <returns></returns>
    public static bool CheckCollapse(GameCollider2D srcCollider, GameCollider2D tgtCollider)
    {
        if(srcCollider == null)
        {
            Log.Error(LogLevel.Normal, "CheckCollapse Error, srcCollider is null!");
            return false;
        }

        if (tgtCollider == null)
        {
            Log.Error(LogLevel.Normal, "CheckCollapse Error, tgtCollider is null!");
            return false;
        }

        return CheckColllapse(srcCollider.AnchorPos, srcCollider.AnchorAngle, srcCollider.Offset, srcCollider.Size,
            tgtCollider.AnchorPos, tgtCollider.AnchorAngle, tgtCollider.Offset, tgtCollider.Size);
    }

    public static bool CheckCollapse(Vector3 srcAnchorPos, float srcAnchorAngle, Vector2 srcOffset, Vector2 srcSize,
        GameCollider2D tgtCollider)
    {
        if (tgtCollider == null)
        {
            Log.Error(LogLevel.Normal, "CheckCollapse Error, tgtCollider is null!");
            return false;
        }

        return CheckColllapse(srcAnchorPos, srcAnchorAngle, srcOffset, srcSize, tgtCollider.AnchorPos, tgtCollider.AnchorAngle, tgtCollider.Offset, tgtCollider.Size);
    }

    private static void GetMaxEnvelopeArea(Vector2[] vertexs, out Vector2 pos, out Vector2 size)
    {
        // get max width
        float maxWidth = 0;
        float maxHeight = 0;
        float newPosX = 0;
        float newPosY = 0;
        for (int i = 0; i < vertexs.Length; i++)
        {
            var startVector = vertexs[i];
            for (int k = i + 1; k < vertexs.Length; k++)
            {
                var checkVector = vertexs[k];
                var xGap = checkVector.x - startVector.x;
                if (Mathf.Abs(xGap) > maxWidth)
                {
                    maxWidth = Mathf.Abs(xGap);
                    newPosX = startVector.x + xGap / 2f;
                }
                var yGap = checkVector.y - startVector.y;
                if (Mathf.Abs(yGap) > maxHeight)
                {
                    maxHeight = Mathf.Abs(yGap);
                    newPosY = startVector.y + yGap / 2f;
                }
            }
        }
        pos = new Vector2(newPosX, newPosY);
        size = new Vector2(maxWidth, maxHeight);
    }


    
    public static void GetMaxEnvelopeArea(GameCollider2D collider, out Vector2 envlpPos, out Vector2 envlpSize)
    {
        GetMaxEnvelopeArea(collider.AnchorPos, collider.AnchorAngle, collider.Offset,collider.Size, out envlpPos, out envlpSize);
    }

    
    public static void GetMaxEnvelopeArea(Vector3 anchorPos, float anchorAngle, Vector2 offset, Vector2 size, out Vector2 envlpPos, out Vector2 envlpSize)
    {
        var vertexs = GetRectangleVertexs(anchorPos,anchorAngle,offset,size);
        GetMaxEnvelopeArea(vertexs, out envlpPos, out envlpSize);
    }




}
