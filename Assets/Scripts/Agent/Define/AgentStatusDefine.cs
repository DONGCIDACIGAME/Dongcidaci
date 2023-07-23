using System.Collections.Generic;

public static class AgentStatusDefine
{
    public static string EMPTY = "Empty";
    public static string IDLE = "Idle";
    public static string RUN = "Run";
    public static string RUN_METER = "RunMeter";
    public static string DASH = "Dash";
    public static string TRANSFER = "Transfer";
    public static string ATTACK = "Attack";
    public static string INSTANT_ATTACK = "InstantAttack";
    public static string BEHIT = "BeHit";
    public static string DEAD = "Dead";

    public static List<string> ALL_STATUS = new List<string>
    {
        EMPTY,
        IDLE,
        RUN,
        DASH,
        TRANSFER,
        ATTACK,
        BEHIT,
        DEAD
    };

    public static bool IsResetComboStatus(string statusName)
    {
        //if (statusName == IDLE || statusName == RUN || statusName == TRANSFER
        //    || statusName == BEHIT || statusName == DEAD)
        //    return true;

        if (statusName == IDLE || statusName == TRANSFER
    || statusName == BEHIT || statusName == DEAD)
            return true;

        return false;
    }
}
