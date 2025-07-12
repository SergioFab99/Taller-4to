using UnityEngine;

public class ZoneNar : NarrationMain
{
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            Execute();
            triggered = true;
        }
    }
}
