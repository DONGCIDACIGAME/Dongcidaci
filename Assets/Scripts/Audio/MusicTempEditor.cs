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
    public TextMeshPro moveText;
    public TextMeshPro attackText;
    

    private bool recordMeter;
    private bool recordBaseMeter;
    private bool startPlayback;

    private float musicLen;

    public List<float> meterRecords = new List<float>();
    public List<float> basemeterRecords = new List<float>();

    private void Awake()
    {
        LoadMeter();
        LoadBaseMeter();
        audioSource.clip = music;
        musicLen = music.length;
    }

    private void Reset()
    {
        startPlayback = false;
        recordMeter = false;
        recordBaseMeter = false;
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

        if (!recordBaseMeter && Input.GetKeyDown(KeyCode.B))
        {
            recordBaseMeter = true;
            audioSource.PlayDelayed(3);
            audioSource.loop = false;
            basemeterRecords.Clear();
            return;
        }

        if(audioSource.isPlaying)
        {
            RecordMeter();
            RecordBaseMeter();
        }

        if (audioSource.time >= musicLen)
        {
            SaveMeter();
            SaveBaseMeter();

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

    private void RecordBaseMeter()
    {
        if (!recordBaseMeter)
            return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            basemeterRecords.Add(audioSource.time);
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

    private void SaveBaseMeter()
    {
        StringBuilder str = new StringBuilder();
        for (int i = 0; i < basemeterRecords.Count; i++)
        {
            str.Append(basemeterRecords[i] + ",");
        }
        FileHelper.WriteStr(Path.Combine(Application.dataPath, music.name + ".basemeter"), str.ToString(), Encoding.UTF8);
    }

    private void LoadMeter()
    {
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

    private void LoadBaseMeter()
    {
        string path = Path.Combine(Application.dataPath, music.name + ".basemeter");

        if (!FileHelper.FileExist(path))
            return;

        string data = FileHelper.ReadText(path, Encoding.UTF8);

        string[] basemeters = data.Split(',', StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < basemeters.Length; i++)
        {
            basemeterRecords.Add(float.Parse(basemeters[i]));
        }
    }


    float playbackTime;
    private int moveIndex;
    private int attackIndex;
    private void PlayBack(float deltaTime)
    {
        if (!startPlayback)
            return;

        if (attackIndex < meterRecords.Count)
        {
            if (playbackTime >= meterRecords[attackIndex])
            {
                attackText.text = attackIndex.ToString();
                attackIndex++;
            }


        }

        if(moveIndex < basemeterRecords.Count)
        {
            if (playbackTime >= basemeterRecords[moveIndex])
            {
                moveText.text = moveIndex.ToString();
                moveIndex++;
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
