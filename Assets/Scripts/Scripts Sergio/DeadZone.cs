using UnityEngine;

public class DeadZoneTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    public MiBombo2 dead;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            dead.FuckingDie();
            Debug.Log("dead");
        }
    }
}
