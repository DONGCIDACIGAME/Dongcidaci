using UnityEngine;

public static class TimeMgr
{
    public static float Now
    {
        get
        {
            return Time.realtimeSinceStartup;
        }
    }
}
