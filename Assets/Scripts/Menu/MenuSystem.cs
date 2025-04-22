using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class MenuSystem : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene(2);
    }
    public void Salir()
    {
        Console.WriteLine("Saliendo del juego...");
        Application.Quit();
    }
}
