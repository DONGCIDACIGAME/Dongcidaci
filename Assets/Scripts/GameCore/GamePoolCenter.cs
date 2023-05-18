using GameEngine;

public class GamePoolCenter : Singleton<GamePoolCenter>
{
    public GeneralGamePool<RectEffectExcutor> RectEffectExcutorPool = new GeneralGamePool<RectEffectExcutor>();

    public GeneralGamePool<AgentInputCommand> AgentInputCommandPool = new GeneralGamePool<AgentInputCommand>();

    public GeneralGamePool<ExcutiveComboAction> ExcutiveComboActionPool = new GeneralGamePool<ExcutiveComboAction>();
}