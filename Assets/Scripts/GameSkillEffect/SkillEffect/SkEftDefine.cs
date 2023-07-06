using System;
using System.Reflection;

namespace GameSkillEffect
{
    public enum EffectTgt
    {
        // 对敌人
        Enemy = 0,
        // 对自己
        Self,
        // 仅队友不包含自己
        TeamWithoutSelf,
        // 对包含自己的队友
        TeamIncludeSelf,
        // 除自己外的其它单位
        AnyOthers,
        // 所有对象包含自己
        All
    }

    public static class SkEftDefine
    {
        #region Skill Effect Type Define

        public const int Damage = 0;




        #endregion


        
    }
}

