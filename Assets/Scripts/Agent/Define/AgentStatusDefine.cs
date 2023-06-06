public static class AgentStatusDefine
{
    public static string EMPTY = "Empty";
    public static string IDLE = "Idle";
    public static string WALK = "Walk";
    public static string RUN = "Run";
    public static string DASH = "Dash";
    public static string TRANSFER = "Transfer";
    public static string ATTACK = "Attack";
    public static string BE_HIT = "Be_Hit";
    public static string DEAD = "Dead";

    public static bool IsResetComboStatus(string statusName)
    {
        if (statusName == IDLE || statusName == RUN || statusName == TRANSFER
            || statusName == BE_HIT || statusName == DEAD)
            return true;

        return false;
    }
}