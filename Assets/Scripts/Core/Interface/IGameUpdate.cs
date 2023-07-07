namespace GameEngine
{
    public interface IGameUpdate
    {
        void OnUpdate(float deltaTime);
    }

    /// <summary>
    /// Added by weng 0707
    /// 由UpdateCenter驱动的update方法
    /// </summary>
    public interface IUpdateCenterDrive : IGameUpdate
    {
        public void RegisterToUpdateCenter(IGameUpdate updater);
        public void UnregisterFromUpdateCenter(IGameUpdate updater);

    }


}


