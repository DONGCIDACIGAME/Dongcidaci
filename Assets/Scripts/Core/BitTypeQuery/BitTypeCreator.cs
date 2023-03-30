using GameEngine;
using System.Collections.Generic;

public static class BitTypeCreator
{
    public static BitType CreateModuleBitType(int index, string desc)
    {
        int bufferSize = ModuleBitTypeQuery.Ins.GetBitTypeMaxValue() / CoreDefine.bufferSizeOfInt;
        if (index < 0 || index >= bufferSize * CoreDefine.bufferSizeOfInt)
        {
            Log.Error(LogLevel.Fatal, "CreateModuleBitType Failed, index:{0} out of range:[{1},{2}]", index, 0, bufferSize);
            return null;
        }

        return new BitType(index, desc, ModuleBitTypeQuery.Ins, false);
    }

    public static BitType CreateEventModuleBitType(int index, string desc)
    {
        int bufferSize = EventBitTypeQuery.Ins.GetBitTypeMaxValue() / CoreDefine.bufferSizeOfInt;
        if (index < 0 || index >= bufferSize * CoreDefine.bufferSizeOfInt)
        {
            Log.Error(LogLevel.Fatal, "CreateModuleBitType Failed, index:{0} out of range:[{1},{2}]", index, 0, bufferSize);
            return null;
        }

        return new BitType(index, desc, EventBitTypeQuery.Ins, false);
    }
}



