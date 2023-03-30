using System;

[Serializable]
public class AudioMeterData
{
    public string audioName;
    public float audioLen;
    public float[] baseMeters;
    public float[] attackMeters;
}
