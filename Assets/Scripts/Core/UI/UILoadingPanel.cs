using GameEngine;
using System.Collections.Generic;
public class UILoadingPanel
{
    public UIPanel panel;
    public string resPath;
    public Dictionary<string, object> openArgs;

    public UILoadingPanel(UIPanel panel, string resPath, Dictionary<string,object> openArgs)
    {
        this.panel = panel;
        this.resPath = resPath;
        this.openArgs = openArgs;
    }
}
