using UnityEngine;
using TMPro;

public class FPSHUD : MonoBehaviour
{
    TextMeshPro tmp;

    // 1sË¢ÐÂÒ»´Î
    private float UpdateFreq = 1.0f;
    private int frameCounter = 0;
    private float nextUpdateTime = 0;

    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
        nextUpdateTime = Time.realtimeSinceStartup + UpdateFreq;
    }

    void Update()
    {
        frameCounter++;
        if (Time.realtimeSinceStartup > nextUpdateTime)
        {
            nextUpdateTime += UpdateFreq;
            tmp.text = "FPS:" + frameCounter;
            frameCounter = 0;
        }
    }
}
