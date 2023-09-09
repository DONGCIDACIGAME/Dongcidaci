using System.Collections.Generic;

public enum MapRoomTypeDefine
{
    /// <summary>
    /// 老家基地
    /// </summary>
    Home = 0,
    /// <summary>
    /// 野外商店
    /// </summary>
    Merchant = 1,
    /// <summary>
    /// 特殊NPC
    /// </summary>
    SpecialNPC = 2,
    /// <summary>
    /// 祝福房间
    /// </summary>
    BlessingRoom = 3,

    /// <summary>
    /// 常规的怪物房
    /// </summary>
    MonsterRoom = 10,
    /// <summary>
    /// 精英房间
    /// </summary>
    EliteMonsterRoom = 11,
    /// <summary>
    /// Boss房间
    /// </summary>
    BossRoom = 12,

    /// <summary>
    /// 金币房间
    /// </summary>
    CoinRoom = 20,
    /// <summary>
    /// 道具房间
    /// </summary>
    PropRoom = 21,
    /// <summary>
    /// 技能房间
    /// </summary>
    SkillRoom = 22,
    /// <summary>
    /// 宝箱房间
    /// </summary>
    ChestRoom = 23,
    /// <summary>
    /// 节奏之混房间,节奏之混是强化类的资源，可以强化道具和技能
    /// </summary>
    BeatSoulRoom = 24,

    /// <summary>
    /// 未知房间
    /// </summary>
    UnknownRoom = 30,

}

[System.Serializable]
public class NaviGridCell
{
    public int xIndex;
    public int yIndex;

    public float sizeX;
    public float sizeY;
    
    public float anchorPosX;
    public float anchorPosY;
    public float anchorPosZ;

    /// <summary>
    /// 该导航网格的最大通过半径
    /// </summary>
    public float maxPassRadius;

    /// <summary>
    /// 该导航网格的优先级
    /// 如果网格上存在负面事件，那么该优先级会更低
    /// </summary>
    public int priority;

    /// <summary>
    /// 是否被事件障碍物阻挡着
    /// </summary>
    public bool isEventBlocked;
}




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
    /// 这个地图房间的定义
    /// </summary>
    public MapRoomTypeDefine roomType = MapRoomTypeDefine.MonsterRoom;

    /// <summary>
    /// 这个房间的标识图标
    /// </summary>
    public string roomIconName;

    /// <summary>
    /// 地图列数
    /// </summary>
    public int mapColCount;

    /// <summary>
    /// 地图行数
    /// </summary>
    public int mapRowCount;

    /// <summary>
    /// 地图单个网格的宽度
    /// </summary>
    public float mapCellWidth;

    /// <summary>
    /// 地图单个网格的高度
    /// </summary>
    public float mapCellHeight;

    /// <summary>
    /// 地图基本地形的预制体名
    /// </summary>
    public string mapBasePrefabName;

    /// <summary>
    /// 地图的事件数据
    /// </summary>
    public List<MapEventData> mapEventDatas;

    /// <summary>
    /// 地图的所有导航数据
    /// </summary>
    public List<NaviGridCell> naviCells;

}
