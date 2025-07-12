using UnityEngine;

public class InteractNar : NarrationMain
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Execute();
        }
    }
}