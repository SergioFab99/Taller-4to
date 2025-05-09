using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogMain : MonoBehaviour
{
    [SerializeField] protected List<string> texts;
    [SerializeField] protected float delay;
    protected void Execute()
    {
        TextManager.Instance.SetUp(texts, delay);
    }

}
