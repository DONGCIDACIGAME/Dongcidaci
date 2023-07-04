using GameEngine;
using GameSkillEffect;

public class GamePoolCenter : Singleton<GamePoolCenter>
{
    // Changed by weng 0704
    public GeneralGamePool<ComboEffectExcutor> ComboEffectExcutorPool = new GeneralGamePool<ComboEffectExcutor>();
    
    public GeneralGamePool<AnimMovementExcutor> MovementExcutorPool = new GeneralGamePool<AnimMovementExcutor>();

    public GeneralGamePool<AgentInputCommand> AgentInputCommandPool = new GeneralGamePool<AgentInputCommand>();

    public GeneralGamePool<TriggeredComboStep> TriggeredComboActionPool = new GeneralGamePool<TriggeredComboStep>();

    public GeneralGamePool<GameEventListener> GameEventLIstenerPool = new GeneralGamePool<GameEventListener>();

    // changed by weng 0626
    public GeneralGamePool<GameCollider2D> GameCollider2DPool = new GeneralGamePool<GameCollider2D>();
    public GeneralGamePool<ConvexCollider2D> ConvexCollider2DPool = new GeneralGamePool<ConvexCollider2D>();

    // added by weng 0703
    public GeneralGamePool<AgentSkEftHandler> AgtSkEftHandlerPool = new GeneralGamePool<AgentSkEftHandler>();


}