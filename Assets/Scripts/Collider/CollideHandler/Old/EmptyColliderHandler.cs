/// <summary>
/// 一个空的碰撞处理器，不对碰撞对象做任何处理
/// 对于移动等行为可能有用
/// 对于不做处理的碰撞行为，保证传入处理器时非空
/// </summary>
public class EmptyColliderHandler : IGameColliderHandler
{
    public void Dispose()
    {
        
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
