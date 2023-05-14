using UnityEngine;
using GameEngine;
using System.Collections;

public class AudioManager : ModuleManager<AudioManager>
{
    private AudioSource BGMPlayer;
    private AudioSource GameEffectPlayer;
    private AudioSource AttackEffectPlayer;

    private AudioClip mCurBgm;

    public override void Initialize()
    {
        GameObject bgmNode = GameObject.Find(AudioDef.BgmNodePath);
        if (bgmNode != null)
            BGMPlayer = bgmNode.GetComponent<AudioSource>();

        GameObject gameEffectNode = GameObject.Find(AudioDef.GameEffectNodePath);
        if (gameEffectNode != null)
            GameEffectPlayer = gameEffectNode.GetComponent<AudioSource>();

        GameObject attackEffectNode = GameObject.Find(AudioDef.AttackEffectNodePath);
        if (attackEffectNode != null)
            AttackEffectPlayer = attackEffectNode.GetComponent<AudioSource>();

        if (BGMPlayer == null)
            Log.Error(LogLevel.Critical, "AudioManager BGM Player Initialize Failed!");

        if (GameEffectPlayer == null)
            Log.Error(LogLevel.Critical, "AudioManager GameEffectPlayer Player Initialize Failed!");

        if (AttackEffectPlayer == null)
            Log.Error(LogLevel.Critical, "AudioManager AttackEffectPlayer Player Initialize Failed!");
    }

    public override void Dispose()
    {
        BGMPlayer = null;
        GameEffectPlayer = null;
        AttackEffectPlayer = null;
    }

    public void LoadBgm(string audioPath)
    {
        if (string.IsNullOrEmpty(audioPath))
        {
            Log.Error(LogLevel.Normal, "LoadBgm Failed, audio path is null or empty!");
            return;
        }

        string audioName = audioPath.Split('.')[0];

        if(mCurBgm != null && mCurBgm.name.Equals(audioName))
        {
            return;
        }

        mCurBgm = ResourceMgr.Ins.LoadRes<AudioClip>(audioPath, true, "Load BGM");
    }

    public void PlayBgm(bool loop)
    {
        if (mCurBgm == null)
            return;

        if (BGMPlayer == null)
            return;

        if (mCurBgm != null)
        {
            MeterManager.Ins.Start(mCurBgm.name);
            BGMPlayer.Stop();
            BGMPlayer.clip = mCurBgm;
            BGMPlayer.loop = loop;
            BGMPlayer.Play();
        }
    }

    public float GetCurBgmTime()
    {
        return BGMPlayer.time;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

}
