using UnityEngine;
using TMPro;  // Necesario para usar TMP_InputField
using UnityEngine.Networking;
using System.Collections;

public class Registro : MonoBehaviour
{
    // Declaramos las variables como TMP_InputField (para usar TextMeshPro)
    public TMP_InputField inputUsername;
    public TMP_InputField inputLastname;
    public TMP_InputField inputEmail;
    public TMP_InputField inputPassword;

    public void EnviarDatos()
    {
        // Obtener los datos de los campos de texto (TMP_InputField)
        string username = inputUsername.text;
        string lastname = inputLastname.text;
        string email = inputEmail.text;
        string password = inputPassword.text;
        string time = Timer.Timevalue.ToString("F2"); // Obtener el tiempo del temporizador

        // URL de tu archivo PHP en el servidor local
        string url = "https://progra251jp.samidareno.com/RegistroUsuario.php";

        // Construcción de los datos para enviar
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("lastname", lastname);
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("time", time);

        // Iniciar la solicitud POST
        StartCoroutine(EnviarSolicitud(url, form));
    }

    IEnumerator EnviarSolicitud(string url, WWWForm form)
    {
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        // Enviamos la solicitud y esperamos la respuesta
        yield return www.SendWebRequest();

        // Comprobamos si la solicitud fue exitosa
        if (www.result == UnityWebRequest.Result.Success)
        {
            // Si la solicitud es exitosa, mostramos el mensaje que regresa desde PHP
            Debug.Log("Registro exitoso: " + www.downloadHandler.text);
        }
        else
        {
            // Si ocurre un error, mostramos el error
            Debug.LogError("Error al registrar: " + www.error);
        }
    }
}
