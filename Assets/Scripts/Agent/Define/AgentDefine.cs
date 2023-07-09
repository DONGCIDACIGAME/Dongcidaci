public static class AgentDefine
{
    public const int AgentType_NotDefine = 0;
    public const int AgentType_Hero = 1;
    public const int AgentType_Monster = 2;
    public const int AgentType_NPC = 3;


    public static TypeWithDesc[] ALL_AGENT_TYPES = new TypeWithDesc[]
    {
        new TypeWithDesc(AgentType_NotDefine, "Undefine"),
        new TypeWithDesc(AgentType_Hero, "Hero"),
        new TypeWithDesc(AgentType_Monster, "Monster"),
        new TypeWithDesc(AgentType_NPC, "NPC"),
    };
}
