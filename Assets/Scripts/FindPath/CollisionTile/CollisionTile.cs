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
public class CollisionTile 
{
    
    public CollisionTile() {}
	~CollisionTile(){}
    //!得到某一个点所在Tile的高度
	public bool GetLocation(float  x,float  z,ref float absX,ref float absY,ref float absZ,ref int lengthIndex,ref int widthIndex)
    {
        return true;
    }

    public int GetHeight()
    {
        return m_tileFileHeader.m_countZ;
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
