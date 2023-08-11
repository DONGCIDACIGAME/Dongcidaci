
using System.Collections.Generic;
using GameEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelDemo : UIPanel, IMeterHandler
{
    private TMP_Text Text_MeterShow;
    private ColorLoopOnce MeterHint;
    private GameObject Node_AttackTowards;
    private Button Btn_Quit;

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
        Node_AttackTowards = BindNode("Node_AttackTowards");
        Btn_Quit = BindButtonNode("Btn_Quit", OnBtnQuitClick);
    }

    private void OnBtnQuitClick()
    {
        GameSceneManager.Ins.LoadAndSwitchToScene(SceneDefine.SceneSelect);
    }

    protected override void BindEvents()
    {
        base.BindEvents();

        mEventListener.Listen<Vector3>("ChangeAttackTowards", OnChangeAttackTowards);
    }

    private void OnChangeAttackTowards(Vector3 towards)
    {
        Node_AttackTowards.transform.localRotation = Quaternion.FromToRotation(Vector3.right, towards);
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
