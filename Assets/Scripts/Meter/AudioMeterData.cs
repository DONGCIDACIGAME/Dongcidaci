using System;

[Serializable]
public class AudioMeterData
{
    public string audioName;
    public float audioLen;
    public int rhythmType;
    public int[] sceneBehaviourMeterTrigger;
    public float[] baseMeters;
}
