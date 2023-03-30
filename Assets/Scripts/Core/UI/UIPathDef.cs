using System.Collections.Generic;

public static class UIPathDef
{
    public const string UI_ROOT = "_UI_ROOT";
    public const string UI_POOL_NODE = "_UI_POOL";

    public const string UI_LAYER_BOTTOM_STATIC = "_LAYER_BOTTOM/_STATIC";
    public const string UI_LAYER_BOTTOM_DYNAMIC = "_LAYER_BOTTOM/_DYNAMIC";
    public const string UI_LAYER_NORMAL_STATIC = "_LAYER_NORMAL/_STATIC";
    public const string UI_LAYER_NORMAL_DYNAMIC = "_LAYER_NORMAL/_DYNAMIC";
    public const string UI_LAYER_MSG_STATIC = "_LAYER_MSG/_STATIC";
    public const string UI_LAYER_MSG_DYNAMIC = "_LAYER_MSG/_DYNAMIC";
    public const string UI_LAYER_TOP_STATIC = "_LAYER_TOP/_STATIC";
    public const string UI_LAYER_TOP_DYNAMIC = "_LAYER_TOP/_DYNAMIC";
    public const string UI_LAYER_CONSOLE = "_LAYER_CONSOLE";

    public static List<string> ALL_UI_LAYER = new List<string>()
    {
        UI_LAYER_BOTTOM_STATIC,
        UI_LAYER_BOTTOM_DYNAMIC,
        UI_LAYER_NORMAL_STATIC,
        UI_LAYER_NORMAL_DYNAMIC,
        UI_LAYER_MSG_STATIC,
        UI_LAYER_MSG_DYNAMIC,
        UI_LAYER_TOP_STATIC,
        UI_LAYER_TOP_DYNAMIC,
        UI_LAYER_CONSOLE,
    };
}