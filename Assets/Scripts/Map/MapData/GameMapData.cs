[System.Serializable]
public class GameMapData
{
    /// <summary>
    /// ID
    /// </summary>
    public int mapUID;

    /// <summary>
    /// 地图名称
    /// </summary>
    public string mapName;

    /// <summary>
    /// 地图宽度
    /// </summary>
    public int mapWidth;

    /// <summary>
    /// 地图高度
    /// </summary>
    public int mapHeight;

    /// <summary>
    /// 地图上所有的地板
    /// </summary>
    public MapGround[] mapGrounds;

    /// <summary>
    /// 地图上所有的墙
    /// </summary>
    //public MapWall[] mapWalls;


    
}
