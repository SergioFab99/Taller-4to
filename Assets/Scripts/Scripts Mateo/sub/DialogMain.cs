using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogMain : MonoBehaviour
{
    [SerializeField] protected List<string> texts;
    [SerializeField] protected float delay;
    
    protected void Execute()
    {
        if (TextManager.Instance != null)
        {
            TextManager.Instance.SetUp(texts, delay);
        }
        else
        {
            Debug.LogWarning("TextManager.Instance no encontrado. Asegúrate de que TextManager esté en la escena.");
        }
    }
}