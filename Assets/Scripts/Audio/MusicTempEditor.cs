using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System.IO;
using System;

public class MusicTempEditor : MonoBehaviour
{
    public AudioClip music;
    public AudioSource audioSource;
    public TextMeshPro meterRecord;
    

    private bool recordMeter;
    private bool startPlayback;

    private float musicLen;

    public List<float> meterRecords = new List<float>();

    private void Awake()
    {
        LoadMeter();
        audioSource.clip = music;
        musicLen = music.length;
    }

    private void Reset()
    {
        startPlayback = false;
        recordMeter = false;
        audioSource.Stop();
    }

    private void Update()
   {
        if (music == null)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Reset();
            return;
        }

        PlayBack(Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.P))
        {
            playbackTime = 0;
            audioSource.Play();
            startPlayback = true;
            return;
        }

        if (!recordMeter && Input.GetKeyDown(KeyCode.R))
        {
            recordMeter = true;
            audioSource.PlayDelayed(3);
            audioSource.loop = false;
            meterRecords.Clear();
        }


        if(audioSource.isPlaying)
        {
            RecordMeter();
        }

        if (audioSource.time >= musicLen)
        {
            SaveMeter();
            Reset();
        }
    }

    private void RecordMeter()
    {
        if (!recordMeter)
            return;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            meterRecords.Add(audioSource.time);
        }
    }


    private void SaveMeter()
    {
        StringBuilder str = new StringBuilder();
        for(int i =0;i<meterRecords.Count;i++)
        {
            str.Append(meterRecords[i] + ",");
        }
        FileHelper.WriteStr(Path.Combine(Application.dataPath, music.name + ".meter"), str.ToString(), Encoding.UTF8);
    }


    private void LoadMeter()
    {
        if (music == null)
            return;

        string path = Path.Combine(Application.dataPath, music.name + ".meter");

        if (!FileHelper.FileExist(path))
            return;

        string data = FileHelper.ReadText(path, Encoding.UTF8);

        string[] meters = data.Split(',', StringSplitOptions.RemoveEmptyEntries);

        for(int i = 0;i<meters.Length;i++)
        {
            meterRecords.Add(float.Parse(meters[i]));
        }
    }


    float playbackTime;
    private int meterIndex;
    private void PlayBack(float deltaTime)
    {
        if (!startPlayback)
            return;

        if (meterIndex < meterRecords.Count)
        {
            if (playbackTime >= meterRecords[meterIndex])
            {
                meterRecord.text = meterIndex.ToString();
                meterIndex++;
            }


        }

        if (audioSource.time >= music.length)
        {
            startPlayback = false;
            audioSource.Stop();
        }

        playbackTime += deltaTime;

    }
}
