using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFileHeader
{
    //!文件版本号
	public int m_version;
	//!地表宽度,对应X
	public int		  m_countX;
	//!地表长度,对于Z
	public int		  m_countZ;
	//!地表每个Tile真实大小
	public float 		  m_tileSize;
	//!地表Tile起始坐标X
	public float		  m_startX;
	//!地表Tile起始坐标Z
	public float		  m_startZ;

}
struct Tile {
    public float m_height;
}
public class CollisionTile 
{
    //!碰撞格子结果
	List<Tile> m_collisionResult;

    public bool LoadMap() {
        return false;
    }
    public bool Reload() {
        return false;
    }
    public CollisionTile() {}
	~CollisionTile(){}
    //!得到某一个点所在Tile的高度
	public bool GetLocation(float  x,float  z,ref float absX,ref float absY,ref float absZ,ref int lengthIndex,ref int widthIndex)
    {
        if ( z > m_tileFileHeader.m_startZ && x > m_tileFileHeader.m_startX)
        {
            //!array Index
            lengthIndex = (int)((z-m_tileFileHeader.m_startZ)/m_tileFileHeader.m_tileSize);
            widthIndex  = (int)((x-m_tileFileHeader.m_startX)/m_tileFileHeader.m_tileSize);

            if (lengthIndex >= 0 && lengthIndex < (m_collisionResult.Count))
            {
                int idx = lengthIndex * m_tileFileHeader.m_countX + widthIndex;
                absX = widthIndex + m_tileFileHeader.m_startX + 0.5f;
                absZ = lengthIndex + m_tileFileHeader.m_startZ + 0.5f;
                if (idx >= 0 && idx < (m_collisionResult.Count))
                {
                    absY = m_collisionResult[idx].m_height;
                    if (absY < -5000)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }
	
	    return false;
    }

    //!得到x方向上的数量
    public int GetWidth()
    {
        return m_tileFileHeader.m_countX;
    }

    //!得到z方向上的数量
    public int GetHeight()
    {
        return m_tileFileHeader.m_countZ;
    }

    //!获得x方向上的起点
    public float GetXStart()
    {
        return m_tileFileHeader.m_startX;
    }
    //!获得z方向上的起点
    public float GetZStart()
    {
        return m_tileFileHeader.m_startZ;
    }

    //!使用虚拟的三角形进行碰撞
public bool GetSmoothDisposeBymulate(float x,float z,ref float absY)
{   
	return true;

}
    //!碰撞文件头部
    public TileFileHeader m_tileFileHeader;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
