using System;

namespace GameEngine
{
    public class Singleton<T> where T : new()
    {
        private static T mIns;
        public static T Ins
        {
            get
            {
                if (mIns == null)
                {
                    mIns = default(T) == null ? Activator.CreateInstance<T>() : default(T);
                }

                return mIns;
            }
        }
    }
}
