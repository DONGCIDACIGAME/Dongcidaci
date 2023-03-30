using GameEngine;
using System.Text;

public class ModuleBitTypeQuery : IBitTypeQuery
{
    private StringBuilder fullTypeStr = new StringBuilder();

    private ModuleBitTypeQuery() { }

    private static ModuleBitTypeQuery _ins;
    public static ModuleBitTypeQuery Ins
    {
        get
        {
            if (_ins == null)
            {
                _ins = new ModuleBitTypeQuery();
            }

            return _ins;
        }
    }

    private void BitTypeEmmurator(BitType bt)
    {
        fullTypeStr.Append("<" + bt.ToString() + ">");
    }

    public string BitTypeTranslate(BitType bt)
    {
        if(bt == null)
        {
            Log.Warning("BitTypeTranslate failed,BitType is null");
            return string.Empty;
        }
        fullTypeStr.Clear();
        bt.ForEachSingleType(BitTypeEmmurator);
        return fullTypeStr.ToString();
    }

    //public int GetBufferMaxSize()
    //{
    //    return CoreDefine.BitTypeModuleBufferSize / CoreDefine.bufferSizeOfInt;
    //}

    public int GetBitTypeMaxValue()
    {
        return CoreDefine.BitTypeModuleBufferSize;
    }
}
