using System.Collections.Generic;

public static class AgentStatusDefine
{
    public static string EMPTY                          = "Empty";
    public static string IDLE                           = "Idle";
    public static string RUN                            = "Run";
    public static string DASH                           = "Dash";
    public static string INSTANT_ATTACK                 = "Instant_Attack";
    public static string METER_ATTACK                   = "Meter_Attack";
    public static string CHARGING                       = "Charging";
    public static string CHARGING_ATTACK                = "Charging_Attack";
    public static string TRANSITION                     = "Transition";
    public static string BEHIT                          = "BeHit";
    public static string DEAD                           = "Dead";

    public static List<string> ALL_STATUS = new List<string>
    {
        EMPTY,
        IDLE,
        RUN,
        DASH,
        INSTANT_ATTACK,
        CHARGING,
        CHARGING_ATTACK,
        BEHIT,
        DEAD
    };

    public static bool IsResetComboStatus(string statusName)
    {
        //if (statusName == IDLE || statusName == RUN || statusName == TRANSFER
        //    || statusName == BEHIT || statusName == DEAD)
        //    return true;

        //if (statusName == IDLE || statusName == BEHIT || statusName == DEAD)
        //   return true;

        if (statusName == BEHIT || statusName == DEAD)
            return true;

        return false;
    }
}
