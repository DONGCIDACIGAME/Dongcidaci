using System.Collections.Generic;
using DongciDaci;
using UnityEngine;

namespace GameSkillEffect
{
    public class FireBullet : CastEffect, IRemoteEffect
    {
        /// <summary>
        /// 这个发射效果的飞行距离
        /// </summary>
        private float _flightDis = 2f;
        public float FlightDis => _flightDis;

        /// <summary>
        /// 子弹的发射初速度
        /// </summary>
        private float _fireSpeed = 0.2f;

        /// <summary>
        /// 发射的entity实体的路径
        /// </summary>
        private string _bulletPrefabPath;

        private Vector3 _entityInitPos;
        private float _entityRotateY;


        /// <summary>
        /// 这个发射效果碰撞后能产生的效果
        /// </summary>
        private List<SkillEffect> _carrySkEfts;

        public override bool InitSkEft(Agent user, SkillEffectData initData, SkEftBaseCfg eftBsCfg)
        {
            Log.Logic(LogLevel.Normal,"Init Fire Bullet -- Start");

            // 1 校验初始化数据的准确性
            if (!initData.strValueDict.ContainsKey("bulletPrefab") || 
                !initData.floatValueDict.ContainsKey("fireSpeed")|| 
                !initData.floatValueDict.ContainsKey("flightDis")||
                !initData.floatValueDict.ContainsKey("initPosX")|| 
                !initData.floatValueDict.ContainsKey("initPosY")|| 
                !initData.floatValueDict.ContainsKey("initPosZ")|| 
                !initData.floatValueDict.ContainsKey("rotateY"))
            {
                return false;
            }

            _eftUser = user;
            _initSkEftData = initData;
            _eftBsCfg = eftBsCfg;
            _flightDis = initData.floatValueDict["flightDis"];
            _fireSpeed = initData.floatValueDict["fireSpeed"];
            _bulletPrefabPath = initData.strValueDict["bulletPrefab"];
            _entityInitPos = new Vector3(initData.floatValueDict["initPosX"], initData.floatValueDict["initPosY"],initData.floatValueDict["initPosZ"]);
            _entityRotateY = initData.floatValueDict["rotateY"];

            // 3 FireBullet时，碰撞体的数据在view上，因此不需要额外初始化ConvexShape
            // 4 init carry skEffects
            _carrySkEfts = new List<SkillEffect>();
            if (initData.subEffects != null && initData.subEffects.Length > 0)
            {
                foreach (var subEftData in initData.subEffects)
                {
                    var newSubEft = GameSkEftPool.Ins.PopWithInit(_eftUser, subEftData);
                    if (newSubEft != null) _carrySkEfts.Add(newSubEft);
                }
            }
            
            return true;
        }

        public override void TriggerSkEft()
        {
            // 1 触发时，调用skEftHandler，查看是否能被触发或效果参数被修改
            if (_eftUser.SkillEftHandler.OnApplyRemoteEft(this) == false)
            {
                // 这个效果被阻止释放
                // recycle and return
                Recycle();
                return;
            }

            // 加载 bullet 
            var rlsBullet = new BulletEntity();
            // 计算飞行方向
            // 飞的方向因该按照实际初始化角度计算
            // ↑↑↑ 和 ↖↑↗的区别
            var rotatedPos = Quaternion.AngleAxis(_eftUser.GetRotation().y, Vector3.up) * _entityInitPos;
            var realRotationY = _eftUser.GetRotation().y + _entityRotateY;
            var initData = new SkEntityInitData
            {
                PrefabPath = _bulletPrefabPath,
                WorldPos = _eftUser.GetPosition() + rotatedPos,
                FlightDir = (Quaternion.AngleAxis(realRotationY, Vector3.up) * Vector3.forward).normalized,
                RotateAngle = new Vector3(0,realRotationY,0),
                FlightDis = _flightDis,
                FlightSpeed = _fireSpeed
            };
            
            rlsBullet.InitSkEftEntity(initData,_carrySkEfts.ToArray());

            // 触发结束后回收这个技能效果
            //Recycle();
        }


        public override void Dispose()
        {
            //this._bindEftEntity = null;
            _eftCollideShape = null;
            _eftUser = null;
            _initSkEftData = null;
            _eftBsCfg = null;
            _carrySkEfts = null;

        }

        public override void Recycle()
        {
            Dispose();
            // put into pool

        }

        public override void RecycleReset()
        {
            Dispose();

        }

        



    }
}


