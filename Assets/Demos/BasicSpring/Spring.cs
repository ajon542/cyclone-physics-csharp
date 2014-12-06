using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Spring : MonoBehaviour
{
    #region UnityEditor
    
    // TODO: Create a spring editor script.

    /// <summary>
    /// Mass of the first object.
    /// </summary>
    public double object1Mass;

    /// <summary>
    /// Damping of the first object.
    /// </summary>
    public double object1Damping;

    /// <summary>
    /// Mass of the second object.
    /// </summary>
    public double object2Mass;

    /// <summary>
    /// Damping of the second object.
    /// </summary>
    public double object2Damping;

    /// <summary>
    /// The spring constant represents the stiffness of the spring.
    /// </summary>
    public double springConstant;

    /// <summary>
    /// The resting length of the spring.
    /// </summary>
    public double restingLength;

    #endregion

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
    /// Set the spring properties.
    /// </summary>
    void Start()
    {
        // Set particle properties.
        particle1.Mass = object1Mass;
        particle1.Damping = object1Damping;
        particle1.SetPosition(object1.transform.position.x, object1.transform.position.y, object1.transform.position.z);

        particle2.Mass = object2Mass;
        particle2.Damping = object2Damping;
        particle2.SetPosition(object2.transform.position.x, object2.transform.position.y, object2.transform.position.z);

        // Create the particle springs.
        Cyclone.ParticleSpring particleSpring1 = new Cyclone.ParticleSpring(particle2, springConstant, restingLength);
        Cyclone.ParticleSpring particleSpring2 = new Cyclone.ParticleSpring(particle1, springConstant, restingLength);

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
