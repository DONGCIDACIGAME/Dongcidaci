public interface IMeterHandler
{
    void OnMeterEnter(int meterIndex);

    void OnMeterEnd(int meterIndex);

    //TODO: 要加一个音乐切换的处理方法
    //void OnAudioChange();
}

