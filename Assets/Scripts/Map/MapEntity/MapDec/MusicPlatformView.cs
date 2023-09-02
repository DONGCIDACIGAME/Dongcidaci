using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicPlatformView : MapDecView, IMeterHandler
{
    private float _initialPosY;
    private float _moveRange;

    private void Start()
    {
        _initialPosY = transform.position.y;
        _moveRange = Random.Range(0.2f,0.6f);
    }


    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        float crtMeterTime = MeterManager.Ins.GetTotalMeterTime(meterIndex, meterIndex + 1);
        MoveUpDownByMeters(crtMeterTime);
    }


    private void MoveUpDownByMeters(float meterTime)
    {
        if (transform.position.y > _initialPosY)
        {
            transform.DOMoveY(_initialPosY- _moveRange, meterTime);

        }else if (transform.position.y < _initialPosY)
        {
            transform.DOMoveY(_initialPosY + _moveRange, meterTime);
        }
        else
        {
            transform.DOMoveY(_initialPosY + _moveRange, meterTime);
        }
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
