public class MapManager : MeterModuleManager<MapManager>
{
    /// <summary>
    /// 加载地表
    /// </summary>
    private void LoadGround()
    {

    }

    /// <summary>
    /// 加载墙体
    /// </summary>
    private void LoadWall()
    {

    }

    /// <summary>
    /// 加载装饰物
    /// </summary>
    private void LoadDec()
    {

    }


    public void LoadMap(string mapId)
    {


        LoadGround();
        LoadWall();
        LoadDec();
    }

    public override void Dispose()
    {
        
    }

    public override void Initialize()
    {
        
    }

    public override void OnMeterEnd(int meterIndex)
    {
        
    }

    public override void OnMeterEnter(int curMeterIndex)
    {
        
    }
}

