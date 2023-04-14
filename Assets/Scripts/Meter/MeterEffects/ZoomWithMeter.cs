using UnityEngine;

public class ZoomWithMeter : WithMeter
{
    public Vector3 zoomFrom;
    public Vector3 zoomTo;
    public bool OddTriggered;
    public bool EvenTriggered;
    private bool beginZoom;
    public float zoomDuration { get; private set; }
    private float timeRecord;
    public AnimationCurve Curve;
 

    public override void OnUpdate(float deltaTime)
    {
        if (!beginZoom)
            return;
        float progress = Curve.Evaluate((timeRecord % zoomDuration)/zoomDuration);
        Vector3 scale = Vector3.Lerp(zoomFrom, zoomTo, progress);
        this.transform.localScale = scale;
        timeRecord += deltaTime;
        if (timeRecord > zoomDuration)
            beginZoom = false;
    }

    public override void OnMeter(int meterIndex)
    {
        if (meterIndex % 2 == 0 && !EvenTriggered)
            return;

        if (meterIndex % 2 == 1 && !OddTriggered)
            return;
        beginZoom = true;
        zoomDuration = MeterManager.Ins.GetCurrentMeterTime();
        this.transform.localScale = zoomFrom;
        timeRecord = 0;
    }
}
