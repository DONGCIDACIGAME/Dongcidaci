using UnityEngine;

public static class GameColliderHelper
{
    private static Vector2[,] GetRectangleLines(GameCollider2D collider)
    {
        var vertexs = GetRectangleVertexs(collider);

        return new Vector2[4, 2] {
            {vertexs[0],vertexs[1]},
            {vertexs[1],vertexs[2]},
            {vertexs[2],vertexs[3]},
            {vertexs[3],vertexs[0]}
        };
    }

    private static Vector2[,] GetRectangleLines(Vector2 size, Vector2 offset, float anchorAngle, Vector3 anchorPos, Vector3 scale)
    {
        var vertexs = GetRectangleVertexs(size, offset, anchorAngle, anchorPos, scale);

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
        return GetRectangleVertexs(collider.size, collider.offset, collider.anchorAngle, collider.anchorPos, collider.scale);
    }


    /// <summary>
    /// 获取这个矩形的四个顶点
    /// </summary>
    /// <returns>Vector2[] 碰撞体4个顶点的坐标</returns>
    private static Vector2[] GetRectangleVertexs(Vector2 size, Vector2 offset, float anchorAngle, Vector3 anchorPos, Vector3 scale)
    {
        Vector2 _scale = new Vector2(scale.x, scale.z);

        Vector2 _size = size * _scale;
        Vector2 _offset = offset * _scale;

        float disToTgtPoint = new Vector2(_offset.x, _offset.y).magnitude;
        float rotate_angle = GetRealRotateAngle(anchorAngle, _offset.x, _offset.y);
        float realpos_x = anchorPos.x + Mathf.Sin(rotate_angle * Mathf.Deg2Rad) * disToTgtPoint;
        float realpos_z = anchorPos.z + Mathf.Cos(rotate_angle * Mathf.Deg2Rad) * disToTgtPoint;

        // 计算左上
        var originalLeftUpVector = new Vector3(-_size.x / 2, 0, _size.y / 2);
        var realLeftUpVector = Quaternion.AngleAxis(rotate_angle, Vector3.up) * originalLeftUpVector;
        var leftUpPos = new Vector2(realpos_x + realLeftUpVector.x, realpos_z + realLeftUpVector.z);

        // 计算左下
        var originalLeftDownVector = new Vector3(-_size.x / 2, 0, -_size.y / 2);
        var realLeftDownVector = Quaternion.AngleAxis(rotate_angle, Vector3.up) * originalLeftDownVector;
        var leftDownPos = new Vector2(realpos_x + realLeftDownVector.x, realpos_z + realLeftDownVector.z);

        // 计算右下
        var originalRightDownVector = new Vector3(_size.x / 2, 0, -_size.y / 2);
        var realRightDownVector = Quaternion.AngleAxis(rotate_angle, Vector3.up) * originalRightDownVector;
        var rightDownPos = new Vector2(realpos_x + realRightDownVector.x, realpos_z + realRightDownVector.z);

        // 计算右上
        var originalRightUpVector = new Vector3(_size.x / 2, 0, _size.y / 2);
        var realRightUpVector = Quaternion.AngleAxis(rotate_angle, Vector3.up) * originalRightUpVector;
        var rightUpPos = new Vector2(realpos_x + realRightUpVector.x, realpos_z + realRightUpVector.z);

        return new Vector2[4] { leftUpPos, leftDownPos, rightDownPos, rightUpPos };
    }

    private static float GetRealRotateAngle(float newAnchorRotateAngle, float offsetX2Zero, float offsetY2Zero)
    {
        // 计算距离
        var offsetV2 = new Vector2(offsetX2Zero, offsetY2Zero);
        float disToTgtPoint = offsetV2.magnitude;

        // 计算偏转角度
        // [0 - 360]
        var initOffsetAngle = Vector2.Angle(offsetV2, Vector2.up);
        initOffsetAngle = offsetX2Zero < 0 ? 360 - initOffsetAngle : initOffsetAngle;

        // 将anchorRotateAngle转化到[0,360]
        var anchorRealRoateAngle = newAnchorRotateAngle % 360;
        anchorRealRoateAngle = anchorRealRoateAngle < 0 ? anchorRealRoateAngle + 360 : anchorRealRoateAngle;

        var realAngle = (initOffsetAngle + anchorRealRoateAngle) % 360;
        return realAngle;
    }

    /// <summary>
    /// 获取这个碰撞矩形的面积
    /// </summary>
    /// <returns></returns>
    private static float GetSize(float size_x, float size_z)
    {
        return size_x * size_z;
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

    private static bool CheckColllapse(Vector2 src_size, Vector2 src_offset, float src_anchorAngle, Vector3 src_anchorPos, Vector3 src_scale,
        Vector2 tgt_size, Vector2 tgt_offset, float tgt_anchorAngle, Vector3 tgt_anchorPos, Vector3 tgt_scale)
    {
        // 所有顶点
        var src_vertexs = GetRectangleVertexs(src_size, src_offset, src_anchorAngle, src_anchorPos, src_scale);
        var srcColliderLines = GetRectangleLines(src_vertexs);


        var tgt_vertexs = GetRectangleVertexs(tgt_size, tgt_offset, tgt_anchorAngle, tgt_anchorPos, tgt_scale);
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
        if (GetSize(src_size.x, src_size.y) <= GetSize(tgt_size.x, tgt_size.y))
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

        return CheckColllapse(srcCollider.size, srcCollider.offset, srcCollider.anchorAngle, srcCollider.anchorPos, srcCollider.scale,
            tgtCollider.size, tgtCollider.offset, tgtCollider.anchorAngle, tgtCollider.anchorPos, tgtCollider.scale);
    }

    /// <summary>
    /// 检测和其它碰撞体的碰撞
    /// </summary>
    /// <param name="tgtCollider"></param>
    /// <returns></returns>
    public static bool CheckCollapse(Vector2 src_size, Vector2 src_offset, float src_anchorAngle, Vector3 src_anchorPos, Vector3 src_scale, 
        GameCollider2D tgtCollider)
    {
        if (tgtCollider == null)
        {
            Log.Error(LogLevel.Normal, "CheckCollapse Error, tgtCollider is null!");
            return false;
        }

        return CheckColllapse(src_size, src_offset, src_anchorAngle, src_anchorPos, src_scale,
            tgtCollider.size, tgtCollider.offset, tgtCollider.anchorAngle, tgtCollider.anchorPos, tgtCollider.scale);
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


    /// <summary>
    /// 获取这个旋转矩形的最大矩形包络
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="size"></param>
    public static void GetMaxEnvelopeArea(GameCollider2D collider, out Vector2 envlpPos, out Vector2 envlpSize)
    {
        GetMaxEnvelopeArea(collider.size, collider.offset, collider.anchorAngle,collider.anchorPos, collider.scale, out envlpPos, out envlpSize);
    }

    /// <summary>
    /// 获取这个旋转矩形的最大矩形包络
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="size"></param>
    public static void GetMaxEnvelopeArea(Vector2 size, Vector2 offset, float anchorAngle, Vector3 anchorPos, Vector3 scale, out Vector2 envlpPos, out Vector2 envlpSize)
    {
        var vertexs = GetRectangleVertexs(size, offset, anchorAngle, anchorPos, scale);
        GetMaxEnvelopeArea(vertexs, out envlpPos, out envlpSize);
    }
}
