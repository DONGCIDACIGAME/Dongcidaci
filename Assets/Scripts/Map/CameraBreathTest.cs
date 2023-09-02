using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraBreathTest : MonoBehaviour,IMeterHandler
{

    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        float crtMeterTime = MeterManager.Ins.GetTotalMeterTime(meterIndex, meterIndex + 1);
        MoveUpDownByMeters(crtMeterTime);
    }


    private void MoveUpDownByMeters(float meterTime)
    {
        if (transform.position.y > 0)
        {
            transform.DOMoveY(-0.05f, meterTime);

        }
        else if (transform.position.y < 0)
        {
            transform.DOMoveY(0.05f, meterTime);
        }
        else
        {
            transform.DOMoveY(0.05f, meterTime);
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

    // Start is called before the first frame update
    void Start()
    {
        if(MeterManager.Ins != null)
        {
            MeterManager.Ins.RegisterMeterHandler(this);
        }
    }

   
}
