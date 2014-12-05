using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public Transform water;
    public Transform crate;

    public double maxDepth = 1.0f;
    public double volume = 100.0f;
    public double liquidDensity = 1000.0f;
    public double gravity = -9.8f;

    /// <summary>
    /// The particle force registry.
    /// </summary>
    private Cyclone.ParticleForceRegistry registry = new Cyclone.ParticleForceRegistry();

    /// <summary>
    /// The first particle.
    /// </summary>
    private Cyclone.Particle particle = new Cyclone.Particle();

    private void Start()
    {
        particle.Mass = 2.0f;
        particle.Acceleration = new Cyclone.Math.Vector3(0.0f, gravity, 0.0f);
        particle.Position = new Cyclone.Math.Vector3(crate.transform.position.x, crate.transform.position.y, crate.transform.position.z);
        particle.Damping = 0.4f;

        Cyclone.ParticleBuoyancy buoyancy = new Cyclone.ParticleBuoyancy
            (
                maxDepth, volume, water.transform.position.y, liquidDensity
            );

        registry.Add(particle, buoyancy);
    }

    private void Update()
    {
        double duration = Time.deltaTime;
        registry.UpdateForces(duration);
        particle.Integrate(duration);

        // TODO: Work on a conversion method for Cyclone.Math and UnityEngine Vector3
        crate.transform.position = new Vector3((float)particle.Position.x, (float)particle.Position.y, (float)particle.Position.z);
    }
}
