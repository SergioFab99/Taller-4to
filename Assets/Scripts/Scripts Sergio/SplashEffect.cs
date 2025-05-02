using UnityEngine;

public class SplashEffect : MonoBehaviour
{
    public ParticleSystem splashEffectPrefab; 
    public AudioClip splashSound;             
    public float collisionVelocityThreshold = 1f;          

    private AudioSource audioSource;
    public float soundVolume = 0.5f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") &&
            collision.relativeVelocity.magnitude > collisionVelocityThreshold)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                TriggerSplash(contact.point, contact.normal);
            }
        }
    }

    void TriggerSplash(Vector3 position, Vector3 normal)
    {
        if (splashEffectPrefab == null) return;

        ParticleSystem splash = Instantiate(splashEffectPrefab, position, Quaternion.LookRotation(normal));

        var main = splash.main;
        main.loop = false;
        main.startLifetime = 0.8f;
        main.startSpeed = new ParticleSystem.MinMaxCurve(4f, 7f);
        main.gravityModifier = 1f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = splash.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0f, 20)
        });

        var shape = splash.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.rotation = Vector3.zero;
        shape.angle = 20f;
        shape.radius = 0.1f;

        if (splashSound != null)
        {
            audioSource.PlayOneShot(splashSound, soundVolume);
        }

        Destroy(splash.gameObject, main.startLifetime.constantMax + 0.5f);
    }
}

