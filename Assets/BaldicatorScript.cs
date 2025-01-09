using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldicatorScript : MonoBehaviour
{
    public Animator baldicator;
    public static Animator current;
    private void Awake()
    {
        current = baldicator;
    }

    public static void Trigger(string t)
    {
        current.SetTrigger(t);
    }
}
