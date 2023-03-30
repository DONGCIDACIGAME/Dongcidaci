using System.Collections;
using System.Collections.Generic;

namespace GameEngine
{
    public delegate void SimpleQueueForeach<T>(T obj, out bool finishTag);

    /// <summary>
    ///  简单队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleQueue<T> : IEnumerable
    {
        private SimpleQueueNode<T> Head;
        private SimpleQueueNode<T> Tail;
        private SimpleQueueNode<T> Current;

        public void Clear()
        {
            Current = null;
            Tail = null;
            Head = null;
        }

        public T GetCurrent()
        {
            if(Current == null)
            {
                return default(T);
            }

            return Current.GetObj();
        }

        public bool MoveNext()
        {
            if (Current == null)
                return false;

            var next = Current.GetNext();
            if (next == null)
                return false;

            Current = next;
            return true;
        }

        public void Reset()
        {
            Current = Head;
        }
        public int Count { get; private set; } = 0;

        public void Enqueue(T obj)
        {
            SimpleQueueNode<T> node = new SimpleQueueNode<T>(obj);
            // 队列内没有元素
            if (Tail == null)
            {
                Head = node;
                Tail = Head;
                Count = 1;
            }
            else // 队列内有元素，加到队尾
            {
                Tail.SetNext(node);
                node.SetLast(Tail);
                Tail = node;
                Count++;
            }
        }

        public bool HasItem(T item)
        {
            SimpleQueueNode<T> cur = Head;

            while(cur != null)
            {
                if(cur.GetObj().Equals(item))
                {
                    return true;
                }

                cur = cur.GetNext();
            }

            return false;
        }

        public bool HasItem()
        {
            return Head != null;
        }

        public T Dequeue()
        {
            // 队列内没有元素
            if (Count == 0)
            {
                return default(T);
            }

            // 队列内只有一个元素
            T obj = Head.GetObj();
            if (Count == 1)
            {
                Head = null;
                Tail = null;
                Count = 0;
                return obj;
            }

            // 队列内有多个元素
            var next = Head.GetNext();
            if (next != null)
            {
                next.SetLast(null);
            }
            Head = next;
            Count--;
            return obj;
        }

        public IEnumerator GetEnumerator()
        {
            SimpleQueueNode<T> temp = Head;
            while (temp != null)
            {
                T obj = temp.GetObj();
                temp = temp.GetNext();
                yield return obj;
            }
        }
    }

    public class SimpleQueueNode<T>
    {
        private SimpleQueueNode<T> mLast;
        private SimpleQueueNode<T> mNext;

        private T mObj;

        public SimpleQueueNode(T obj)
        {
            mObj = obj;
        }

        public T GetObj()
        {
            return mObj;
        }

        public void SetLast(SimpleQueueNode<T> node)
        {
            mLast = node;
        }

        public SimpleQueueNode<T> GetLast()
        {
            return mLast;
        }

        public void SetNext(SimpleQueueNode<T> node)
        {
            mNext = node;
        }

        public SimpleQueueNode<T> GetNext()
        {
            return mNext;
        }
    }
}
