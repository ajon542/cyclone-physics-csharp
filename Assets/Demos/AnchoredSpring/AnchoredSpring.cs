using Cyclone;
using UnityEngine;

/// <summary>
/// Demonstration of a basic spring anchored to a fixed point in space.
/// </summary>
public class AnchoredSpring : MonoBehaviour
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
    /// The first particle.
    /// </summary>
    private Cyclone.Particle particle = new Cyclone.Particle();

    /// <summary>
    /// Set the default properties.
    /// </summary>
    void Start()
    {
        // Set particle properties.
        particle.Mass = 2.0f;
        particle.SetPosition(object1.transform.position.x, object1.transform.position.y, object1.transform.position.z);
        particle.Damping = 0.99f;

        // Create the particle anchored spring.
        Cyclone.Math.Vector3 anchorPosition = new Cyclone.Math.Vector3(anchor.transform.position.x, anchor.transform.position.y, anchor.transform.position.z);
        Cyclone.ParticleAnchoredSpring anchoredSpring = new ParticleAnchoredSpring(anchorPosition, 1.0f, 2.0f);

        // Add the particle spring to the force registry.
        registry.Add(particle, anchoredSpring);
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
