
using System.Collections;
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
    private GameObject Image_AttackArrow;
    private Button Btn_Quit;
    private GameObject Node_ChargingStatus;
    private Image Image_Charging;

    private float attack_arrow_min_radius = 150f;
    private float attack_arrow_max_radius = 400f;

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
        Image_AttackArrow = BindNode("Image_AttackArrow");
        Node_ChargingStatus = BindNode("Node_ChargingStatus");
        Image_Charging = BindImageNode("Node_ChargingStatus/Image_Charging");
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

        mEventListener.Listen<float>("HeroStartCharging", OnHeroStartCharging);
        mEventListener.Listen("HeroEndCharging", OnHeroEndCharging);
    }

    private float timer;
    private GameCoroutine mChargingImageCoroutine;

    private void OnHeroStartCharging(float fullChargingTime)
    {
        if (mChargingImageCoroutine != null)
            return;

        Node_ChargingStatus.SetActive(true);
        timer = 0;
        mChargingImageCoroutine = CoroutineManager.Ins.StartCoroutine(ChargingImageGrow(fullChargingTime));
    }

    private void OnHeroEndCharging()
    {
        Node_ChargingStatus.SetActive(false);
        Image_Charging.fillAmount = 0;
        CoroutineManager.Ins.StopCoroutine(mChargingImageCoroutine);
        mChargingImageCoroutine = null;
    }

    private IEnumerator ChargingImageGrow(float fullTime)
    {
        if(fullTime <= 0)
        {
            CoroutineManager.Ins.StopCoroutine(mChargingImageCoroutine);
            mChargingImageCoroutine = null;
            yield break;
        }


        while(timer <= fullTime)
        {
            timer += TimeMgr.Ins.DeltaTime;
            float fill = timer / fullTime;
            Image_Charging.fillAmount = fill;
            yield return null;
        }

        CoroutineManager.Ins.StopCoroutine(mChargingImageCoroutine);
        mChargingImageCoroutine = null;
    }

    private Vector2 GetArrowPos(Vector2 mouseLocalPos)
    {
        float radius = mouseLocalPos.magnitude;
        Vector2 pos = mouseLocalPos;

        if (radius < attack_arrow_min_radius)
        {
            pos = mouseLocalPos.normalized * attack_arrow_min_radius;
        }
        else if(radius > attack_arrow_max_radius)
        {
            pos = mouseLocalPos.normalized * attack_arrow_max_radius;
        }

        return pos;
    }


    private void OnChangeAttackTowards(Vector3 mousePos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Node_AttackTowards.transform as RectTransform, mousePos,
            CameraManager.Ins.GetUICam(), out Vector2 mouseLocalPos);


        Vector2 localPos = GetArrowPos(mouseLocalPos);
        Image_AttackArrow.transform.localPosition = localPos;

        Image_AttackArrow.transform.localRotation = Quaternion.FromToRotation(Node_AttackTowards.transform.right, localPos.normalized);
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
