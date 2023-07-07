using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

namespace GameSkillEffect
{
    public class BulletEntity : SkEftEntity, IMeterHandler, IUpdateCenterDrive
    {
        protected override MyColliderType ColliderType => MyColliderType.Collider_SkBullet;

        public override int GetEntityType()
        {
            return EntityTypeDefine.SkEftBullet;
        }


        private BulletEntityView _bulletView;

        private SkEntityInitData _skEntityInitData;

        private SkillEffect[] _carrySkEffects;


        #region MOVE CONTROL
        private bool _isMoveStart = false;
        private float _crtMoveSpeed;


        #endregion



        public override void InitSkEftEntity(SkEntityInitData entityInitData, SkillEffect[] carrySkEfts)
        {
            var go = PrefabUtil.LoadPrefab(entityInitData.PrefabPath, SkEftDefine.SkEftEntityNode, "Load Bullet Prefab");
            if(go == null)
            {
                Log.Error(LogLevel.Critical,"Init BulletEntity -- Load Prefab error");
                Dispose();
                return;
            }
            var ret = go.TryGetComponent<BulletEntityView>(out BulletEntityView bulletView);
            if(ret == false)
            {
                Log.Error(LogLevel.Critical, "Init BulletEntity -- No Bullet View On Prefab");
                Dispose();
                return;
            }

            BindBulletView(bulletView);
            _skEntityInitData = entityInitData;
            _carrySkEffects = carrySkEfts;
            

            // 1 初始化位置信息
            SetPosition(_skEntityInitData.WorldPos);
            SetRotation(_skEntityInitData.RotateAngle);

            // 注册到 Update Center
            RegisterToUpdateCenter(this);
            MeterManager.Ins.RegisterMeterHandler(this);
            _isMoveStart = true;
            _crtMoveSpeed = _skEntityInitData.FlightSpeed;
        }

        private void BindBulletView(BulletEntityView bulletView)
        {
            BindMapEntityView(bulletView);
            _bulletView = bulletView;
        }


        public override void Dispose()
        {
            base.Dispose();
            UnregisterFromUpdateCenter(this);
            MeterManager.Ins.UnregiseterMeterHandler(this);

            _carrySkEffects = null;
            //销毁 bullet view


        }


        public void OnMeterEnter(int meterIndex)
        {

        }

        public void OnMeterEnd(int meterIndex)
        {
            if (_isMoveStart)
            {
                if (_crtMoveSpeed < _skEntityInitData.FlightSpeed)
                {
                    _crtMoveSpeed = _skEntityInitData.FlightSpeed;
                }
                else
                {
                    _crtMoveSpeed = _skEntityInitData.FlightSpeed *0.4f;
                }
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_isMoveStart) return;

            Vector3 pos = GetPosition() + _skEntityInitData.FlightDir * _crtMoveSpeed;
            //MoveToPosition(pos);
            SetPosition(pos);
        }



        public void RegisterToUpdateCenter(IGameUpdate updater)
        {
            UpdateCenter.Ins.RegisterUpdater(updater);
        }

        public void UnregisterFromUpdateCenter(IGameUpdate updater)
        {
            UpdateCenter.Ins.UnregisterUpdater(updater);
        }



    }
}

