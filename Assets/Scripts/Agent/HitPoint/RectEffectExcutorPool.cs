using System.Collections.Generic;
using GameEngine;

public class RectEffectExcutorPool : Singleton<RectEffectExcutorPool>
{
    private Stack<RectEffectExcutor> mPool;

    public RectEffectExcutorPool()
    {
        mPool = new Stack<RectEffectExcutor>();
    }
    
    public RectEffectExcutor PopExcutor()
    {
        if (mPool.TryPop(out RectEffectExcutor excutor))
        {
            if (excutor != null)
            {
                return excutor;
            }
        }

        return new RectEffectExcutor();
    }

    public void PushExcutor(RectEffectExcutor excutor)
    {
        if (excutor == null)
            return;

        if (mPool.Contains(excutor))
            return;

        mPool.Push(excutor);
    }
}
