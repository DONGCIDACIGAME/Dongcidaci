using UnityEngine;

/// <summary>
/// 用于描述碰撞体区域的位置信息
/// </summary>
public struct RectangleColliderVector3
{
    public float x;
    public float y;
    /// <summary>
    /// 这个碰撞体的旋转角度[-180 - 180]
    /// 以向前为基准
    /// </summary>
    public float rotateAngle;
    public RectangleColliderVector3(float posX, float posY, float angle)
    {
        this.x = posX;
        this.y = posY;
        this.rotateAngle = angle;
    }

    /// <summary>
    /// 获取这个矩形的四个顶点
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public Vector2[] GetRectangleVertexs(Vector2 size)
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
    /// <param name="size"></param>
    /// <returns></returns>
    public Vector2[,] GetRectangleLines(Vector2 size)
    {
        var vertexs = GetRectangleVertexs(size);

        return new Vector2[4, 2] { 
            {vertexs[0],vertexs[1]},
            {vertexs[1],vertexs[2]},
            {vertexs[2],vertexs[3]},
            {vertexs[3],vertexs[0]} 
        };
    }

}


public abstract class GameCollider2D : IGameCollider2D
{
    
    private GameColliderData2D _colliderData;
    /// <summary>
    /// 该碰撞体的配置数据
    /// </summary>
    public GameColliderData2D ColliderData => _colliderData;

    /// <summary>
    /// 这个碰撞体绑定的游戏体transform信息
    /// </summary>
    private Transform _bindTransform;

    /// <summary>
    /// 用于描述这个碰撞体的位置信息
    /// X和Y信息以及旋转的信息
    /// </summary>
    public RectangleColliderVector3 PosVector3
    {
        get
        {
            if (_bindTransform == null)
            {
                Debug.LogError("The collider bind transform should not be null");
                return new RectangleColliderVector3();
            }

            // 计算距离
            float disToTgtPoint = _colliderData.offset.magnitude;
            // 计算偏转角度
            //var initOffsetAngle = Vector2.Angle(Vector2.up,_colliderData.offset);
            // [-180 180]
            var initOffsetAngle = Mathf.Atan2(_colliderData.offset.x,_colliderData.offset.y)* Mathf.Rad2Deg;

            // 结合当前对象的旋转角度计算实际的偏移x和y
            var tranaformRoateAngle = _bindTransform.eulerAngles.y <= 180? _bindTransform.eulerAngles.y:_bindTransform.eulerAngles.y-360;
            var realAngle = initOffsetAngle + tranaformRoateAngle;
            var colliderPosX = _bindTransform.position.x + Mathf.Sin(realAngle*Mathf.Deg2Rad) * disToTgtPoint;
            var colliderPosY = _bindTransform.position.z + Mathf.Cos(realAngle * Mathf.Deg2Rad) * disToTgtPoint;

            return new RectangleColliderVector3(colliderPosX,colliderPosY,tranaformRoateAngle);
        }
    }

    
    protected GameCollider2D(GameColliderData2D colliderData,Transform tgtTransform)
    {
        this._colliderData = colliderData;
        this._bindTransform = tgtTransform;
    }

