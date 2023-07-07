using GameEngine;
using System.Collections.Generic;

namespace GameSkillEffect
{
    public class SkEftEntityManager : ModuleManager<SkEftEntityManager>, IMeterHandler
    {
        /// <summary>
        /// 用于存放所有注册进来的技能效果实体
        /// </summary>
        private List<SkEftEntity> _mEntitys;


        public override void Initialize()
        {
            _mEntitys = new List<SkEftEntity>();

        }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate(float deltaTime)
        {
            //base.OnUpdate(deltaTime);


        }


        public void OnMeterEnter(int meterIndex)
        {
            throw new System.NotImplementedException();
        }

        public void OnMeterEnd(int meterIndex)
        {
            throw new System.NotImplementedException();
        }


    }
}

