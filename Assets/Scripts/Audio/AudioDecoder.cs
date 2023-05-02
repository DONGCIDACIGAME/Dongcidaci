using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AudioDecoder : MonoBehaviour
{
    public AudioSource AS;
    float[] spectrum = new float[256];
    float[] outputs = new float[1024];

    [ContextMenu("DecodeClip")]
    private void Decode()
    {
        AS.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        AS.GetOutputData(outputs, 0);
    }


    private void Update()
    {
        for (int i = 1; i < spectrum.Length - 1; i++)
        {
            Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
        }


        
        for (int i = 1; i < outputs.Length - 1; i++)
        {
            Debug.DrawLine(new Vector3(i - 1, outputs[i] + 10, 0), new Vector3(i, outputs[i + 1] + 10, 0), Color.yellow);
        }
    }
}
