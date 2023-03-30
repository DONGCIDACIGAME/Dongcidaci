using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputControlCenter
{
    public static PlayerInputControl AttackInputCtl = new PlayerInputControl();


    public static void Register()
    {
        InputManager.Ins.RegisterInputControl(AttackInputCtl);
    }
}
