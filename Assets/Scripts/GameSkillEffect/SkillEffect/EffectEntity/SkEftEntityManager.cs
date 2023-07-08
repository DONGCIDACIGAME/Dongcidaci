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

            // 注册相应的MeterHandler
            MeterManager.Ins.RegisterMeterHandler(this);
        }

        /// <summary>
        /// 注册一个新的技能对象到这个管理器
        /// </summary>
        /// <param name="newSkEntity"></param>
        /// <returns></returns>
        public void RegisterNewSkEftEntity(SkEftEntity newSkEntity)
        {
            if (_mEntitys == null) return;
            if (_mEntitys.Contains(newSkEntity)) return;
            _mEntitys.Add(newSkEntity);
        }

        /// <summary>
        /// 移除一个已经注册的技能实体
        /// </summary>
        /// <param name="removeSkEntity"></param>
        public void UnRegisterSkEftEntity(SkEftEntity removeSkEntity)
        {
            if (_mEntitys == null) return;
            _mEntitys.Remove(removeSkEntity);
        }



        public override void Dispose()
        {
            MeterManager.Ins.UnregiseterMeterHandler(this);
        }

        public override void OnUpdate(float deltaTime)
        {
            //base.OnUpdate(deltaTime);

            if (_mEntitys == null || _mEntitys.Count == 0) return;
            for (int i = _mEntitys.Count-1; i>=0; i--)
            {
                if(_mEntitys[i] is IGameUpdate)
                {
                    var updateEntity = _mEntitys[i] as IGameUpdate;
                    updateEntity.OnUpdate(deltaTime);
                }
            }
        }


        public void OnMeterEnter(int meterIndex)
        {
            if (_mEntitys == null || _mEntitys.Count == 0) return;
            for (int i = _mEntitys.Count - 1; i >= 0; i--)
            {
                if (_mEntitys[i] is IMeterHandler)
                {
                    var meterHandleEntity = _mEntitys[i] as IMeterHandler;
                    meterHandleEntity.OnMeterEnter(meterIndex);
                }
            }
        }

        public void OnMeterEnd(int meterIndex)
        {
            if (_mEntitys == null || _mEntitys.Count == 0) return;
            for (int i = _mEntitys.Count - 1; i >= 0; i--)
            {
                if (_mEntitys[i] is IMeterHandler)
                {
                    var meterHandleEntity = _mEntitys[i] as IMeterHandler;
                    meterHandleEntity.OnMeterEnd(meterIndex);
                }
            }
        }


    }
}

