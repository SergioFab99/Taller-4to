using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSub : DialogMain
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Execute();
        }
    }
}
