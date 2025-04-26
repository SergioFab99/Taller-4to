using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovimientoPlataforma: MonoBehaviour
{
    private Vector3 startPosition;
    public float range = 3f;
    public float speed = 2f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, range * 2) - range;
        transform.position = new Vector3(startPosition.x, startPosition.y + offset, startPosition.z);
    }
}