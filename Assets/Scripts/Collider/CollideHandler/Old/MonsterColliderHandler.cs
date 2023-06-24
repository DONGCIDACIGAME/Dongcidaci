public class MonsterColliderHandler : IGameColliderHandler
{
    private Monster mMonster;

    public MonsterColliderHandler(Monster monster)
    {
        mMonster = monster;
    }

    public void Dispose()
    {
        mMonster = null;
    }

    public void HandleColliderToBlock(MapBlock block)
    {
        
    }

    public void HandleColliderToHero(Hero hero)
    {
        
    }

    public void HandleColliderToMonster(Monster monster)
    {
        
    }
}
