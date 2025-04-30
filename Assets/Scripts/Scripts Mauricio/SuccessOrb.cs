using UnityEngine;

public class SuccessOrb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Success!");
    }
}
