using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class RegisterUser1 : MonoBehaviour
{
    public static string username;
    public static string lastname;
    public static string email;
    public static string password;

    public TMP_InputField inputUsername;
    public TMP_InputField inputLastname;
    public TMP_InputField inputEmail;
    public TMP_InputField inputPassword;

    public void EnviarDatos()
    {
        username = inputUsername.text;
        lastname = inputLastname.text;
        email = inputEmail.text;
        password = inputPassword.text;

        string url = "https://progra251jp.samidareno.com/insertkoi.php";

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("lastname", lastname);
        form.AddField("email", email);
        form.AddField("password", password);

        StartCoroutine(EnviarSolicitud(url, form));
    }

    IEnumerator EnviarSolicitud(string url, WWWForm form)
    {
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Registro exitoso: " + www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error al registrar: " + www.error);
        }
    }
}
