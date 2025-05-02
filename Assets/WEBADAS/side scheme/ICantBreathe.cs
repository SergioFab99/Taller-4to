using UnityEngine;

public class ICantBreathe : MonoBehaviour
{
    public float refillAmount = 100f;

    private void OnTriggerEnter(Collider other)
    {
        bar bb = FindFirstObjectByType<bar>();
        if (bb != null)
        {
            bb.RefillOxygen(refillAmount);
            Destroy(gameObject); 
        }
    }
}
