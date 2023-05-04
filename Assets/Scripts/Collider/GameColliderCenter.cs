using System.Collections;
using System.Collections.Generic;
using GameEngine;

/// <summary>
/// TODO: ����Ҫ�Ż�һ�£����ݵ�ͼ���ֿ飬������ʱ��ֻ���ĳ�������ڵ�
/// ��������Ҫ��һ��
/// ��������Ӧ���Ǹ���λ�úͰ�Χ�д�С
/// </summary>
public class GameColliderCenter : ModuleManager<GameColliderCenter>
{
    private HashSet<GameCollider2D>[,] mAllGameColliders;
    private Dictionary<int, List<int>> mAllCollidersRecord;

    public override void Initialize()
    {
        mAllCollidersRecord = new Dictionary<int, List<int>>();
    }

    public void ResetWithMapInfo(int mapWidth, int mapHeight, int cellSize)
    {
        int colNum = (mapWidth % cellSize) == 0 ? (mapWidth / cellSize) : (mapWidth / cellSize) + 1;
        int rowNum = (mapHeight % cellSize) == 0 ? (mapHeight / cellSize) : (mapHeight / cellSize) + 1;

        mAllGameColliders = new HashSet<GameCollider2D>[colNum, rowNum];
        mAllCollidersRecord.Clear();
    }

    public void RegisterGameCollider(GameCollider2D collider)
    {

    }

    public void UnRegisterGameCollider(GameCollider2D collider)
    {
        
    }
    
    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);


    }

    public override void Dispose()
    {
        mAllGameColliders = null;
    }


}
