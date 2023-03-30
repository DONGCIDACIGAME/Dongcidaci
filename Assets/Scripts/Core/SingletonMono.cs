using System;
using UnityEngine;

namespace GameEngine
{
    /// <summary>
    /// DO NOT REWRITE AWAKE FUNCTION!!!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T mIns;

        public static T Ins 
        {
            get
            {
                return mIns;
            }
        }

        void Awake()
        {
            Type type = GetType();
            if(mIns != null)
            {
                Log.Error(LogLevel.Critical, "{0}: Ins Set to Different instance is not valid!", type);
                return;
            }

            T _ins = gameObject.GetComponent(type) as T;
            if(_ins != null)
            {
                mIns = _ins;
                OnAwake();
            }
        }

        protected virtual void OnAwake()
        {

        }
    }
}
