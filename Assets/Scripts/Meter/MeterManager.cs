using System.Collections.Generic;
using GameEngine;
using System.Text;
using UnityEngine;

public class MeterManager : ModuleManager<MeterManager>
{
    /// <summary>
    /// ��ǰ���ֵĽ�������
    /// </summary>
    private AudioMeterData mCurAudioMeterData;

    /// <summary>
    /// ��ǰ�����Ľ���index
    /// </summary>
    public int MeterIndex { get; private set; }

    /// <summary>
    /// ���ֿ�ʼ����ǰ��ʱ�䣨loop��
    /// </summary>
    private float timeRecord;

    /// <summary>
    /// ����������Ƿ�����
    /// </summary>
    private bool enable;

    private Dictionary<int, IMeterHandler> mBaseMeterHandlers;

    /// <summary>
    /// ����������
    /// </summary>
    private int totalMeterLen;

    public int GetCurAudioTotalMeterLen()
    {
        return totalMeterLen;
    }

    public override void Initialize()
    {
        mBaseMeterHandlers = new Dictionary<int, IMeterHandler>();
    }

    public void RegisterMeterHandler(IMeterHandler handler)
    {
        if (handler == null)
            return;

        int unicode = handler.GetHashCode();
        if (mBaseMeterHandlers.ContainsKey(unicode))
            return;

        mBaseMeterHandlers.Add(unicode, handler);
    }

    public void UnregiseterMeterHandler(IMeterHandler handler)
    {
        if (handler == null)
            return;

        int unicode = handler.GetHashCode();
        if (mBaseMeterHandlers.ContainsKey(unicode))
        {
            mBaseMeterHandlers.Remove(unicode);
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
    /// �Ƿ��ڽ��ĵ�ָ���ݲΧ��
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

        // ����������ʼ�ĺͽ����ĵ����⴦��
        if (meterIndex == 0 || meterIndex == totalMeterLen - 1)
        {
            return (timeRecord >= mCurAudioMeterData.baseMeters[totalMeterLen - 1] - tolerance/2 + offset)
                        && timeRecord <= mCurAudioMeterData.baseMeters[0] + tolerance/2 + offset;
        }

        return timeRecord >= mCurAudioMeterData.baseMeters[meterIndex] - tolerance / 2 + offset
                    && timeRecord <= mCurAudioMeterData.baseMeters[meterIndex] + tolerance/2 + offset;
    }

    /// <summary>
    /// ����ڱ��ĵ�ʱ�����Ƿ񴥷�
    /// </summary>
    /// <param name="tolerance">�����ݲ�</param>
    /// <param name="offset">���ʱ��ƫ��</param>
    /// <param name="triggerMeter">�����Ľ���index</param>
    /// <returns>�Ƿ񴥷�</returns>
    public bool CheckTriggered(float tolerance, float offset, out int triggerMeter)
    {
        triggerMeter = 0;
        if (mCurAudioMeterData == null)
            return false;

       int nextMeter = GetMeterIndex(MeterIndex, 1);

        /*
         *          �������
         *   |---------------------|---------------------|
         *    0000                 00
         *    000����ⲿ��
         *   ��ⴥ��ʱ��ֻ�б��ĵĿ�ʼ�κͱ��ĵĽ������ǿ��Դ���������
         *   ע�ⲻ��ʹ����һ�ģ���Ϊ��һ���Ѿ���ȥ�ˣ�����ô���Ҳ�Ǵ������˵�
         *   �����ݲ���Դ���ƫ��
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

    private void TriggerBaseMeter()
    {
        foreach(IMeterHandler handler in mBaseMeterHandlers.Values)
        {
            handler.OnMeter(MeterIndex);
        }
    }

    /// <summary>
    ///  TODO:����Ľ����߼���Ҫ����
    ///  1. �Ƿ�Ҫ�Զ���ӵ�0��(0s)�����һ�ģ����1s��
    ///  2. ����༭�����Ӳ�ȫӦ����ô����
    ///  3. ��ȷ�������Ӻͽ���Ĺ�ϵ�Ƿ���ȷ
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

        timeRecord += deltaTime;

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

        else if (timeRecord >= mCurAudioMeterData.baseMeters[MeterIndex + 1])
        {
            MeterIndex++;

            if(MeterIndex == totalMeterLen - 1)
            {
                MeterIndex = 0;
            }

            if(MeterIndex > 0)
            {
                TriggerBaseMeter();
            }

        }
    }

    /// <summary>
    /// ��ȡ��ǰ����������offset�ĵ�ʱ��
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    public float GetTimeToBaseMeter(int offset)
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
    /// 
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
    /// ��ȡ����������֮���ʱ����
    /// </summary>
    /// <param name="from">��ʼ��</param>
    /// <param name="to">������</param>
    /// <returns>ʱ����</returns>
    public float GetTotalMeterTime(int from, int to)
    {
        if (mCurAudioMeterData == null)
            return -1f;

        if (from >= totalMeterLen - 1 || to >= totalMeterLen)
        {
            Log.Error(LogLevel.Critical, "MeterManager GetCurrentMeterEclipseTime Error, meter Index out of range!");
            return -2f;
        }

        float time = 0f;
        // ��������ĵ�index<��ʼ�ģ�˵����������loop������
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
    /// ��ȡ��ǰ���ӵ���ʱ��
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
