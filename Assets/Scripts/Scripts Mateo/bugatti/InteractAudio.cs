using UnityEngine;

public class InteractAudio : AudioMain
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Play();
        }
    }
}
