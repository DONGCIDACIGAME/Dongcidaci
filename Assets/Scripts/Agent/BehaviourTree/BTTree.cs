using GameEngine;

public class BTTree : IGameUpdate
{
    // 行为树的根节点
    protected BTNode mEntryNode;

    public void OnUpdate(float deltaTime)
    {
        mEntryNode.Excute();
    }
}