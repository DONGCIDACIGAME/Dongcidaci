using UnityEngine;

/// <summary>
/// 用于描述碰撞体区域的位置信息
/// </summary>
public struct RectangleColliderVector3
{
    public float x;
    public float y;

    /// <summary>
    /// 这个碰撞体的旋转角度[0 - 360]
    /// 以向前为基准
    /// </summary>
    public float rotateAngle;

    /// <summary>
    /// 这个碰撞体的矩形尺寸
    /// </summary>
    public Vector2 size;

    public RectangleColliderVector3(float posX, float posY, float angle,Vector2 size)
    {
        this.x = posX;
        this.y = posY;
        this.rotateAngle = angle;
        this.size = size;
    }

    public RectangleColliderVector3(Vector2 offset, Vector2 initSize, Vector2 anchorPos, float anchorRotateAngle = 0, float scaleX = 1, float scaleY = 1)
    {
        // 计算距离
        float disToTgtPoint = offset.magnitude;
        // 计算偏转角度
        // [0 - 360]
        var initOffsetAngle = Vector2.Angle(offset, Vector2.up);
        initOffsetAngle = offset.x < 0 ? 360 - initOffsetAngle : initOffsetAngle;

        // 将anchorRotateAngle转化到[0,360]
        var anchorRealRoateAngle = anchorRotateAngle % 360;
        anchorRealRoateAngle = anchorRealRoateAngle < 0 ? anchorRealRoateAngle + 360 : anchorRealRoateAngle;

        var realAngle = (initOffsetAngle + anchorRealRoateAngle) % 360;
        var colliderPosX = anchorPos.x + Mathf.Sin(realAngle * Mathf.Deg2Rad) * disToTgtPoint;
        var colliderPosY = anchorPos.y + Mathf.Cos(realAngle * Mathf.Deg2Rad) * disToTgtPoint;

        this.x = colliderPosX;
        this.y = colliderPosY;
        this.rotateAngle = realAngle;
        this.size = new Vector2(initSize.x * scaleX, initSize.y * scaleY);
    }


    /// <summary>
    /// 获取这个矩形的四个顶点
    /// </summary>
    /// <returns>Vector2[] 碰撞体4个顶点的坐标</returns>
    public Vector2[] GetRectangleVertexs()
    {
        // 计算左上
        var originalLeftUpVector = new Vector3(-size.x / 2, 0, size.y / 2);
        var realLeftUpVector = Quaternion.AngleAxis(rotateAngle, Vector3.up) * originalLeftUpVector;
        var leftUpPos = new Vector2(this.x + realLeftUpVector.x, this.y + realLeftUpVector.z);

        // 计算左下
        var originalLeftDownVector = new Vector3(-size.x / 2, 0, -size.y / 2);
        var realLeftDownVector = Quaternion.AngleAxis(rotateAngle, Vector3.up) * originalLeftDownVector;
        var leftDownPos = new Vector2(this.x + realLeftDownVector.x, this.y + realLeftDownVector.z);

        // 计算右下
        var originalRightDownVector = new Vector3(size.x / 2, 0, -size.y / 2);
        var realRightDownVector = Quaternion.AngleAxis(rotateAngle, Vector3.up) * originalRightDownVector;
        var rightDownPos = new Vector2(this.x + realRightDownVector.x, this.y + realRightDownVector.z);

        // 计算右上
        var originalRightUpVector = new Vector3(size.x / 2, 0, size.y / 2);
        var realRightUpVector = Quaternion.AngleAxis(rotateAngle, Vector3.up) * originalRightUpVector;
        var rightUpPos = new Vector2(this.x + realRightUpVector.x, this.y + realRightUpVector.z);

        return new Vector2[4] {leftUpPos,leftDownPos,rightDownPos,rightUpPos};
    }


