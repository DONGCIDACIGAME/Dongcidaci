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
    /// 当前所处的节奏index，这个index自增的，不在结尾处做循环处理
    /// 最高2147483647拍，按一拍0.5s计算，可以折合为12427天，所以自增使用不会出现溢出
    /// </summary>
    public int MeterIndex { get; private set; }

    /// <summary>
    /// 音乐开始到当前的时间（loop）
    /// </summary>
    private float timeRecord;

    private int GetMeterIndexInMusic(int meterIndex)
    {
        if (meterIndex < 0)
            return -1;

        return meterIndex % totalMeterLen;
    }

    /// <summary>
    /// Added by Weng 2023/05/05
    /// 当前距离这1拍索引已经经过的时间
    /// </summary>
    public float CrtMeterPassedTime {

        get
        {
            //Added By Weng 2023/05/05
            int meterIndexInMusic = GetMeterIndexInMusic(MeterIndex);
            float gap = timeRecord - mCurAudioMeterData.baseMeters[meterIndexInMusic];
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

        if (meterIndex < 0)
        {
            return false;
        }

        // 获取节拍在音乐中的index
        int meterIndexInMusic = GetMeterIndexInMusic(meterIndex);

        // 获取节拍在触发数组中的index
        int localIndex = meterIndexInMusic % mCurAudioMeterData.sceneBehaviourMeterTrigger.Length;
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

        if (meterIndex < 0)
        {
            return false;
        }

        // 获取节拍在音乐中的index
        int meterIndexInMusic = GetMeterIndexInMusic(meterIndex);

        // 对于音乐起始拍和结束拍的特殊处理
        if (meterIndexInMusic == 0 || meterIndexInMusic == totalMeterLen - 1)
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

    private void TriggerMeterEnd()
    {
        foreach (IMeterHandler handler in mMeterHandlers)
        {
            handler.OnMeterEnd(MeterIndex);
        }
    }

    /// <summary>
    /// 触发在拍子上需要执行的操作
    /// </summary>
    private void TriggerMeterEnter()
    {
        foreach(IMeterHandler handler in mMeterHandlers)
        {
            handler.OnMeterEnter(MeterIndex);
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
        }

        // 获取节拍在音乐中的index
        int meterIndexInMusic = GetMeterIndexInMusic(MeterIndex);
        if (timeRecord >= mCurAudioMeterData.baseMeters[meterIndexInMusic + 1])
        {
            // 节拍结束
            TriggerMeterEnd();
            // 节拍自增
            MeterIndex++;
            // 节拍开始
            TriggerMeterEnter();
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

        int meterIndexInMusic_ori = GetMeterIndexInMusic(MeterIndex);
        int meterIndexInMusic_new = GetMeterIndexInMusic(GetMeterIndex(MeterIndex, offset));

        float time = 0;
        // 如果新的节拍index < 老的节拍index， 说明过了音乐的最后一拍，循环到开头了
        if(meterIndexInMusic_new < meterIndexInMusic_ori)
        {
            time += mCurAudioMeterData.audioLen - timeRecord;
            time += mCurAudioMeterData.baseMeters[meterIndexInMusic_new];
        }
        else
        {
            time = mCurAudioMeterData.baseMeters[meterIndexInMusic_new] - timeRecord;
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
        return targetIndex;
    }

    public int GetMeterOffset(int from, int to)
    {
        int offset = to - from;
        return offset;
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

        int meterIndexInMusic_ori = GetMeterIndexInMusic(from);
        int meterIndexInMusic_new = GetMeterIndexInMusic(to);

        float time = 0;
        // 如果新的节拍index < 老的节拍index， 说明过了音乐的最后一拍，循环到开头了
        if (meterIndexInMusic_new < meterIndexInMusic_ori)
        {
            time += mCurAudioMeterData.audioLen - mCurAudioMeterData.baseMeters[meterIndexInMusic_ori];
            time += mCurAudioMeterData.baseMeters[meterIndexInMusic_new];
        }
        else
        {
            time = mCurAudioMeterData.baseMeters[meterIndexInMusic_new] - mCurAudioMeterData.baseMeters[meterIndexInMusic_ori];
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

    public float GetCurrentMeterProgress()
    {
        // 当前拍的剩余时间
        float timeToNextMeter = GetTimeToMeter(1);
        // 当前拍的总时间
        float timeOfCurrentMeter = GetTotalMeterTime(MeterIndex, MeterIndex + 1);

        if (timeOfCurrentMeter <= 0)
        {
            Log.Error(LogLevel.Normal, "GetCurrentMeterProgress Error, 当前拍的总时间<=0, 当前拍:{0}", MeterIndex);
            return 1;
        }

        float progress = timeToNextMeter / timeOfCurrentMeter;

        return progress;
    }


    public override void Dispose()
    {
        mCurAudioMeterData = null;
        mMeterHandlers = null;
        MeterIndex = 0;
        totalMeterLen = 0;
        timeRecord = 0;
        enable = false;
    }
}
