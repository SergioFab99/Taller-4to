using UnityEngine;

public class SplashEffect : MonoBehaviour
{
    public ParticleSystem splashEffectPrefab; // Tu prefab personalizado arrastrado
    public float collisionVelocityThreshold = 1f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && collision.relativeVelocity.magnitude > collisionVelocityThreshold)
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

        // FORZAR EFECTO PARABÓLICO
        var main = splash.main;
        main.loop = false;
        main.startLifetime = 0.8f;
        main.startSpeed = new ParticleSystem.MinMaxCurve(4f, 7f); // Sube rápido
        main.gravityModifier = 1f; // Luego cae
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = splash.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[]{
            new ParticleSystem.Burst(0f, 20)
        });

        var shape = splash.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.rotation = Vector3.zero; // Ajusta la rotación del cono si es necesario
        shape.angle = 20f; // Ajusta el ángulo del cono para la dispersión
        shape.radius = 0.1f; // Ajusta el radio de la base del cono

        // Destruir el GameObject después de un tiempo (basado en la vida útil de las partículas)
        Destroy(splash.gameObject, main.startLifetime.constantMax + 0.5f);
    }
}
