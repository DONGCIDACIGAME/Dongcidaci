using System.Collections.Generic;
using GameEngine;
using TMPro;


public class PanelDemo : UIPanel, IMeterHandler
{
    private TMP_Text Text_MeterShow;

    public override string GetPanelLayerPath()
    {
        return UIPathDef.UI_LAYER_NORMAL_STATIC;
    }

    public void OnMeterEnd(int meterIndex)
    {

    }

    public void OnMeterEnter(int meterIndex)
    {
        Text_MeterShow.text = meterIndex.ToString();
    }

    protected override void BindUINodes()
    {
        Text_MeterShow = BindTextNode("Text_MeterShow");
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        MeterManager.Ins.RegisterMeterHandler(this);
    }
}
