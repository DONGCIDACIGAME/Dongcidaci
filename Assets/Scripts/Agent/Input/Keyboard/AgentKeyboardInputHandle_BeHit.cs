public class AgentKeyboardInputHandle_BeHit : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_BeHit(Hero hero) : base(hero)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_BeHit;
    }


    public override void OnUpdate(float deltaTime)
    {
        // 重写该方法
        // 不要删除！！！
        // 受击状态下无法接受输入
    }
}
