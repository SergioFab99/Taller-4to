using UnityEngine;

public class camcam2: MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationSpeed = 5f;

    public PlayerInputHandler player;
    void Update()
    {
      if(player.isFuckingDead == false)
        {
            Vector3 lookDirection = cameraTransform.forward;
            lookDirection.y = 0f;

            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
