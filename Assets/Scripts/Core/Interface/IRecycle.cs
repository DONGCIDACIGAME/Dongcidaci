namespace GameEngine
{
    public interface IRecycle
    {
        /// <summary>
        /// 回收相关资源；清空一些数据并放入缓存池方便复用
        /// </summary>
        void Recycle();

        /// <summary>
        /// 完全释放相关的引用
        /// </summary>
        void RecycleReset();
    }

}

