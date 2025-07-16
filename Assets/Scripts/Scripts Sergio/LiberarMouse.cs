using UnityEngine;

public class LiberarMouse: MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;

        Cursor.lockState = CursorLockMode.None;
    }
}
