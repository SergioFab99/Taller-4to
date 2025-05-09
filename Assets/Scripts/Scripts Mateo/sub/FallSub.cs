using UnityEngine;

public class FallSub : DialogMain
{
    public float minAscentThreshold = 10f;
    public float minFallThreshold = 5f;
    public float cooldownDuration = 5f;
    private float highestPoint;
    private bool isAscending = false;
    private bool canTriggerMessage = true;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        highestPoint = transform.position.y;
    }

    private void Update()
    {
        if (rb.linearVelocity.y > 0f && transform.position.y > highestPoint)
        {
            highestPoint = transform.position.y;
            isAscending = true;
        }
        else if (transform.position.y < highestPoint - minFallThreshold && isAscending && canTriggerMessage)
        {
            Execute();
            isAscending = false;
            canTriggerMessage = false;
            Invoke(nameof(ResetMessageTrigger), cooldownDuration);
        }
        else if (transform.position.y >= highestPoint && !isAscending)
        {
            isAscending = true;
        }
    }

    private void ResetMessageTrigger()
    {
        canTriggerMessage = true; 
    }
}
