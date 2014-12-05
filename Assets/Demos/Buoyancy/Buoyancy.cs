using UnityEngine;

/// <summary>
/// Demonstration of the buoyancy of an object in water.
/// TODO: Haven't found decent default settings.
/// </summary>
public class Buoyancy : MonoBehaviour
{
    /// <summary>
    /// The object representing the water surface.
    /// </summary>
    public Transform water;

    /// <summary>
    /// The buoyant object.
    /// </summary>
    public Transform crate;

    /// <summary>
    /// The maximum submersion depth of the object before it generates its
    /// maximum buoyancy force.
    /// </summary>
    public double maxDepth = -1.0f;

    /// <summary>
    /// The volume of the object.
    /// </summary>
    public double volume = 0.05f;

    /// <summary>
    /// The density of the liquid.
    /// </summary>
    public double liquidDensity = 1000.0f;
    
    /// <summary>
    /// The gravity.
    /// </summary>
    public double gravity = -10.0f;

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
        particle.Mass = 4.0f;
        particle.Acceleration = new Cyclone.Math.Vector3(0.0f, gravity, 0.0f);
        particle.Position = new Cyclone.Math.Vector3(crate.transform.position.x, crate.transform.position.y, crate.transform.position.z);
        particle.Damping = 0.8f;

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
