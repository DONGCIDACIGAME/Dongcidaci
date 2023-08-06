using GameEngine;
using UnityEngine;

/// <summary>
/// 节点中心
/// </summary>
public class GameNodeCenter: Singleton<GameNodeCenter>
{
    public GameObject HeroNode;
    public GameObject MonsterNode;

    public void InitliazeAgentNodes()
    {
        HeroNode = GameObject.Find("_AGENT/_HERO");
        MonsterNode = GameObject.Find("_AGENT/_MONSTER");
    }

    public void DisposeAgentNodes()
    {
        HeroNode = null;
        MonsterNode = null;
    }



    public GameObject MapRootNode;
    public GameObject BaseLayerNode;
    public GameObject EventLayerNode;


    public void InitliazeMapNodes()
    {
        MapRootNode = GameObject.Find(GameDefine._MAP_ROOT);
        BaseLayerNode = GameObject.Find("_SCENE/_MAP/_BASE_LAYER");
        EventLayerNode = GameObject.Find("_SCENE/_MAP/_EVENT_LAYER");
    }

    public void DisposeMapNodes()
    {
        MapRootNode = null;
        BaseLayerNode = null;
        EventLayerNode = null;
    }
}
