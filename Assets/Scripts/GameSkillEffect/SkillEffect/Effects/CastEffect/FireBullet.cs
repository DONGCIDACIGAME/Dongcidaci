using System.Collections.Generic;
using DongciDaci;

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
        public float FireLowSpeed => 0.8f * _fireSpeed;

        
        /// <summary>
        /// 发射的entity实体的路径
        /// </summary>
        private string _bulletPrefabPath;


        /// <summary>
        /// 这个发射效果碰撞后能产生的效果
        /// </summary>
        private List<SkillEffect> _carrySkEfts;

        public override bool InitSkEft(Agent user, SkillEffectData initData, SkEftBaseCfg eftBsCfg)
        {
            // 1 校验初始化数据的准确性
            if (!initData.strValueDict.ContainsKey("bulletPrefab") || 
                !initData.floatValueDict.ContainsKey("fireSpeed")|| 
                !initData.floatValueDict.ContainsKey("flightDis"))
            {
                return false;
            }

            this._eftUser = user;
            this._initSkEftData = initData;
            this._eftBsCfg = eftBsCfg;
            this._flightDis = initData.floatValueDict["flightDis"];
            this._fireSpeed = initData.floatValueDict["fireSpeed"];
            this._bulletPrefabPath = initData.strValueDict["bulletPrefab"];

            // 2 加载bullet的实体prefab
            //_bindEftEntity = new BulletEntity();
            //_bindEftEntity.InitSkEftEntity(_bulletPrefabPath);

            // 3 FireBullet时，碰撞体的数据在view上，因此不需要额外初始化ConvexShape
            // 4 init carry skEffects
            _carrySkEfts = new List<SkillEffect>();
            if (initData.subEffects == null || initData.subEffects.Length == 0) return false; 
            foreach (var subEftData in initData.subEffects)
            {
                var newSubEft = GameSkEftPool.Ins.PopWithInit(_eftUser,subEftData);
                if (newSubEft != null) _carrySkEfts.Add(newSubEft);
            }

            return true;
        }

        public override void TriggerSkEft()
        {
            // 1 触发时，调用skEftHandler，查看是否能被触发或效果参数被修改
            if (!_eftUser.SkillEftHandler.OnApplyRemoteEft(this))
            {
                // 这个效果被阻止释放
                // recycle and return
                Recycle();
                return;
            }

            // 加载 bullet 
            var rlsBullet = new BulletEntity();

            //!!!这里的bullet初始化缺少参数
            rlsBullet.InitSkEftEntity(_bulletPrefabPath);

        }


        public override void Dispose()
        {
            //this._bindEftEntity = null;
            this._eftCollideShape = null;
            this._eftUser = null;
            this._initSkEftData = null;
            this._eftBsCfg = null;
            this._carrySkEfts = null;

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


