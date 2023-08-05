using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text;
using System;

public class PanelMusicEditor : UIPanel
{
    public override string GetPanelLayerPath()
    {
        return UIPathDef.UI_LAYER_NORMAL_DYNAMIC;
    }

    private Button Btn_File;
    private Button Btn_Exit;
    private GameObject SubMenuContainer;
    private AudioSource AudioSrc;

    private ControlSubMenus ctl_submenus;
    private Button Btn_Open;
    private TMP_Text Text_MeterRecord;


    public AudioClip music;
    private bool recordMeter;
    private bool startPlayback;
    private float musicLen;

    public List<float> meterRecords = new List<float>();


    protected override void BindUINodes()
    {
        Btn_File = BindButtonNode("Top/Top_Menu/Btn_File",OnBtnFileClicked);
        SubMenuContainer = BindNode("SubMenuContainer");
        ctl_submenus = UIManager.Ins.BindControl<ControlSubMenus>(this, SubMenuContainer);
        Btn_Open = BindButtonNode("SubMenuContainer/Node_Menus/Btn_Open", OnBtnOpenClick);
        Btn_Exit = BindButtonNode("Top/Btn_Exit", OnBtnExitClicked);
        AudioSrc = BindNode("AudioSource").GetComponent<AudioSource>();
        Text_MeterRecord = BindTextNode("Text_MeterRecord");
    }

    private void OnBtnFileClicked()
    {
        SubMenuContainer.SetActive(!SubMenuContainer.activeSelf);
        if (ctl_submenus != null)
        {
            ctl_submenus.AllignTo(this);
        }
    }

    protected override void BindEvents()
    {
        base.BindEvents();

        mEventListener.Listen("OnLoadMusic", (string musicPath) =>
        {
            string[] temp = musicPath.Split("/");
            string musicName = temp[temp.Length - 1].Replace(".mp3","");

            string musicAssetPath = "Audio/Music/" + musicName;

            music = ResourceMgr.Ins.Load<AudioClip>(musicAssetPath, "LoadMusic");
            AudioSrc.clip = music;
            recordMeter = false;
            //playbackTime = 0;
            musicLen = music.length;
        });
    }


    private void OnBtnOpenClick()
    {
        SubMenuContainer.SetActive(false);

        Dictionary<string, object> args = new Dictionary<string, object>()
        {
            { "root_dir", "Assets/AssetBundles/Audio/Music"},
            { "ext",".mp3"},
            { "loadEvent", "OnLoadMusic"}
        };

        UIManager.Ins.OpenPanel<PanelLoadFileHUD>("Prefabs/UI/Common/Panel_LoadFileHUD",args);
    }


    private void OnBtnExitClicked()
    {
        UIManager.Ins.ClosePanel<PanelMusicEditor>();

        GameSceneManager.Ins.LoadAndSwitchToScene(SceneDefine.SceneSelect);
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {

    }


    private void Reset()
    {
        startPlayback = false;
        recordMeter = false;
        AudioSrc.Stop();
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

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
            //playbackTime = 0;
            AudioSrc.Play();
            startPlayback = true;
            return;
        }

        if (!recordMeter && Input.GetKeyDown(KeyCode.R))
        {
            recordMeter = true;
            AudioSrc.PlayDelayed(3);
            AudioSrc.loop = false;
            meterRecords.Clear();
        }


        if (AudioSrc.isPlaying)
        {
            RecordMeter();
        }

        if (AudioSrc.time >= musicLen)
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
            meterRecords.Add(AudioSrc.time);
            Text_MeterRecord.text = meterRecords.Count.ToString();
        }
    }


    private void SaveMeter()
    {
        AudioMeterData meterData = new AudioMeterData();
        meterData.audioLen = music.length;
        meterData.audioName = music.name;
        meterData.rhythmType = 4;
        meterData.sceneBehaviourMeterTrigger = new int[] { 1, 1, 0, 0, 1, 0, 1, 0 };
        meterRecords.Insert(0, 0);
        meterRecords.Add(music.length);
        meterData.baseMeters = meterRecords.ToArray();
        string output = JsonUtility.ToJson(meterData);
        FileHelper.WriteStr(Path.Combine(Application.dataPath, music.name + ".meter"), output, Encoding.UTF8);
    }


    //float playbackTime;
    //private int meterIndex;
    private void PlayBack(float deltaTime)
    {
        if (!startPlayback)
            return;

        //if (meterIndex < meterRecords.Count)
        //{
        //    if (playbackTime >= meterRecords[meterIndex])
        //    {
        //        meterIndex++;
        //    }


        //}

        if (AudioSrc.time >= music.length)
        {
            startPlayback = false;
            AudioSrc.Stop();
        }

        //playbackTime += deltaTime;

    }
}
