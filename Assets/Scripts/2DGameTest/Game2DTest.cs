using GameEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Game2DTest : MonoBehaviour,IMeterHandler
{
    private MeterManager meterMgr;
    private AudioManager audioMgr;

    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        BounceSprite();
    }

    public void OnMeterEnd(int meterIndex)
    {
        return;
    }

    public void OnMeterEnter(int meterIndex)
    {
        return;
    }


    // Start is called before the first frame update
    void Start()
    {
        DataCenter.Ins.Initialize();
        ResourceMgr.Ins.Initialize();
        //TimeMgr.Ins.Initialize();
        //UpdateCenter.Ins.Initialize();

        meterMgr = MeterManager.Ins;
        meterMgr.Initialize();
        audioMgr = AudioManager.Ins;
        audioMgr.Initialize();
        string musicName = "bass-gone-walking_preview-150bpm";
        AudioManager.Ins.LoadBgm("Audio/Music/" + musicName);
        AudioManager.Ins.PlayBgm(true);

        meterMgr.RegisterMeterHandler(this);
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;

        meterMgr.OnGameUpdate(deltaTime);
    }

    public void BounceSprite()
    {
        Sequence bounceAnim = DOTween.Sequence();
        bounceAnim.Insert(0, transform.DOScaleY(0.6f, 0.1f));
        bounceAnim.Insert(0.1f, transform.DOScaleY(0.5f,0.05f));
        
    }



}
