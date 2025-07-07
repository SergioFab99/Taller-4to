using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro; // Importa TextMeshPro

[System.Serializable]
public class LoginResponse
{
    public string mensaje;
    public int usuario_id;
    public string nombre;
    public string apellido;
    public string email;
}

public class LoginManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField emailInput;   // Campo para el email (TextMeshPro)
    public TMP_InputField passwordInput;   // Campo para la contraseña (TextMeshPro)
    public Button loginButton;         // Botón para iniciar sesión

    [Header("Settings")]
    public string loginURL = "http://localhost/koi/loginkoi.php"; // Cambia por tu URL
    public string gameSceneName = "Menu"; // Nombre de la escena del juego

    private void Start()
    {
        // Configurar el botón de login
        if (loginButton != null)
        {
            loginButton.onClick.AddListener(OnLoginButtonClicked);
        }
    }

    public void OnLoginButtonClicked()
    {
        // Validar campos
        if (string.IsNullOrEmpty(emailInput.text) || string.IsNullOrEmpty(passwordInput.text))
        {
            Debug.LogError("Por favor, complete todos los campos.");
            return;
        }

        // Iniciar proceso de login
        StartCoroutine(LoginCoroutine());
    }

    private IEnumerator LoginCoroutine()
    {
        // Mostrar estado de carga en consola (puedes agregar algo visual si deseas)
        Debug.Log("Iniciando sesión...");

        // Deshabilitar botón para evitar múltiples clics
        if (loginButton != null)
            loginButton.interactable = false;

        // Crear formulario para enviar datos
        WWWForm form = new WWWForm();
        form.AddField("email", emailInput.text.Trim());
        form.AddField("password", passwordInput.text);

        // Crear request
        UnityWebRequest request = UnityWebRequest.Post(loginURL, form);

        // Enviar request
        yield return request.SendWebRequest();

        // Procesar respuesta
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Respuesta del servidor: " + responseText);

            // Intentar procesar la respuesta JSON
            LoginResponse response = null;
            try
            {
                response = JsonUtility.FromJson<LoginResponse>(responseText);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al procesar respuesta del servidor: " + e.Message);
                // Rehabilitar botón antes de salir
                if (loginButton != null)
                    loginButton.interactable = true;
                yield break;
            }

            if (response != null && response.mensaje == "Éxito")
            {
                // Login exitoso
                Debug.Log("¡Login exitoso! Bienvenido " + response.nombre);

                // Guardar datos del usuario para usar en otras escenas
                PlayerPrefs.SetString("LoggedUsername", response.nombre);
                PlayerPrefs.SetInt("LoggedUserID", response.usuario_id);
                PlayerPrefs.SetString("LoggedUserLastname", response.apellido);
                PlayerPrefs.SetString("LoggedUserEmail", response.email);
                PlayerPrefs.SetInt("IsLoggedIn", 1);
                PlayerPrefs.Save();

                // Esperar un momento y cambiar de escena
                yield return new WaitForSeconds(1.5f);
                SceneManager.LoadScene(gameSceneName);
            }
            else
            {
                // Login fallido
                string mensaje = response != null ? response.mensaje : "Error desconocido";
                Debug.LogError(mensaje);
            }
        }
        else
        {
            // Error de conexión
            Debug.LogError("Error de conexión: " + request.error);
        }

        // Rehabilitar botón
        if (loginButton != null)
            loginButton.interactable = true;
    }

    // Método para logout (puedes usarlo en otras escenas)
    public void Logout()
    {
        PlayerPrefs.DeleteKey("LoggedUsername");
        PlayerPrefs.DeleteKey("LoggedUserID");
        PlayerPrefs.DeleteKey("LoggedUserLastname");
        PlayerPrefs.DeleteKey("LoggedUserEmail");
        PlayerPrefs.DeleteKey("IsLoggedIn");
        PlayerPrefs.Save();

        SceneManager.LoadScene("LoginScene");
    }

    // Métodos estáticos para acceder a los datos del usuario desde otras escenas
    public static bool IsUserLoggedIn()
    {
        return PlayerPrefs.GetInt("IsLoggedIn", 0) == 1;
    }

    public static string GetCurrentUsername()
    {
        return PlayerPrefs.GetString("LoggedUsername", "");
    }

    public static int GetCurrentUserID()
    {
        return PlayerPrefs.GetInt("LoggedUserID", 0);
    }

    public static string GetCurrentUserLastname()
    {
        return PlayerPrefs.GetString("LoggedUserLastname", "");
    }

    public static string GetCurrentUserEmail()
    {
        return PlayerPrefs.GetString("LoggedUserEmail", "");
    }

    // Método para limpiar los campos (opcional)
    public void ClearFields()
    {
        if (emailInput != null) emailInput.text = "";
        if (passwordInput != null) passwordInput.text = "";
    }
}
