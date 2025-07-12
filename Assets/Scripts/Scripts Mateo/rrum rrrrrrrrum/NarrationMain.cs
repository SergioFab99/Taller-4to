using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationMain : MonoBehaviour
{
    [SerializeField] protected List<Narration> narrations;
    [SerializeField] protected float delayBetweenNarrations;

    protected void Execute()
    {
        if (NarrationManager.Instance != null)
        {
            NarrationManager.Instance.SetUp(narrations, delayBetweenNarrations);
        }
        else
        {
            Debug.LogWarning("NarrationManager.Instance not found. Make sure NarrationManager is in the scene.");
        }
    }
}