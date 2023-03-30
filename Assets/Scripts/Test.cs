using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Animator anim;

    [ContextMenu("RunLeft")]
    public void RunLeft()
    {
        anim.CrossFade("RunLeft", 0.1f);

    }

    [ContextMenu("RunRight")]
    public void RunRight()
    {
        anim.CrossFade("RunRight", 0.1f);
    }
}
