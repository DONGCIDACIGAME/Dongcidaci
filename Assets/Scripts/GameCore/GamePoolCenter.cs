using GameEngine;

public class GamePoolCenter : Singleton<GamePoolCenter>
{
    public GeneralGamePool<RectEffectExcutor> RectEffectExcutorPool = new GeneralGamePool<RectEffectExcutor>();
    
    public GeneralGamePool<MovementExcutor> MovementExcutorPool = new GeneralGamePool<MovementExcutor>();

    public GeneralGamePool<AgentInputCommand> AgentInputCommandPool = new GeneralGamePool<AgentInputCommand>();

    public GeneralGamePool<TriggeredComboStep> TriggeredComboActionPool = new GeneralGamePool<TriggeredComboStep>();
}