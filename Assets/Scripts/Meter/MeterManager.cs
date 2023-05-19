using System.Collections.Generic;
using GameEngine;
using System.Text;
using UnityEngine;

public class MeterManager : ModuleManager<MeterManager>
{
    /// <summary>
    /// 当前音乐的节奏数据
    /// </summary>
    private AudioMeterData mCurAudioMeterData;

    /// <summary>
    /// 当前所处的节奏index
    /// </summary>
    public int MeterIndex { get; private set; }

    /// <summary>
    /// 音乐开始到当前的时间（loop）
    /// </summary>
    private float timeRecord;


    /// <summary>
    /// Added by Weng 2023/05/05
    /// 当前距离这1拍索引已经经过的时间
    /// </summary>
    public float CrtMeterPassedTime {

        get
        {
            //Added By Weng 2023/05/05
            var gap = (timeRecord - mCurAudioMeterData.baseMeters[MeterIndex]);
            return gap > 0 ? gap : 0;
        }
    }

    /// <summary>
    /// 节奏控制器是否启用
    /// </summary>
    private bool enable;

    /// <summary>
    /// 所有的节拍处理器
    /// </summary>
    private HashSet<IMeterHandler> mMeterHandlers;

    /// <summary>
    /// 节拍总数量
    /// </summary>
    private int totalMeterLen;

    public int GetCurAudioTotalMeterLen()
    {
        return totalMeterLen;
    }

    public override void Initialize()
    {
        mMeterHandlers = new HashSet<IMeterHandler>();
    }


    public void RegisterMeterHandler(IMeterHandler handler)
    {
        if (handler == null)
        {
            Log.Error(LogLevel.Normal, "RegisterMeterHandler Error, meter handler is null!");
            return;
        }

        if(mMeterHandlers.Contains(handler))
        {
            Log.Error(LogLevel.Normal, "RegisterMeterHandler Error, repeate register meter handler!");
            return;
        }

        mMeterHandlers.Add(handler);
    }

    public void UnregiseterMeterHandler(IMeterHandler handler)
    {
        if (handler == null)
        {
            Log.Error(LogLevel.Info, "UnregiseterMeterHandler Error, meter handler is null!");
            return;
        }

        if(mMeterHandlers.Contains(handler))
        {
            mMeterHandlers.Remove(handler);
        }
    }

    public int GetCurrentMusicRhythmType()
    {
        if (mCurAudioMeterData == null)
            return MeterDefine.RhythmType_Unknown;

        return mCurAudioMeterData.rhythmType;
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

        if (mCurAudioMeterData != null)
            totalMeterLen = mCurAudioMeterData.baseMeters.Length;
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
        MeterIndex = 0;
    }

    public void Stop()
    {
        Reset();
    }

    public void Pause()
    {
        enable = false;
    }

    public void Resume()
    {
        enable = true;
    }

    public bool CheckTriggerSceneBehaviour(int meterIndex)
    {
        if (mCurAudioMeterData == null)
            return false;

        if (meterIndex < 0 || meterIndex > totalMeterLen - 1)
        {
            return false;
        }

        int localIndex = meterIndex % mCurAudioMeterData.sceneBehaviourMeterTrigger.Length;
        return mCurAudioMeterData.sceneBehaviourMeterTrigger[localIndex] == 1;
    }

    public bool CheckTriggerMeter(int meterIndex, float tolerance, float offset)
    {
        return IsInMeterWithTolerance(meterIndex, tolerance, offset);
    }

    /// <summary>
    /// 是否在节拍的指定容差范围内
    /// </summary>
    /// <param name="meterIndex"></param>
    /// <param name="tolerance"></param>
    /// <returns></returns>
    public bool IsInMeterWithTolerance(int meterIndex, float tolerance, float offset)
    {
        if (mCurAudioMeterData == null)
            return false;

        if (meterIndex < 0 || meterIndex > totalMeterLen - 1)
        {
            return false;
        }

        // 对于音乐起始拍和结束拍的特殊处理
        if (meterIndex == 0 || meterIndex == totalMeterLen - 1)
        {
            return (timeRecord >= mCurAudioMeterData.baseMeters[totalMeterLen - 1] - tolerance/2 + offset)
                        && timeRecord <= mCurAudioMeterData.baseMeters[0] + tolerance/2 + offset;
        }

        return timeRecord >= mCurAudioMeterData.baseMeters[meterIndex] - tolerance / 2 + offset
                    && timeRecord <= mCurAudioMeterData.baseMeters[meterIndex] + tolerance/2 + offset;
    }

