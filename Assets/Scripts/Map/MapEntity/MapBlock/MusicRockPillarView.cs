using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;
using DG.Tweening;

public class MusicRockPillarView : MapBlockView, IMeterHandler
{
    [SerializeField] private Transform _lightRock1;
    [SerializeField] private Transform _lightRock2;


    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        int rotateIndex = Random.Range(0,2);
        if (rotateIndex == 0)
        {
            RotateByMeters(_lightRock1);
        }
        else
        {
            RotateByMeters(_lightRock2);
        }

    }

    private void RotateByMeters(Transform tgtT)
    {
        var crtRotateY = tgtT.localEulerAngles.y;
        var tgtRotateY = crtRotateY + 90 > 360 ? (crtRotateY - 270) : crtRotateY + 90;
        tgtT.transform.DOLocalRotate(new Vector3(0,tgtRotateY,0),0.1f);
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
