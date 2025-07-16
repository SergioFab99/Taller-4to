using UnityEngine;

public class DeadZoneTrigger : MonoBehaviour
{
    // Etiqueta del jugador
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

    //private void PressRKey()
    //{
    //    // Si tienes lógica atada a Input.GetKeyDown(KeyCode.R), puedes moverla aquí.
    //    // Por ejemplo, recargar la escena actual:
    //    UnityEngine.SceneManagement.SceneManager.LoadScene(
    //        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
    //    );
    //}
}
