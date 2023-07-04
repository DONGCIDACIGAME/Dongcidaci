using System.Collections;
using System.Collections.Generic;
using GameEngine;


namespace GameSkillEffect
{
    public class GameSkEftPool : Singleton<GameSkEftPool>
    {
        private List<SkillEffect> _skEftPool = new List<SkillEffect>();

        /// <summary>
        /// 往缓存池中加入一个指定类型的技能效果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="skEft"></param>
        public void Push<T>(T skEft) where T : SkillEffect, new()
        {
            if (skEft == null)
                return;

            if (_skEftPool.Contains(skEft))
                return;

            _skEftPool.Add(skEft);
        }

        /// <summary>
        /// 从缓存池中取出一个指定类型的技能效果;
        /// 后入先出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="skEft"></param>
        public void Pop<T>(out T skEft) where T : SkillEffect, new()
        {
            // 检查是否为空
            if (_skEftPool == null || _skEftPool.Count == 0)
            {
                skEft = new T();
                return;
            }

            // 查找有这个类别的技能效果
            for (int i = _skEftPool.Count - 1; i >= 0; i--)
            {
                if (_skEftPool[i] is T)
                {
                    skEft = _skEftPool[i] as T;
                    _skEftPool.RemoveAt(i);
                    return;
                }
            }
            // 没有的情况下新建一个实例
            skEft = new T();
        }


    }
}


