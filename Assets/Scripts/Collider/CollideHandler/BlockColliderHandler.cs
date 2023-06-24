public class BlockColliderHandler : IGameColliderHandler
{
    private MapBlock mBlock;

    public BlockColliderHandler(MapBlock block)
    {
        mBlock = block;
    }

    public void Dispose()
    {
        mBlock = null;
    }

    public void HandleColliderToBlock(MapBlock block)
    {
       
    }

    public void HandleColliderToHero(Hero hero)
    {
        Log.Logic(LogLevel.Info, "<color=green>HandleColliderToHero------enter hero-{0}</color>", hero.GetName());
    }

    public void HandleColliderToMonster(Monster monster)
    {
        
    }
}
