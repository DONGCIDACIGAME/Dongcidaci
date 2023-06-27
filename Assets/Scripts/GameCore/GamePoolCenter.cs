using GameEngine;

public class GamePoolCenter : Singleton<GamePoolCenter>
{
    public GeneralGamePool<RectEffectExcutor> RectEffectExcutorPool = new GeneralGamePool<RectEffectExcutor>();
    
    public GeneralGamePool<AnimMovementExcutor> MovementExcutorPool = new GeneralGamePool<AnimMovementExcutor>();

    public GeneralGamePool<AgentInputCommand> AgentInputCommandPool = new GeneralGamePool<AgentInputCommand>();

    public GeneralGamePool<TriggeredComboStep> TriggeredComboActionPool = new GeneralGamePool<TriggeredComboStep>();

    public GeneralGamePool<GameEventListener> GameEventLIstenerPool = new GeneralGamePool<GameEventListener>();

    // changed by weng 0626
    public GeneralGamePool<GameCollider2D> GameCollider2DPool = new GeneralGamePool<GameCollider2D>();
    public GeneralGamePool<EllipseCollider2D> EllipseCollider2DPool = new GeneralGamePool<EllipseCollider2D>();

}