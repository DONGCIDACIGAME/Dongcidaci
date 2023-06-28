using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConvex2DShape
{
    public Vector3 AnchorPos { get; set; }
    public float AnchorAngle { get; set; }
    public Vector2 Offset { get; set; }
    public Vector2 Size { get; set; }
    public Vector2[] GetVertexs();

    //public Vector2[,] GetEdges();

    //public Vector2[] GetEdgeNormals();

}

public struct Triangle2DShape : IConvex2DShape
{
    private Vector3 _anchorPos;
    private float _anchorAngle;
    private Vector2 _offset;
    private Vector2 _size;

    public Triangle2DShape(Vector3 anchorPos, float anchorAngle, Vector2 offset, Vector2 size)
    {
        this._anchorPos = anchorPos;
        this._anchorAngle = anchorAngle;
        this._offset = offset;
        this._size = size;
    }

    public Vector3 AnchorPos { get => _anchorPos; set => _anchorPos = value; }
    public float AnchorAngle { get => _anchorAngle; set => _anchorAngle = value; }
    public Vector2 Offset { get => _offset; set => _offset = value; }
    public Vector2 Size { get => _size; set => _size = value; }

    public Vector2[] GetVertexs()
    {
        return GameColliderHelper.GetTriangleVertexs(_anchorPos, _anchorAngle, _offset, _size);
    }

}


public struct Rect2DShape : IConvex2DShape
{
    private Vector3 _anchorPos;
    private float _anchorAngle;
    private Vector2 _offset;
    private Vector2 _size;

    public Rect2DShape(Vector3 anchorPos, float anchorAngle, Vector2 offset, Vector2 size)
    {
        this._anchorPos = anchorPos;
        this._anchorAngle = anchorAngle;
        this._offset = offset;
        this._size = size;
    }

    public Vector3 AnchorPos { get => _anchorPos; set => _anchorPos = value; }
    public float AnchorAngle { get => _anchorAngle; set => _anchorAngle = value; }
    public Vector2 Offset { get => _offset; set => _offset = value; }
    public Vector2 Size { get => _size; set => _size = value; }

    public Vector2[] GetVertexs()
    {
        return GameColliderHelper.GetRectVertexs(_anchorPos, _anchorAngle, _offset, _size);
    }


}


public struct Ellipse2DShape : IConvex2DShape
{
    private Vector3 _anchorPos;
    private float _anchorAngle;
    private Vector2 _offset;
    private Vector2 _size;
    private const int halfPolyCount = 5;

    public Ellipse2DShape(Vector3 anchorPos, float anchorAngle, Vector2 offset, Vector2 size)
    {
        this._anchorPos = anchorPos;
        this._anchorAngle = anchorAngle;
        this._offset = offset;
        this._size = size;
    }

    public Vector3 AnchorPos { get => _anchorPos; set => _anchorPos = value; }
    public float AnchorAngle { get => _anchorAngle; set => _anchorAngle = value; }
    public Vector2 Offset { get => _offset; set => _offset = value; }
    public Vector2 Size { get => _size; set => _size = value; }

    public Vector2[] GetVertexs()
    {
        return GameColliderHelper.GetEllipseVertexs(_anchorPos, _anchorAngle, _offset, _size, halfPolyCount);
    }

}

public struct Circle2DShape : IConvex2DShape
{
    private Vector3 _anchorPos;
    private float _anchorAngle;
    private Vector2 _offset;
    private float _diameter;
    private const int polyCount = 8;

    public Circle2DShape(Vector3 anchorPos, float anchorAngle, Vector2 offset, float diameter)
    {
        this._anchorPos = anchorPos;
        this._anchorAngle = anchorAngle;
        this._offset = offset;
        this._diameter = diameter;
    }

    public Vector3 AnchorPos { get => _anchorPos; set => _anchorPos = value; }
    public float AnchorAngle { get => _anchorAngle; set => _anchorAngle = value; }
    public Vector2 Offset { get => _offset; set => _offset = value; }
    public Vector2 Size { get => new Vector2(_diameter, _diameter); set => _diameter = value.x; }

    public Vector2[] GetVertexs()
    {
        return GameColliderHelper.GetCircleVertexs(_anchorPos, _anchorAngle, _offset, _diameter/2f, polyCount);
    }


}










