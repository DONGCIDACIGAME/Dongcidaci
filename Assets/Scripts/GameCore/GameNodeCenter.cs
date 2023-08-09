using GameEngine;
using UnityEngine;

/// <summary>
/// 节点中心
/// </summary>
public class GameNodeCenter: Singleton<GameNodeCenter>
{
    #region Agent
    public GameObject HeroNode { get; private set; }
    public GameObject MonsterNode { get; private set; }

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

    #endregion


    #region Map

    public GameObject MapRootNode { get; private set; }
    public GameObject BaseLayerNode { get; private set; }
    public GameObject EventLayerNode { get; private set; }


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
    #endregion

    #region Camera
    public GameObject MainCamNode { get; private set; }
    public GameObject UICamNode { get; private set; }

    public void InitializeCameraNodes()
    {
        MainCamNode = GameObject.Find("_MAIN_CAMERA");
        UICamNode = GameObject.Find("_UI_CAMERA");
    }

    public void DisposeCameraNodes()
    {
        MainCamNode = null;
        UICamNode = null;
    }

    #endregion
}
