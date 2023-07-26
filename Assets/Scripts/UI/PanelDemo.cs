
using System.Collections.Generic;
using GameEngine;
using TMPro;
using UnityEngine.UI;

public class PanelDemo : UIPanel, IMeterHandler
{
    private TMP_Text Text_MeterShow;
    private ColorLoopOnce MeterHint;

    public override string GetPanelLayerPath()
    {
        return UIPathDef.UI_LAYER_NORMAL_STATIC;
    }

    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        if(MeterHint != null)
        {
            MeterHint.LoopOnce();
        }
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
        Image MeterHintImg = BindImageNode("Image_MeterHint");
        if (MeterHintImg != null)
        {
            MeterHint = MeterHintImg.GetComponent<ColorLoopOnce>();
        }
    }

    protected override void OnClose()
    {
        MeterManager.Ins.UnregiseterMeterHandler(this);
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        MeterManager.Ins.RegisterMeterHandler(this);
    }
}
