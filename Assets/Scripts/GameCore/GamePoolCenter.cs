using GameEngine;

public class GamePoolCenter : Singleton<GamePoolCenter>
{
    public GeneralGamePool<RectEffectExcutor> RectEffectExcutorPool = new GeneralGamePool<RectEffectExcutor>();

    public GeneralGamePool<AgentInputCommand> AgentInputCommandPool = new GeneralGamePool<AgentInputCommand>();

    public GeneralGamePool<TriggeredComboAction> TriggeredComboActionPool = new GeneralGamePool<TriggeredComboAction>();
}