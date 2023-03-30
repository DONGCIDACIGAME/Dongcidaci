using GameEngine;

public class EventBitTypeQuery : IBitTypeQuery
{
    private EventBitTypeQuery() { }

    private static EventBitTypeQuery _ins;
    public static EventBitTypeQuery Ins
    {
        get
        {
            if (_ins == null)
            {
                _ins = new EventBitTypeQuery();
            }

            return _ins;
        }
    }

    public int GetBitTypeMaxValue()
    {
        return CoreDefine.BitTypeEventBufferSize;
    }
}