    /// <summary>
    /// 检测在本拍的时间内是否触发
    /// </summary>
    /// <param name="tolerance">检测的容差</param>
    /// <param name="offset">检测时的偏移</param>
    /// <param name="triggerMeter">触发的节奏index</param>
    /// <returns>是否触发</returns>
    public bool CheckTriggered(float tolerance, float offset, out int triggerMeter)
    {
        triggerMeter = 0;
        if (mCurAudioMeterData == null)
            return false;

       int nextMeter = GetMeterIndex(MeterIndex, 1);

        /*
         *          触发检测
         *   |---------------------|---------------------|
         *    0000                 00
         *    000：检测部分
         *   检测触发时，只有本拍的开始段和本拍的结束段是可以触发的区域
         *   注意不能使用上一拍，因为上一拍已经过去了，再怎么检测也是触发不了的
         *   检测的容差可以传入偏移
         */

        float trigger1_Start = mCurAudioMeterData.baseMeters[MeterIndex];
        float trigger1_End = trigger1_Start + tolerance/2f + offset;
        float trigger2_End = mCurAudioMeterData.baseMeters[nextMeter];
        float trigger2_Start = trigger2_End - tolerance/2f + offset;

        if(timeRecord >= trigger1_Start && timeRecord <= trigger1_End)
        {
            triggerMeter = MeterIndex;
            return true;
        }

        if (timeRecord >= trigger2_Start && timeRecord <= trigger2_End)
        {
            triggerMeter = GetMeterIndex(MeterIndex, 1);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 触发在拍子上需要执行的操作
    /// </summary>
    private void TriggerMeter()
    {
        foreach(IMeterHandler handler in mMeterHandlers)
        {
            handler.OnMeter(MeterIndex);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="deltaTime"></param>
    public override void OnUpdate(float deltaTime)
    {
        if (!enable)
            return;

        if (mCurAudioMeterData == null)
            return;

        if (totalMeterLen < 2)
            return;


        timeRecord = AudioManager.Ins.GetCurBgmTime();

        if (timeRecord >= mCurAudioMeterData.audioLen)
        {
            timeRecord %= mCurAudioMeterData.audioLen;
            MeterIndex = 0;
        }

        if(MeterIndex >= totalMeterLen - 1)
        {
            Log.Error(LogLevel.Critical, "MeterManager OnUpdate Error, BaseMeterIndex Index out of range!");
            return;
        }

        if (timeRecord >= mCurAudioMeterData.baseMeters[MeterIndex + 1])
        {
            MeterIndex++;

            if(MeterIndex == totalMeterLen - 1)
            {
                MeterIndex = 0;
            }

            if(MeterIndex > 0)
            {
                TriggerMeter();
            }
        }

        //Log.Logic(LogLevel.Info, "~~~~~~~~~bgm time:{0}, meter time:{1}", AudioManager.Ins.GetCurBgmTime(), timeRecord);
    }


    /// <summary>
    /// 获取当前到接下来第offset拍的时间
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    public float GetTimeToMeter(int offset)
    {
        if (mCurAudioMeterData == null)
            return -1f;

        if (MeterIndex >= totalMeterLen - 1)
        {
            Log.Error(LogLevel.Critical, "MeterManager GetTimeToBaseMeter Error, BaseMeterIndex Index out of range!");
            return -2f;
        }

        int targetIndex = MeterIndex + offset;

        float time = 0;
        if(targetIndex >= totalMeterLen - 1)
        {
            time += mCurAudioMeterData.audioLen - timeRecord;
            targetIndex %= totalMeterLen;
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
    /// 获取从哪个拍子开始，偏移的拍子索引
    /// </summary>
    /// <param name="from"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public int GetMeterIndex(int from, int offset)
    {
        int targetIndex = from + offset;
        if(targetIndex >= totalMeterLen)
        {
            targetIndex %= totalMeterLen;
        }

        return targetIndex;
    }

    /// <summary>
    /// 获取从两个拍子之间的时间间隔
    /// </summary>
    /// <param name="from">起始拍</param>
    /// <param name="to">结束拍</param>
    /// <returns>时间间隔</returns>
    public float GetTotalMeterTime(int from, int to)
    {
        if (mCurAudioMeterData == null)
            return -1f;

        if (from >= totalMeterLen - 1 || to >= totalMeterLen)
        {
            Log.Error(LogLevel.Critical, "MeterManager GetCurrentMeterEclipseTime Error, meter Index out of range!");
            return -2f;
        }

        float time;
        // 如果结束拍的index<起始拍，说明结束拍是loop过来的
        if (to < from)
        {
            time = mCurAudioMeterData.audioLen - mCurAudioMeterData.baseMeters[from] + mCurAudioMeterData.baseMeters[to];
        }
        else
        {
            time = mCurAudioMeterData.baseMeters[to] - mCurAudioMeterData.baseMeters[from];
        }

        //Log.Logic(LogLevel.Info, "GetCurrentMeterTime--from:{0},to:{1},totalTime:{2}", from,to, time);

        return time;
    }


    /// <summary>
    /// 获取当前拍子的总时间
    /// </summary>
    /// <returns></returns>
    public float GetCurrentMeterTime()
    {
        int targetMeter = GetMeterIndex(MeterIndex, 1);
        return GetTotalMeterTime(MeterIndex, targetMeter);
    }


    public override void Dispose()
    {

    }
}
