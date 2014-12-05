using UnityEngine;

/// <summary>
/// Demonstration of a basic spring force between two objects.
/// TODO: The behavior appears incorrect.
/// </summary>
public class BasicSpring : MonoBehaviour
{
    /// <summary>
    /// The object representing the first particle.
    /// </summary>
    public Transform object1;

    /// <summary>
    /// The object representing the second particle.
    /// </summary>
    public Transform object2;

    /// <summary>
    /// The particle force registry.
    /// </summary>
    private Cyclone.ParticleForceRegistry registry = new Cyclone.ParticleForceRegistry();

    /// <summary>
    /// The first particle.
    /// </summary>
    private Cyclone.Particle particle1 = new Cyclone.Particle();

    /// <summary>
    /// The second particle.
    /// </summary>
    private Cyclone.Particle particle2 = new Cyclone.Particle();

    /// <summary>
    /// Set the default properties.
    /// </summary>
    void Start()
    {
        // Set particle properties.
        particle1.Mass = 2.0f;
        particle1.Velocity = new Cyclone.Math.Vector3(0.0f, 0.0f, 0.0f);
        particle1.Acceleration = new Cyclone.Math.Vector3(0.0f, 0.0f, 0.0f);
        particle1.Position = new Cyclone.Math.Vector3(-4.0f, 0.0f, 0.0f);
        particle1.Damping = 0.95f;

        particle2.Mass = 2.0f;
        particle2.Velocity = new Cyclone.Math.Vector3(0.0f, 0.0f, 0.0f);
        particle2.Acceleration = new Cyclone.Math.Vector3(0.0f, 0.0f, 0.0f);
        particle2.Position = new Cyclone.Math.Vector3(4.0f, 0.0f, 0.0f);
        particle2.Damping = 0.95f;

        // Set the object position.
        object1.transform.position = new Vector3((float)particle1.Position.x, (float)particle1.Position.y, (float)particle1.Position.z);
        object2.transform.position = new Vector3((float)particle2.Position.x, (float)particle2.Position.y, (float)particle2.Position.z);

        // Create the particle springs.
        Cyclone.ParticleSpring particleSpring1 = new Cyclone.ParticleSpring(particle2, 1.0f, 3.0f);
        Cyclone.ParticleSpring particleSpring2 = new Cyclone.ParticleSpring(particle1, 1.0f, 3.0f);

        // Add the particle springs to the force registry.
        registry.Add(particle1, particleSpring1);
        registry.Add(particle2, particleSpring2);
    }

    /// <summary>
    /// Update the particle positions.
    /// </summary>
    void Update()
    {
        double duration = Time.deltaTime;
        registry.UpdateForces(duration);
        particle1.Integrate(duration);
        particle2.Integrate(duration);

        // TODO: Work on a conversion method for Cyclone.Math and UnityEngine Vector3
        object1.transform.position = new Vector3((float)particle1.Position.x, (float)particle1.Position.y, (float)particle1.Position.z);
        object2.transform.position = new Vector3((float)particle2.Position.x, (float)particle2.Position.y, (float)particle2.Position.z);
    }
}
