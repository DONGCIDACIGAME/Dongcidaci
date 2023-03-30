namespace GameEngine
{
    /// <summary>
    /// ����һЩ������scope��������scope���Ǵ���Щscope���������ģ��Ա�֤scope�Ľṹ
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