    /// <summary>
    /// 判断与目标碰撞体是否产生了相交
    /// 两个带旋转的矩形检测相交
    /// </summary>
    /// <param name="tgtColliderPos"></param>
    /// <param name="tgtSize"></param>
    /// <returns></returns>
    public bool CheckCollapse(RectangleColliderVector3 tgtColliderPos, Vector2 tgtSize)
    {
        var crtColliderLines = PosVector3.GetRectangleLines(_colliderData.size);
        var tgtColliderLines = tgtColliderPos.GetRectangleLines(tgtSize);

        // 查找是否存在线条交差的情况
        for (int i=0;i<4; i++)
        {
            for (int k=0;k<4;k++)
            {
                // ab crtline ; cd tgtline
                var crtLineVectorAB = new Vector2(crtColliderLines[i,1].x - crtColliderLines[i, 0].x, crtColliderLines[i, 1].y - crtColliderLines[i, 0].y);
                var lineVectorAC = new Vector2(tgtColliderLines[k,0].x - crtColliderLines[i,0].x, tgtColliderLines[k, 0].y - crtColliderLines[i, 0].y);
                var lineVectorAD = new Vector2(tgtColliderLines[k, 1].x - crtColliderLines[i, 0].x, tgtColliderLines[k, 1].y - crtColliderLines[i, 0].y);

                var crossMultiAC2AB = lineVectorAC.x * crtLineVectorAB.y - lineVectorAC.y * crtLineVectorAB.x;
                var crossMultiAD2AB = lineVectorAD.x * crtLineVectorAB.y - lineVectorAD.y * crtLineVectorAB.x;
                // crossAC2AB * crossAD2AB <0 说明cd 在ab的两侧，同理判断ab是否在cd两侧
                var tgtLineVectorCD = new Vector2(tgtColliderLines[k, 1].x - tgtColliderLines[k, 0].x, tgtColliderLines[k, 1].y - tgtColliderLines[k, 0].y);
                var lineVectorCA = new Vector2(crtColliderLines[i, 0].x - tgtColliderLines[k, 0].x, crtColliderLines[i, 0].y - tgtColliderLines[k, 0].y);
                var lineVectorCB = new Vector2(crtColliderLines[i, 1].x - tgtColliderLines[k, 0].x, crtColliderLines[i, 1].y - tgtColliderLines[k, 0].y);
                var crossMultiCA2CD = lineVectorCA.x * tgtLineVectorCD.y - lineVectorCA.y * tgtLineVectorCD.x;
                var crossMultiCB2CD = lineVectorCB.x * tgtLineVectorCD.y - lineVectorCB.y * tgtLineVectorCD.x;

                if (crossMultiAC2AB*crossMultiAD2AB <0 && crossMultiCA2CD*crossMultiCB2CD<0)
                {
                    // ab 和 cd 两条线相交
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 检测与无旋转矩形区域的碰撞
    /// </summary>
    /// <param name="area"></param>
    /// <returns></returns>
    public bool CheckCollapse(Rect area)
    {
        Vector2 size = new Vector2(area.width, area.height);
        return CheckCollapse(new RectangleColliderVector3(area.x,area.y,0) , size);
    }

    /// <summary>
    /// 检测和其它碰撞体的碰撞
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool CheckCollapse(GameCollider2D other)
    {
        return CheckCollapse(other.PosVector3,other.ColliderData.size);
    }

    /// <summary>
    /// 判断某个位置点是否在此碰撞区域内
    /// 不包含贴边的值，如果修改贴边则改成>=0
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckPosInCollider(Vector2 pos)
    {
        var rectangleVertexs = PosVector3.GetRectangleVertexs(_colliderData.size);
        var vectorAE = new Vector2(pos.x - rectangleVertexs[0].x, pos.y - rectangleVertexs[0].y);
        var vectorBE = new Vector2(pos.x - rectangleVertexs[1].x, pos.y - rectangleVertexs[1].y);
        var vectorCE = new Vector2(pos.x - rectangleVertexs[2].x, pos.y - rectangleVertexs[2].y);
        var vectorDE = new Vector2(pos.x - rectangleVertexs[3].x, pos.y - rectangleVertexs[3].y);
        var crossMultiAE2BE = vectorAE.x * vectorBE.y - vectorAE.y * vectorBE.x;
        var crossMultiBE2CE = vectorBE.x * vectorCE.y - vectorBE.y * vectorCE.x;
        var crossMultiCE2DE = vectorCE.x * vectorDE.y - vectorCE.y * vectorDE.x;
        var crossMultiDE2AE = vectorDE.x * vectorAE.y - vectorDE.y * vectorAE.x;

        if (crossMultiAE2BE > 0 && crossMultiBE2CE  >0 && crossMultiCE2DE > 0 && crossMultiDE2AE>0) return true;
        return false;
    }

    
    public GameColliderType GetColliderType()
    {
        return _colliderData.colliderType;
    }

    public abstract void OnColliderEnter(IGameCollider other);
}
