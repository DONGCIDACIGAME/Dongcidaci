using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicWaveTreeView : MapBlockView, IMeterHandler
{
    [SerializeField] private Animator _treeAnimator;

    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        float crtMeterTime = MeterManager.Ins.GetTotalMeterTime(meterIndex,meterIndex+1);
        _treeAnimator.SetFloat("PlaySpeed", 0.483f / crtMeterTime);
        _treeAnimator.Play("MeterWave",0,0);
    }

    public void OnMeterEnd(int meterIndex)
    {
        return;
    }

    public void OnMeterEnter(int meterIndex)
    {
        return;
    }
}
