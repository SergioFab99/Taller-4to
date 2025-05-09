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

    public float range = 6f;
    public float cooldown = 4f;
    public float pull = 25f;
    public float delay = 0.2f;


    public GameObject tonguePrefab;
    public Transform tongueSpawn;
    public float projectileSpeed = 30f;
    public Transform attackTarget; 
    public float attackRadius = 2f;

    public AudioClip attackSound;
    public float attackVolume = 1f;
    private AudioSource audioSource;

    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    private void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        spawnPoint = transform.position;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = attackVolume;

        if (attackTarget != null)
        {
            Vector3 targetPos = new Vector3(attackTarget.position.x, transform.position.y, attackTarget.position.z);
            transform.LookAt(targetPos);
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        float distanceFromSpawn = Vector3.Distance(transform.position, spawnPoint);

        if (distanceToPlayer <= detectionRadius && distanceFromSpawn <= patrolRadius)
        {

            if (distanceToPlayer <= range && Time.time > nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                FireTongue();
            }
        }
    }

    private void FireTongue()
    {
        // Reproducir sonido de ataque
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        if (tonguePrefab != null && tongueSpawn != null && attackTarget != null)
        {
            GameObject projectile = Instantiate(tonguePrefab, tongueSpawn.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 randomDirection = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f)
                ).normalized;

                Vector3 targetPoint = attackTarget.position + randomDirection * Random.Range(0f, attackRadius);

                Vector3 dir = (targetPoint - tongueSpawn.position).normalized;
                rb.linearVelocity = dir * projectileSpeed;

                projectile.GetComponent<Lenguileta>().Initialize(transform);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackTarget.position, attackRadius);
        }
    }
}