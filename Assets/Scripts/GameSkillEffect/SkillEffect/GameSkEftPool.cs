using System.Collections.Generic;
using GameEngine;
using System;
using DongciDaci;

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
            Log.Logic(LogLevel.Info, "Game Sk Effect Pool push a new skill effect");
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

        /// <summary>
        /// 从缓存时池获取一个技能效果；并初始化
        /// </summary>
        /// <param name="user"></param>
        /// <param name="skEftData"></param>
        /// <returns></returns>
        public SkillEffect PopWithInit(Agent user, SkillEffectData skEftData)
        {
            // 反射创建相应的技能效果
            SkEftBaseCfg eftCfg = null;
            SkillEffectCfg_Data data = ConfigDatabase.Get<SkillEffectCfg_Data>();
            if (data == null) return null;
            if (data.SkEftBaseCfgItems.ContainsKey(skEftData.effectCfgUID) == false) return null;

            // 获取uid指向的技能效果的基本配置
            eftCfg = data.SkEftBaseCfgItems[skEftData.effectCfgUID];
            var newSkEft = Pop(eftCfg);
            if (newSkEft != null)
            {
                if(newSkEft.InitSkEft(user, skEftData, eftCfg) == false)
                {
                    return null;
                }
            }
            return newSkEft;
        }


        /// <summary>
        /// 根据技技能效果的基础配置数据，从缓存池中获取对应的实例
        /// </summary>
        /// <param name="eftCfg"></param>
        /// <returns></returns>
        public SkillEffect Pop(SkEftBaseCfg eftCfg)
        {
            string eftClassName = eftCfg.ClassName;
            Type effecType = Type.GetType("GameSkillEffect." + eftClassName);
            if (effecType == null) return null;
            return Pop(effecType);
        }


        public SkillEffect Pop(Type eftType)
        {
            if (eftType.IsSubclassOf(typeof(SkillEffect)) == false) return null;
            // 检查是否为空
            if (_skEftPool == null || _skEftPool.Count == 0)
            {
                // 反射创建新的实例
                return Activator.CreateInstance(eftType) as SkillEffect;
            }

            // 查找有这个类别的技能效果
            for (int i = _skEftPool.Count - 1; i >= 0; i--)
            {
                if (_skEftPool[i].GetType().Equals(eftType))
                {
                    var skEft = _skEftPool[i];
                    _skEftPool.RemoveAt(i);
                    return skEft;
                }
            }

            // 池子中没有技能效果，反射创建新的实例
            return Activator.CreateInstance(eftType) as SkillEffect;

        }




    }
}


