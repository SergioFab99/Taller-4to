using System.Collections;
using UnityEngine;

public class PeriodicNar : NarrationMain
{
    public PlayerInputHandler player;
    public float interval = 45f;

    private bool isRunning = false;

    private void Start()
    {
        StartPeriodicExecution();
    }

    public void StartPeriodicExecution()
    {
        if (!isRunning && !player.isFuckingDead)
        {
            isRunning = true;
            StartCoroutine(PeriodicExecution());
        }
    }

    public void StopPeriodicExecution()
    {
        isRunning = false;
    }

    private IEnumerator PeriodicExecution()
    {
        while (isRunning && !player.isFuckingDead)
        {
            Execute();
            yield return new WaitForSeconds(interval);
        }
    }

    private void Update()
    {
        if (player.isFuckingDead)
        {
            StopPeriodicExecution();
        }
        else if (!isRunning)
        {
            StartPeriodicExecution();
        }
    }
}
