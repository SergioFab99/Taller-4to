using UnityEngine;

public class ZoneAudioi : AudioMain
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            Play();
            triggered = true;
        }
    }
}
