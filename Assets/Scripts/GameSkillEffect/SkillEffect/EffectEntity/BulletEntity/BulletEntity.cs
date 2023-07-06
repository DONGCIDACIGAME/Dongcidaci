using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

namespace GameSkillEffect
{
    public class BulletEntity : SkEftEntity
    {
        private BulletEntityView _bulletView;

        public override void InitSkEftEntity(string eftEntityPrefab)
        {
            
            var go = PrefabUtil.LoadPrefab(eftEntityPrefab, SkEftDefine.SkEftEntityNode, "Load Bullet Prefab");
            if (go != null)
            {
                BulletEntityView bulletView = go.GetComponent<BulletEntityView>();
                BindBulletView(bulletView);
            }


        }

        private void BindBulletView(BulletEntityView bulletView)
        {
            BindMapEntityView(bulletView);
            _bulletView = bulletView;

        }


        protected override MyColliderType ColliderType => MyColliderType.Collider_SkBullet;

        public override int GetEntityType()
        {
            return EntityTypeDefine.SkEftBullet;
        }

        
    }
}

