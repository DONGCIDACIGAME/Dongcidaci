namespace GameEngine
{

    /// <summary>
    /// 扩容速度
    /// </summary>
    public enum SimpleListEnlargeMode
    {
        Slow = 1,
        Normal = 2,
        Fast = 3,
    }

    public struct HashIndex
    {
        public int hashCode;
        public int index;
    }

    public class SimpleList<T>
    {
        private HashIndex[] mIndexArray;
        private MyQueue<int> validIndexQueue;
        private T[] mObjArray;
        private SimpleListEnlargeMode mEnlargeMode;

        public SimpleList(uint initSize, SimpleListEnlargeMode mode)
        {
            mIndexArray = new HashIndex[initSize];
            validIndexQueue = new MyQueue<int>();
            mObjArray = new T[initSize];

            mEnlargeMode = mode;
        }

        private void Enlarge()
        {

        }

        private int GetValidIndex()
        {

            return -1;
        }

        public void Add(T obj)
        {
            GetValidIndex();
            while (GetValidIndex() == -1)
            {
                Enlarge();
            }
        }
    }
}