using GameEngine;
using UnityEngine;

public class GameCollider2D : IGameCollider
{
    /// <summary>
    /// 自增型id生成器
    /// </summary>
    private static AutoIncrementIndex _indexer = new AutoIncrementIndex();

    //private float _realPosX;
    //private float _realPosY;
    //private float _rotateAngle = 0;
    //private float _scaleX = 1;
    //private float _scaleY = 1;
    //private float _sizeX;
    //private float _sizeY;
    //private float _offsetX = 0;
    //private float _offsetY = 0;

    /// <summary>
    /// 碰撞体的id
    /// </summary>
    private int _colliderId;
    public int GetColliderId()
    {
        return _colliderId;
    }

    /// <summary>
    /// 绑定的实体ID
    /// </summary>
    private int _entityId;
    public int GetBindEntityId()
    {
        return _entityId;
    }

    /// <summary>
    /// 碰撞类型
    /// </summary>
    private int _colliderType;
    public int GetColliderType()
    {
        return _colliderType;
    }

    /// <summary>
    /// 碰撞处理
    /// </summary>
    private IGameColliderHandler _colliderHandler;
    public IGameColliderHandler GetColliderHandler()
    {
        return _colliderHandler;
    }

    /*

    public GameCollider2D(int colliderType, int entityId, GameColliderData2D initialColliderData, 
        IGameColliderHandler handler, Vector2 anchorPos,
        float anchorRotateAngle = 0, float scaleX = 1, float scaleY = 1)
    {
        // 生成唯一id
        this._colliderId = _indexer.GetIndex();
        // 碰撞类型
        this._colliderType = colliderType;
        // 绑定的entityId
        this._entityId = entityId;
        // 碰撞处理
        this._colliderHandler = handler;

        // 0616 new logic
        this._sizeX = initialColliderData.size.x;
        this._sizeY = initialColliderData.size.y;
        this._offsetX = initialColliderData.offset.x;
        this._offsetY = initialColliderData.offset.y;
        this._scaleX = scaleX;
        this._scaleY = scaleY;
        var disToTgtPoint = new Vector2(_offsetX, _offsetY).magnitude;
        _rotateAngle = GetRealRotateAngle(anchorRotateAngle, _offsetX, _offsetY);
        _realPosX = anchorPos.x + Mathf.Sin(_rotateAngle * Mathf.Deg2Rad) * disToTgtPoint;
        _realPosY = anchorPos.y + Mathf.Cos(_rotateAngle * Mathf.Deg2Rad) * disToTgtPoint;
    }

    private float GetRealRotateAngle(float newAnchorRotateAngle, float offsetX2Zero, float offsetY2Zero)
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
    public void UpdateCollider2DInfo(Vector2 newAnchorPos, float newAnchorRotateAngle = 0, float scaleX = 1, float scaleY = 1)
    {
        // 1 首先更新缩放的信息
        float scaleXRatio = scaleX / _scaleX;
        float scaleYRatio = scaleY / _scaleY;
        this._sizeX = scaleXRatio * this._sizeX;
        this._sizeY = scaleYRatio * this._sizeY;
        this._offsetX = scaleXRatio * this._offsetX;
        this._offsetY = scaleYRatio * this._offsetY;
        var disToTgtPoint = new Vector2(_offsetX, _offsetY).magnitude;
        _rotateAngle = GetRealRotateAngle(newAnchorRotateAngle, _offsetX, _offsetY);
        _realPosX = newAnchorPos.x + Mathf.Sin(_rotateAngle * Mathf.Deg2Rad) * disToTgtPoint;
        _realPosY = newAnchorPos.y + Mathf.Cos(_rotateAngle * Mathf.Deg2Rad) * disToTgtPoint;
    }
    /// <summary>
    /// 获取这个矩形的四个顶点
    /// </summary>
    /// <returns>Vector2[] 碰撞体4个顶点的坐标</returns>
    public Vector2[] GetRectangleVertexs()
    {
        // 计算左上
        var originalLeftUpVector = new Vector3(-_sizeX / 2, 0, _sizeY / 2);
        var realLeftUpVector = Quaternion.AngleAxis(_rotateAngle, Vector3.up) * originalLeftUpVector;
        var leftUpPos = new Vector2(_realPosX + realLeftUpVector.x, _realPosY + realLeftUpVector.z);
        // 计算左下
        var originalLeftDownVector = new Vector3(-_sizeX / 2, 0, -_sizeY / 2);
        var realLeftDownVector = Quaternion.AngleAxis(_rotateAngle, Vector3.up) * originalLeftDownVector;
        var leftDownPos = new Vector2(_realPosX + realLeftDownVector.x, _realPosY + realLeftDownVector.z);
        // 计算右下
        var originalRightDownVector = new Vector3(_sizeX / 2, 0, -_sizeY / 2);
        var realRightDownVector = Quaternion.AngleAxis(_rotateAngle, Vector3.up) * originalRightDownVector;
        var rightDownPos = new Vector2(_realPosX + realRightDownVector.x, _realPosY + realRightDownVector.z);
        // 计算右上
        var originalRightUpVector = new Vector3(_sizeX / 2, 0, _sizeY / 2);
        var realRightUpVector = Quaternion.AngleAxis(_rotateAngle, Vector3.up) * originalRightUpVector;
        var rightUpPos = new Vector2(_realPosX + realRightUpVector.x, _realPosY + realRightUpVector.z);
        return new Vector2[4] { leftUpPos, leftDownPos, rightDownPos, rightUpPos };
    }
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
    /// 获取这个碰撞矩形的面积
    /// </summary>
    /// <returns></returns>
    public float SizeArea()
    {
        return _sizeX * _sizeY;
    }

    public bool CheckPosInCollider(Vector2 checkPoint)
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
    /// 检测和其它碰撞体的碰撞
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool CheckCollapse(GameCollider2D other)
    {
        var crtColliderLines = GetRectangleLines();
        var tgtColliderLines = other.GetRectangleLines();
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
        if (SizeArea() <= other.SizeArea())
        {
            // 判断这个碰撞体是否在目标中
            var vertexs = GetRectangleVertexs();
            if (other.CheckPosInCollider(vertexs[0]) && other.CheckPosInCollider(vertexs[1]) && other.CheckPosInCollider(vertexs[2]) && other.CheckPosInCollider(vertexs[3]))
            {
                return true;
            }
        }
        else if (SizeArea() > other.SizeArea())
        {
            // 判断目标矩形是否在这个碰撞体中
            var tgtVertexs = other.GetRectangleVertexs();
            if (CheckPosInCollider(tgtVertexs[0]) && CheckPosInCollider(tgtVertexs[1]) && CheckPosInCollider(tgtVertexs[2]) && CheckPosInCollider(tgtVertexs[3]))
            {
                return true;
            }
        }
        return false;
    }

    /**
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
    */

