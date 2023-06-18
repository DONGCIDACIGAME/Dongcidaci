using UnityEngine;

public class MapGroundView : MapEntityView
{
    /// <summary>
    /// 这个地板占据的地图块索引信息
    /// </summary>
    [SerializeField] private int[] _occupyMapIndexs;
    public int[] OccupyMapIndexs => _occupyMapIndexs;


}
