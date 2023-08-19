using UnityEngine;
using UnityEngine.UI;

public class ColorLoopOnce : MyMonoBehaviour
{
    public Color startColor;
    public Color endColor;
    public Graphic target;

    private bool mRunning = false;
    private float mTimeRecord;

    private void Start()
    {
        if (target != null)
            target.color = startColor;
    }


    public void LoopOnce()
    {
        mTimeRecord = 0;
        mRunning = true;
    }


    public override void OnGameUpdate(float deltaTime)
    {
        base.OnGameUpdate(deltaTime);

        if (target == null)
            return;

        if (!mRunning)
            return;

        mTimeRecord += deltaTime;

        if (mTimeRecord < GamePlayDefine.DisplayTimeToMatchMeter)
        {
            target.color = Color.Lerp(startColor, endColor, mTimeRecord / GamePlayDefine.DisplayTimeToMatchMeter);
        }
        else if (mTimeRecord >= GamePlayDefine.DisplayTimeToMatchMeter && mTimeRecord < 2 * GamePlayDefine.DisplayTimeToMatchMeter)
        {
            target.color = Color.Lerp(endColor, startColor, (mTimeRecord - GamePlayDefine.DisplayTimeToMatchMeter) / GamePlayDefine.DisplayTimeToMatchMeter);
        }
        else
        {
            target.color = startColor;
            mRunning = false;
        }

    }

}
