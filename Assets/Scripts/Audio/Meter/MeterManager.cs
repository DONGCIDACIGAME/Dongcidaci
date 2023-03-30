using System.Collections.Generic;
using GameEngine;
using System.Text;
using System;
using UnityEngine;

public class MeterManager : ModuleManager<MeterManager>
{
    // 当前音乐的节奏数据
    private AudioMeterData mCurAudioMeterData;
    
    // 当前所处的基础节奏index
    private int baseMeterIndex;
    // 当前所处的攻击节奏index
    private int attackMeterIndex;

    // 音乐开始到当前的时间（loop）
    private float timeRecord;

    // 节奏控制器是否启用
    private bool enable;

    private Dictionary<int, IMeterHandler> mBaseMeterHandlers;
    private Dictionary<int, IMeterHandler> mAttackMeterHandlers;

    public override void Initialize()
    {
        mBaseMeterHandlers = new Dictionary<int, IMeterHandler>();
        mAttackMeterHandlers = new Dictionary<int, IMeterHandler>();
    }

    public void RegisterBaseMeterHandler(IMeterHandler handler)
    {
        if (handler == null)
            return;

        int unicode = handler.GetHashCode();
        if (mBaseMeterHandlers.ContainsKey(unicode))
            return;

        mBaseMeterHandlers.Add(unicode, handler);
    }

    public void RegisterAttackMeterHandler(IMeterHandler handler)
    {
        if (handler == null)
            return;

        int unicode = handler.GetHashCode();
        if (mAttackMeterHandlers.ContainsKey(unicode))
            return;

        mAttackMeterHandlers.Add(unicode, handler);
    }

    private void LoadAudioMeterInfo(string audioName)
    {
        mCurAudioMeterData = null;

        string meterRootDir = string.Empty;

#if UNITY_EDITOR
        meterRootDir = PathDefine.EDITOR_DATA_DIR_PATH + "/Meter/";
#else
        meterRootDir = PathDefine.RELEASE_DATA_DIR_PATH + "/Meter/";
#endif

        string meterDataPath = meterRootDir + audioName + ".meter";
        if (FileHelper.FileExist(meterDataPath))
        {
            string jsonStr = FileHelper.ReadText(meterDataPath, Encoding.UTF8);
            mCurAudioMeterData = JsonUtility.FromJson<AudioMeterData>(jsonStr);
        }
    }


    public void Start(string audioName)
    {
        Reset();
        LoadAudioMeterInfo(audioName);
    }

    public void Reset()
    {
        timeRecord = 0;
        enable = true;
        baseMeterIndex = 0;
        attackMeterIndex = 0;
    }

    public void Stop()
    {
        enable = false;
    }

    public void Pause()
    {
        enable = false;
    }

    public void Resume()
    {
        enable = true;
    }

    public bool CheckMoveMeter()
    {
        if (mCurAudioMeterData == null)
            return false;

        if (timeRecord >= mCurAudioMeterData.baseMeters[baseMeterIndex] - 0.3f && timeRecord <= mCurAudioMeterData.baseMeters[baseMeterIndex] + 0.3f)
        {
            return true;
        }

        return false;
    }

    public bool CheckAttackMeter()
    {
        if (mCurAudioMeterData == null)
            return false;

        if (timeRecord >= mCurAudioMeterData.attackMeters[attackMeterIndex] - 0.3f && timeRecord <= mCurAudioMeterData.attackMeters[attackMeterIndex] + 0.3f)
        {
            return true;
        }

        return false;
    }

    private void TriggerMove()
    {
        foreach(IMeterHandler handler in mBaseMeterHandlers.Values)
        {
            handler.OnMeter();
        }
    }

    private void TriggerAttack()
    {
        foreach (IMeterHandler handler in mAttackMeterHandlers.Values)
        {
            handler.OnMeter();
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        if (!enable)
            return;

        if (mCurAudioMeterData == null)
            return;

        timeRecord += deltaTime;

        if (baseMeterIndex < mCurAudioMeterData.baseMeters.Length)
        {
            if (timeRecord >= mCurAudioMeterData.baseMeters[baseMeterIndex])
            {
                TriggerMove();
                // 这里自增一定要放在最后
                baseMeterIndex++;
            }
        }

        if (attackMeterIndex < mCurAudioMeterData.attackMeters.Length)
        {
            if (timeRecord >= mCurAudioMeterData.attackMeters[attackMeterIndex])
            {
                TriggerAttack();
                // 这里自增一定要放在最后
                attackMeterIndex++;
            }
        }

        if (timeRecord >= mCurAudioMeterData.audioLen)
        {
            Reset();
        }
    }

    public float GetTimeToNextBaseMeter(int offset)
    {
        if (mCurAudioMeterData == null)
            return -1f;

        if (baseMeterIndex >= mCurAudioMeterData.baseMeters.Length - offset + 1)
            return -2f;

        if(baseMeterIndex == mCurAudioMeterData.baseMeters.Length - offset)
        {
            return mCurAudioMeterData.audioLen - mCurAudioMeterData.baseMeters[baseMeterIndex];
        }

        return mCurAudioMeterData.baseMeters[baseMeterIndex + offset] - timeRecord;
    }


    public override void Dispose()
    {

    }
}
