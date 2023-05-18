using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class UIEntityPool<T>
    {
        private Stack<T> pool;

        public UIEntityPool()
        {
            this.pool = new Stack<T>();
        }

        public void PushObj(T obj)
        {
            pool.Push(obj);
        }

        public T PopObj()
        {
            if (pool.Count == 0)
            {
                return default(T);
            }

            return pool.Pop();
        }

        public void Clear()
        {
            pool.Clear();
        }
    }
}
