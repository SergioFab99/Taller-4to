using UnityEngine;

public class Lenguileta : MonoBehaviour
{
    public float pullForce = 15f;
    public float lifetime = 3f;

    private Transform frog;

    public void Initialize(Transform frogOrigin)
    {
        frog = frogOrigin;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null && frog != null)
            {
                Vector3 pullDir = (frog.position - other.transform.position).normalized;
                playerRb.AddForce(pullDir * pullForce, ForceMode.Impulse);
            }

            Destroy(gameObject);
        }
    }
}
