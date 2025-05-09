using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSub : DialogMain
{
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !triggered)
        {
            Execute();
            triggered = true;
        }
    }
}