    public void Dispose()
    {
        //_realPosX = 0;
        //_realPosY = 0;
        //_rotateAngle  = 0;

        //_scaleX = 1;
        //_scaleY = 1;

        //_sizeX = 0;
        //_sizeY = 0;
        //_offsetX = 0;
        //_offsetY = 0;
        //_entityId = 0;

        size = Vector2.zero;
        offset = Vector2.zero;
        scale = Vector3.zero;
        anchorPos = Vector3.zero;
        anchorAngle = 0;

        //游戏体被销毁
        GameColliderManager.Ins.UnRegisterGameCollider(this);
    }



    public Vector3 anchorPos { get; private set; }
    public float anchorAngle { get; private set; }
    public Vector3 scale { get; private set; }
    public Vector2 size { get; private set; }
    public Vector2 offset { get; private set; }

    public GameCollider2D(int colliderType, int entityId,GameColliderData2D initialColliderData, IGameColliderHandler handler)
    {
        // 生成唯一id
        this._colliderId = _indexer.GetIndex();
        // 碰撞类型
        this._colliderType = colliderType;
        // 绑定的entityId
        this._entityId = entityId;
        // 碰撞处理
        this._colliderHandler = handler;
        // 碰撞的大小和偏移
        this.size = initialColliderData.size;
        this.offset = initialColliderData.offset;
    }

    public void UpdateColliderPos(Vector3 newAnchorPos)
    {
        anchorPos = newAnchorPos;
    }

    public void UpdateColliderRot(float newAnchorRotateAngle)
    {
        anchorAngle  = newAnchorRotateAngle;
    }

    public void UpdateColliderScale(Vector3 newScale)
    {
        scale = newScale;
    }
}