    /// <summary>
    /// 获取矩形的四边的顶点坐标
    /// 0 左上 >> 1 左下 | 1 左下 >> 2 右下 | 2 右下 >> 3 右上 | 3 右上 >> 0 左上
    /// </summary>
    /// <returns></returns>
    public Vector2[,] GetRectangleLines()
    {
        var vertexs = GetRectangleVertexs();

        return new Vector2[4, 2] { 
            {vertexs[0],vertexs[1]},
            {vertexs[1],vertexs[2]},
            {vertexs[2],vertexs[3]},
            {vertexs[3],vertexs[0]} 
        };
    }

    /// <summary>
    /// 获取这个旋转矩形的最大矩形包络
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="size"></param>
    public void GetMaxEnvelopeArea(out Vector2 pos, out Vector2 size)
    {
        var vertexs = GetRectangleVertexs();
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
    /// 获取这个碰撞矩形的面积
    /// </summary>
    /// <returns></returns>
    public float SizeArea()
    {
        return size.x * size.y;
    }

    /// <summary>
    /// 检测某个点是否在矩形中
    /// 包含贴边的情况
    /// </summary>
    /// <param name="checkPoint"></param>
    /// <returns></returns>
    public bool CheckPointInRectangle(Vector2 checkPoint)
    {
        var rectangleVertexs = GetRectangleVertexs();
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

    /// <summary>
    /// 判断与目标碰撞体是否产生了相交
    /// 两个带旋转的矩形检测相交
    /// </summary>
    /// <param name="tgtColliderPos"></param>
    /// <returns></returns>
    public bool CheckCollapse(RectangleColliderVector3 tgtColliderPos)
    {
        var crtColliderLines = GetRectangleLines();
        var tgtColliderLines = tgtColliderPos.GetRectangleLines();

        // 查找是否存在线条交差的情况
        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < 4; k++)
            {
                // ab crtline ; cd tgtline
                var crtLineVectorAB = new Vector2(crtColliderLines[i, 1].x - crtColliderLines[i, 0].x, crtColliderLines[i, 1].y - crtColliderLines[i, 0].y);
                var lineVectorAC = new Vector2(tgtColliderLines[k, 0].x - crtColliderLines[i, 0].x, tgtColliderLines[k, 0].y - crtColliderLines[i, 0].y);
                var lineVectorAD = new Vector2(tgtColliderLines[k, 1].x - crtColliderLines[i, 0].x, tgtColliderLines[k, 1].y - crtColliderLines[i, 0].y);

                var crossMultiAC2AB = lineVectorAC.x * crtLineVectorAB.y - lineVectorAC.y * crtLineVectorAB.x;
                var crossMultiAD2AB = lineVectorAD.x * crtLineVectorAB.y - lineVectorAD.y * crtLineVectorAB.x;
                // crossAC2AB * crossAD2AB <0 说明cd 在ab的两侧，同理判断ab是否在cd两侧
                var tgtLineVectorCD = new Vector2(tgtColliderLines[k, 1].x - tgtColliderLines[k, 0].x, tgtColliderLines[k, 1].y - tgtColliderLines[k, 0].y);
                var lineVectorCA = new Vector2(crtColliderLines[i, 0].x - tgtColliderLines[k, 0].x, crtColliderLines[i, 0].y - tgtColliderLines[k, 0].y);
                var lineVectorCB = new Vector2(crtColliderLines[i, 1].x - tgtColliderLines[k, 0].x, crtColliderLines[i, 1].y - tgtColliderLines[k, 0].y);
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
        if (SizeArea() <= tgtColliderPos.SizeArea())
        {
            // 判断这个碰撞体是否在目标中
            var vertexs = GetRectangleVertexs();
            if (tgtColliderPos.CheckPointInRectangle(vertexs[0]) && tgtColliderPos.CheckPointInRectangle(vertexs[1]) && tgtColliderPos.CheckPointInRectangle(vertexs[2]) && tgtColliderPos.CheckPointInRectangle(vertexs[3]))
            {
                return true;
            }
        }
        else if (SizeArea() > tgtColliderPos.SizeArea())
        {
            // 判断目标矩形是否在这个碰撞体中
            var tgtVertexs = tgtColliderPos.GetRectangleVertexs();
            if (CheckPointInRectangle(tgtVertexs[0]) && CheckPointInRectangle(tgtVertexs[1]) && CheckPointInRectangle(tgtVertexs[2]) && CheckPointInRectangle(tgtVertexs[3]))
            {
                return true;
            }
        }


        return false;
    }
}


public class GameCollider2D : IGameCollider2D
{
    
    private GameColliderData2D _colliderInitData;
    /// <summary>
    /// 该碰撞体的初始配置数据
    /// </summary>
    public GameColliderData2D CollideInitData => _colliderInitData;


    private RectangleColliderVector3 _rectPosV3;

    /// <summary>
    /// 设置当前碰撞体的位置信息
    /// </summary>
    /// <param name="anchorPos"></param>
    /// <param name="anchorRotateAngle"></param>
    /// <param name="scaleX"></param>
    /// <param name="scaleY"></param>
    public void SetCollideRectPos(Vector2 anchorPos, float anchorRotateAngle = 0, float scaleX = 1, float scaleY = 1)
    {
        if (_colliderInitData == null)
        {
            this._rectPosV3 = new RectangleColliderVector3();
            return;
        }

        this._rectPosV3 = new RectangleColliderVector3(_colliderInitData.offset, _colliderInitData.size, anchorPos, anchorRotateAngle, scaleX, scaleY);

    }

    /// <summary>
    /// 用于描述这个碰撞体的位置信息
    /// 包含位置信息，旋转信息和尺寸信息
    /// </summary>
    public RectangleColliderVector3 RectanglePosv3 => _rectPosV3;


    private ICollideProcessor _colliderProcessor;
    public ICollideProcessor GetCollideProcessor()
    {
        return _colliderProcessor;
    }

    /// <summary>
    /// 创建一个2d碰撞体
    /// </summary>
    /// <param name="initialcolliderData"></param>
    /// <param name="collideProcessor"></param>
    /// <param name="anchorPos"></param>
    /// <param name="anchorRotateAngle"></param>
    /// <param name="scaleX"></param>
    /// <param name="scaleY"></param>
    public GameCollider2D(GameColliderData2D initialColliderData,ICollideProcessor collideProcessor,Vector2 anchorPos, float anchorRotateAngle = 0, float scaleX =1, float scaleY =1)
    {
        this._colliderInitData = initialColliderData;
        this._colliderProcessor = collideProcessor;
        SetCollideRectPos(anchorPos,anchorRotateAngle,scaleX,scaleY);
    }

    /// <summary>
    /// 判断与目标碰撞体是否产生了相交
    /// 两个带旋转的矩形检测相交
    /// </summary>
    /// <param name="tgtColliderPos"></param>
    /// <returns></returns>
    public bool CheckCollapse(RectangleColliderVector3 tgtColliderPos)
    {
        return _rectPosV3.CheckCollapse(tgtColliderPos);
    }

    /// <summary>
    /// 检测与无旋转矩形区域的碰撞
    /// </summary>
    /// <param name="area"></param>
    /// <returns></returns>
    public bool CheckCollapse(Rect area)
    {
        Vector2 size = new Vector2(area.width, area.height);
        return CheckCollapse(new RectangleColliderVector3(area.x,area.y,0,size));
    }

    /// <summary>
    /// 检测和其它碰撞体的碰撞
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool CheckCollapse(GameCollider2D other)
    {
        return CheckCollapse(other.RectanglePosv3);
    }

    /// <summary>
    /// 判断某个位置点是否在此碰撞区域内
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckPosInCollider(Vector2 pos)
    {
        return this._rectPosV3.CheckPointInRectangle(pos);
    }

    /// <summary>
    /// 当碰撞发生时
    /// </summary>
    /// <param name="other"></param>
    public void OnColliderEnter(IGameCollider other)
    {
        if (this._colliderProcessor !=null)
        {
            //这个碰撞处理机处理碰撞
            this._colliderProcessor.HandleCollideTo(other.GetCollideProcessor());
        }
    }

    public void Dispose()
    {
        //游戏体被销毁
        GameColliderManager.Ins.UnRegisterGameCollider(this);
        this._colliderInitData = null;
        this._colliderProcessor = null;
    }

}
