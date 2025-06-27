using UnityEngine;

public class RandomAudio : AudioMain
{
    [SerializeField] private float interval = 10f;

    private void Start()
    {
        InvokeRepeating(nameof(Play), interval, interval);
    }
}
