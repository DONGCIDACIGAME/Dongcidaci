using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRuneStoneView : MapDecView, IMeterHandler
{
    [SerializeField] private Animator _animator;
    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        float crtMeterTime = MeterManager.Ins.GetTotalMeterTime(meterIndex, meterIndex + 1);
        _animator.SetFloat("PlaySpeed", 0.5f / crtMeterTime);
        _animator.Play("MeterWave", 0, 0);

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
