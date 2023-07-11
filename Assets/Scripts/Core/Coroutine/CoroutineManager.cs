using System.Collections;
using System.Collections.Generic;

namespace GameEngine
{
    public class CoroutineManager : ModuleManager<CoroutineManager>
    {
        /// <summary>
        /// 运行中的协程
        /// </summary>
        private LinkedList<GameCoroutine> _coroutines;

        /// <summary>
        /// 待移除的协程
        /// </summary>
        private LinkedList<GameCoroutine> _toStopCoroutines;

        public override void Initialize()
        {
            _coroutines = new LinkedList<GameCoroutine>();
            _toStopCoroutines = new LinkedList<GameCoroutine>();
        }

        public override void Dispose()
        {
            _coroutines = null;
            _toStopCoroutines = null;
        }

        public override void OnUpdate(float deltaTime)
        {
            if(_coroutines == null || _toStopCoroutines == null)
            {
                Log.Error(LogLevel.Critical, "CoroutineManager OnUpdate Error!");
                return;
            }

            //if(_coroutines.Count == 0 && _toStopCoroutines.Count == 0)
            //{
            //    return;
            //}

            LinkedListNode<GameCoroutine> node = _coroutines.First;
            while (node != null)
            {
                GameCoroutine co = node.Value;
                if (co != null && !_toStopCoroutines.Contains(co))
                {
                    if (!co.MoveNext(deltaTime))
                    {
                        _toStopCoroutines.AddLast(co);
                    }
                }
                node = node.Next;
            }

            LinkedListNode<GameCoroutine> toStop = _toStopCoroutines.First;
            while(toStop != null)
            {
                GameCoroutine co = toStop.Value;
                if (co != null)
                {
                    _coroutines.Remove(co);
                }
                toStop = toStop.Next;
            }

            _toStopCoroutines.Clear();
        }

        public GameCoroutine StartCoroutine(IEnumerator routine)
        {
            if (routine == null)
            {
                Log.Error(LogLevel.Info, "StartCoroutine Failed,routine is null!");
                return null;
            }

            if (_coroutines == null)
            {
                Log.Error(LogLevel.Info, "StartCoroutine Failed,_coroutines is null!");
                return null;
            }

            GameCoroutine co = new GameCoroutine(routine);
            _coroutines.AddLast(co);
            return co;
        }

        public void StopCoroutine(GameCoroutine co)
        {
            if (co == null)
            {
                return;
            }

            if(_toStopCoroutines != null)
            {
                _toStopCoroutines.AddLast(co);
            }
        }

        public void StopAllCoroutines()
        {
            if(_coroutines != null)
            {
                _coroutines.Clear();
            }

            if(_toStopCoroutines != null)
            {
                _toStopCoroutines.Clear();
            }
        }
    }
}


