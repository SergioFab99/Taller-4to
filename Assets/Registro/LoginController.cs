using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoginController : MonoBehaviour
{
    [Header("Campos de login")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    [Header("Botón Jugar")]
    public Button jugarButton;

    private string escenaDeJuego = "Nivel 1"; // Nombre de la escena de gameplay

    private void Start()
    {
        if (jugarButton != null)
        {
            jugarButton.onClick.AddListener(Jugar);
        }
    }

    public void Jugar()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("Faltan datos para el login.");
            return;
        }

        StartCoroutine(EnviarLogin(email, password));
    }

    IEnumerator EnviarLogin(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password); // El campo correcto es "password" y no "contraseña" en el PHP

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/koi/loginkoi.php", form))  // Cambié la URL aquí
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error en la conexión: " + www.error);
            }
            else
            {
                string respuesta = www.downloadHandler.text;
                Debug.Log("Respuesta del servidor: " + respuesta);

                // Asegúrate de que la respuesta contiene un campo success
                if (respuesta.Contains("\"success\":true"))
                {
                    Debug.Log("Login exitoso, cargando la escena: " + escenaDeJuego);
                    SceneManager.LoadScene(escenaDeJuego);
                }
                else
                {
                    Debug.LogWarning("Login fallido, revisa email o contraseña.");
                }
            }
        }
    }
}
