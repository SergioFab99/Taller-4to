using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BonecaAmbalabu : MonoBehaviour
{
    public Transform player;
    public Rigidbody rb;

    public float hopForce = 15f;
    public float hopUp = 7f;
    public float hopCd = 2f;
    public float detectionRadius = 15f;
    public float patrolRadius = 25f;
    private Vector3 spawnPoint;
    public string groundTag = "Ground";
    private bool isGrounded = true;
    private bool isHopping = false;

    public float range = 6f;
    public float cooldown = 4f;
    public float pull = 25f;
    public float delay = 0.2f;
    private bool ready = true;

    public Renderer sapoTruco;
    private Color originalColor;
    public float warning = 0.5f;


    private void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        spawnPoint = transform.position;
        if (sapoTruco != null)
        {
            originalColor = sapoTruco.material.color;
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        float distanceFromSpawn = Vector3.Distance(transform.position, spawnPoint);

        if (distanceToPlayer <= detectionRadius && distanceFromSpawn <= patrolRadius)
        {
            Vector3 dir = player.position - transform.position;
            dir.y = 0f;

            if (dir.magnitude > 0.1f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
            }

            if (distanceToPlayer <= range && ready)
            {
                StartCoroutine(TongueAttack());
            }
        }
    }

    public IEnumerator HopRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);

            float distanceToPlayer = Vector3.Distance(player.position, transform.position);
            float distanceFromSpawn = Vector3.Distance(transform.position, spawnPoint);

            if (isGrounded && !isHopping && distanceToPlayer <= detectionRadius && distanceFromSpawn <= patrolRadius)
            {
                isHopping = true;
                HopTowardPlayer();
            }
        }
    }

    private void HopTowardPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 hop = new Vector3(direction.x * hopForce, hopUp, direction.z * hopForce);

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(hop, ForceMode.Impulse);
    }

    private IEnumerator TongueAttack()
    {
        ready = false;

        if (sapoTruco != null)
        {
            sapoTruco.material.color = Color.red;
        }

        yield return new WaitForSeconds(warning); 

        if (player != null)
        {
            Vector3 pullDir = (transform.position - player.position).normalized;
            Rigidbody playerRb = player.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                playerRb.AddForce(pullDir * pull, ForceMode.Impulse);
            }
        }

        if (sapoTruco != null)
        {
            sapoTruco.material.color = originalColor;
        }

        yield return new WaitForSeconds(cooldown);
        ready = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = true;
            isHopping = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = false;
        }
    }
}
