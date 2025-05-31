using UnityEngine;

public class Aneurysm : MonoBehaviour
{
    public Unity.Cinemachine.CinemachineCamera menuCamera;
    public Unity.Cinemachine.CinemachineCamera playerFollowCamera;
    public Transform fish;
    public ThirdPersonCameraController cam;
    private Unity.Cinemachine.CinemachineBrain cinemachineBrain;
    public GameObject playButton;

    void Start()
    {
        cinemachineBrain = Camera.main.GetComponent<Unity.Cinemachine.CinemachineBrain>();
        menuCamera.Priority = 1;
        playerFollowCamera.Priority = 0;
        cam.enabled = false;
        fish.GetComponent<Jump>().enabled = false;
    }

    public void Click()
    {
        playerFollowCamera.Priority = 1;
        menuCamera.Priority = 0;
        playButton.SetActive(false);

        Invoke(nameof(GetMeOut), 1f); 
    }

    void GetMeOut()
    {
        fish.GetComponent<Jump>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam.enabled = true;
        Debug.Log("enable movement xdddd");
    }
}
