using UnityEngine;
using GameEngine;

public class AutoSwitchScene : MonoBehaviour
{
    public string SwitchTo;

    private void Start()
    {
        GameSceneManager.Ins.LoadAndSwitchToScene(SwitchTo);
    }
}
