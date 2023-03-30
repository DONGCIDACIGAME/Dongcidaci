using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class GamePool<T>
    {
        private Stack<T> pool;

        public GamePool()
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
