namespace GameEngine
{
    public delegate void Callback();
    public delegate void Callback<in T1>(T1 arg0);
    public delegate void Callback<in T1, in T2>(T1 arg0, T2 arg1);
    public delegate void Callback<in T1, in T2, in T3>(T1 arg0, T2 arg1, T3 arg2);
    public delegate void Callback<in T1, in T2, in T3, in T4>(T1 arg0, T2 arg1, T3 arg3, T4 arg4);
    public delegate void Callback<in T1, in T2, in T3, in T4, in T5>(T1 arg0, T2 arg1, T3 arg3, T4 arg4, T5 arg5);
    public delegate void Callback<in T1, in T2, in T3, in T4, in T5, in T6>(T1 arg0, T2 arg1, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
    public delegate void Callback<in T1, in T2, in T3, in T4, in T5, in T6, in T7>(T1 arg0, T2 arg1, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
}

