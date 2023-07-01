using System.Collections;

namespace GameEngine
{
    public delegate void MyQueueForeach<T>(T obj, out bool finishTag);

    /// <summary>
    ///  简单队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MyQueue<T> : IEnumerable
    {
        private MyQueueNode<T> Head;
        private MyQueueNode<T> Tail;
        private MyQueueNode<T> Current;
        public int Count { get; private set; } = 0;

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
            if (Count == 0)
                return false;


            if (Current == null)
            {
                Current = Head;
                return true;
            }    

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


        public void Enqueue(T obj)
        {
            MyQueueNode<T> node = new MyQueueNode<T>(obj);
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
            MyQueueNode<T> cur = Head;

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

        public void RemoveItem(T item)
        {
            MyQueueNode<T> cur = Head;

            while (cur != null)
            {
                if (cur.GetObj().Equals(item))
                {
                    MyQueueNode<T> last = cur.GetLast();
                    MyQueueNode<T> next = cur.GetNext();

                    last.SetNext(next);
                    next.SetLast(last);

                    // 删除的是第一个元素
                    if (cur.Equals(Head))
                    {
                        Head = next;
                    }
                    break;
                }
            }
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
            MyQueueNode<T> temp = Head;
            while (temp != null)
            {
                T obj = temp.GetObj();
                temp = temp.GetNext();
                yield return obj;
            }
        }
    }

    public class MyQueueNode<T>
    {
        private MyQueueNode<T> mLast;
        private MyQueueNode<T> mNext;

        private T mObj;

        public MyQueueNode(T obj)
        {
            mObj = obj;
        }

        public T GetObj()
        {
            return mObj;
        }

        public void SetLast(MyQueueNode<T> node)
        {
            mLast = node;
        }

        public MyQueueNode<T> GetLast()
        {
            return mLast;
        }

        public void SetNext(MyQueueNode<T> node)
        {
            mNext = node;
        }

        public MyQueueNode<T> GetNext()
        {
            return mNext;
        }
    }
}
