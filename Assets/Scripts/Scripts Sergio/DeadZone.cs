using UnityEngine;

public class DeadZoneTrigger : MonoBehaviour
{
    // Etiqueta del jugador
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Simula presionar la tecla R
            PressRKey();
        }
    }

    private void PressRKey()
    {
        // Si tienes lógica atada a Input.GetKeyDown(KeyCode.R), puedes moverla aquí.
        // Por ejemplo, recargar la escena actual:
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
