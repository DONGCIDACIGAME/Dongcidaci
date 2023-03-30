namespace GameEngine
{
    /// <summary>
    /// 定义一些基础的scope，其他的scope都是从这些scope派生出来的，以保证scope的结构
    /// </summary>
    public partial class GameScope
    {
        public static GameScope RootScope = new GameScope("ROOT", null);
        public static GameScope UIScope = RootScope.CreateChildScope("UI");
        public static GameScope ModuleScope = RootScope.CreateChildScope("MODULE");
        public static GameScope SceneScope = RootScope.CreateChildScope("SCENE");
        public static GameScope LogicScope = RootScope.CreateChildScope("LOGIC");
    }
}


