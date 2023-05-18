using System.Collections.Generic;

namespace GameEngine
{
    public class GeneralGamePool<T> where T : new()
    {
        private Stack<T> mPool;

        public GeneralGamePool()
        {
            mPool = new Stack<T>();
        }

        public T Pop()
        {
            if (mPool.TryPop(out T obj))
            {
                if (obj != null)
                {
                    return obj;
                }
            }

            return new T();
        }

        public void Push(T obj)
        {
            if (obj == null)
                return;

            if (mPool.Contains(obj))
                return;

            mPool.Push(obj);
        }
    }
}
