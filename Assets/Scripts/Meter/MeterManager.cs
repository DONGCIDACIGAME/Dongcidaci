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
    public int BaseMeterIndex { get; private set; }

    // 音乐开始到当前的时间（loop）
    private float timeRecord;

    // 节奏控制器是否启用
    private bool enable;

    private Dictionary<int, IMeterHandler> mBaseMeterHandlers;

    public override void Initialize()
    {
        mBaseMeterHandlers = new Dictionary<int, IMeterHandler>();
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

    public void UnregiseterBaseMeterHandler(IMeterHandler handler)
    {
        if (handler == null)
            return;

        int unicode = handler.GetHashCode();
        if (mBaseMeterHandlers.ContainsKey(unicode))
        {
            mBaseMeterHandlers.Remove(unicode);
        }
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
        BaseMeterIndex = 0;
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

    public bool CheckTriggerBaseMeter()
    {
        if (mCurAudioMeterData == null)
            return false;

        if (timeRecord >= mCurAudioMeterData.baseMeters[BaseMeterIndex] - GamePlayDefine.MeterCheckTolerance 
            && timeRecord <= mCurAudioMeterData.baseMeters[BaseMeterIndex] + GamePlayDefine.MeterCheckTolerance)
        {
            return true;
        }

        return false;
    }

    private void TriggerBaseMeter()
    {
        foreach(IMeterHandler handler in mBaseMeterHandlers.Values)
        {
            handler.OnMeter(BaseMeterIndex);
        }
    }

    /// <summary>
    ///  TODO:这里的节拍逻辑需要调整
    ///  1. 是否要自动添加第0拍(0s)和最后一拍（最后1s）
    ///  2. 如果编辑的拍子不全应该怎么处理
    ///  3. 再确认下拍子和节奏的关系是否正确
    /// </summary>
    /// <param name="deltaTime"></param>
    public override void OnUpdate(float deltaTime)
    {
        if (!enable)
            return;

        if (mCurAudioMeterData == null)
            return;

        if (mCurAudioMeterData.baseMeters.Length < 2)
            return;

        timeRecord += deltaTime;

        if (timeRecord >= mCurAudioMeterData.audioLen)
        {
            timeRecord %= mCurAudioMeterData.audioLen;
            BaseMeterIndex = 0;
        }

        if(BaseMeterIndex >= mCurAudioMeterData.baseMeters.Length - 1)
        {
            Log.Error(LogLevel.Critical, "MeterManager OnUpdate Error, BaseMeterIndex Index out of range!");
            return;
        }

        else if (timeRecord >= mCurAudioMeterData.baseMeters[BaseMeterIndex + 1])
        {
            BaseMeterIndex++;
            if(BaseMeterIndex > 0)
            {
                TriggerBaseMeter();
            }
        }
    }

    /// <summary>
    /// 获取当前到接下来第offset拍的时间
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    public float GetTimeToBaseMeter(int offset)
    {
        if (mCurAudioMeterData == null)
            return -1f;

        if (BaseMeterIndex >= mCurAudioMeterData.baseMeters.Length - 1)
        {
            Log.Error(LogLevel.Critical, "MeterManager GetTimeToBaseMeter Error, BaseMeterIndex Index out of range!");
            return -2f;
        }

        int targetIndex = BaseMeterIndex + offset;

        float time = 0;
        if(targetIndex >= mCurAudioMeterData.baseMeters.Length - 1)
        {
            time += mCurAudioMeterData.audioLen - timeRecord;
            targetIndex %= mCurAudioMeterData.baseMeters.Length;
            time += mCurAudioMeterData.baseMeters[targetIndex];
        }
        else
        {
            time = mCurAudioMeterData.baseMeters[targetIndex] - timeRecord;
        }

        //Log.Logic(LogLevel.Info, "GetTimeToBaseMeter--baseMeterIndex:{0},targetMeter:{1}, time:{2}, curTime:{3}", BaseMeterIndex, targetIndex, time, timeRecord);
        return time;
    }

    /// <summary>
    /// 获取当前拍子已经过去的时间
    /// </summary>
    /// <returns></returns>
    public float GetCurrentMeterEclipseTime()
    {
        if (mCurAudioMeterData == null)
            return -1f;

        if (BaseMeterIndex >= mCurAudioMeterData.baseMeters.Length - 1)
        {
            Log.Error(LogLevel.Critical, "MeterManager GetCurrentMeterEclipseTime Error, BaseMeterIndex Index out of range!");
            return -2f;
        }

        float time = timeRecord - mCurAudioMeterData.baseMeters[BaseMeterIndex];

        Log.Logic(LogLevel.Info, "GetCurrentMeterTime--baseMeterIndex:{0},time:{1},", BaseMeterIndex, time);

        return time;
    }


    /// <summary>
    /// 获取当前拍子的总时间
    /// </summary>
    /// <returns></returns>
    public float GetCurrentMeterTime()
    {
        //float time1 = GetCurrentMeterEclipseTime();
        //float time2 = GetTimeToBaseMeter(1);
        //float currentMeterTime = time1 + time2;

        if (mCurAudioMeterData == null)
            return -1f;

        if (BaseMeterIndex >= mCurAudioMeterData.baseMeters.Length - 1)
        {
            Log.Error(LogLevel.Critical, "MeterManager GetCurrentMeterTime Error, BaseMeterIndex Index out of range!");
            return -2f;
        }

        float time = mCurAudioMeterData.baseMeters[BaseMeterIndex + 1] - mCurAudioMeterData.baseMeters[BaseMeterIndex];
        if(time <= 0)
        {
            Log.Error(LogLevel.Critical, "MeterManager GetCurrentMeterTime Error, current meter duration <= 0");
            return -3f;
        }

        //Log.Logic(LogLevel.Info, "GetCurrentMeterTime--baseMeterIndex:{0},time:{1},", BaseMeterIndex, time);
        return time;
    }


    public override void Dispose()
    {

    }
}
