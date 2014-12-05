using UnityEngine;

/// <summary>
/// Demonstration of an elastic bungee.
/// </summary>
public class Bungee : MonoBehaviour
{
    /// <summary>
    /// The object representing the anchor point in space.
    /// </summary>
    public Transform anchor;

    /// <summary>
    /// The object representing the particle at the end of the spring.
    /// </summary>
    public Transform object1;

    /// <summary>
    /// The particle force registry.
    /// </summary>
    private Cyclone.ParticleForceRegistry registry = new Cyclone.ParticleForceRegistry();

    /// <summary>
    /// A particle acting as the anchor.
    /// </summary>
    private Cyclone.Particle anchorParticle = new Cyclone.Particle();

    /// <summary>
    /// A particle affected by the bungee.
    /// </summary>
    private Cyclone.Particle particle = new Cyclone.Particle();

    /// <summary>
    /// Set the default properties.
    /// </summary>
    void Start()
    {
        // Set particle properties.
        anchorParticle.Mass = 2.0f;
        anchorParticle.Velocity = new Cyclone.Math.Vector3(0.0f, 0.0f, 0.0f);
        anchorParticle.Acceleration = new Cyclone.Math.Vector3(0.0f, 0.0f, 0.0f);
        anchorParticle.Position = new Cyclone.Math.Vector3(anchor.transform.position.x, anchor.transform.position.y, anchor.transform.position.z);
        anchorParticle.Damping = 0.99f;

        particle.Mass = 2.0f;
        particle.Velocity = new Cyclone.Math.Vector3(0.0f, 0.0f, 0.0f);
        particle.Acceleration = new Cyclone.Math.Vector3(0.0f, 0.0f, 0.0f);
        particle.Position = new Cyclone.Math.Vector3(object1.transform.position.x, object1.transform.position.y, object1.transform.position.z);
        particle.Damping = 0.99f;

        // Create the particle anchored bungee.
        Cyclone.ParticleBungee bungee = new Cyclone.ParticleBungee(anchorParticle, 5.0f, 2.0f);

        // Add the particle spring to the force registry.
        registry.Add(particle, bungee);
    }

    /// <summary>
    /// Update the particle positions.
    /// </summary>
    void Update()
    {
        double duration = Time.deltaTime;
        registry.UpdateForces(duration);
        particle.Integrate(duration);

        // TODO: Work on a conversion method for Cyclone.Math and UnityEngine Vector3
        object1.transform.position = new Vector3((float)particle.Position.x, (float)particle.Position.y, (float)particle.Position.z);
    }
}
