using UnityEngine;

/// <summary>
/// 
/// </summary>
public class RigidBodySpring : MonoBehaviour
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
    private Cyclone.ForceRegistry registry = new Cyclone.ForceRegistry();

    /// <summary>
    /// The first particle.
    /// </summary>
    private Cyclone.RigidBody body1 = new Cyclone.RigidBody();

    /// <summary>
    /// The second particle.
    /// </summary>
    private Cyclone.RigidBody body2 = new Cyclone.RigidBody();

    /// <summary>
    /// Set the spring properties.
    /// </summary>
    void Start()
    {
        // Set particle properties.
        body1.Mass = object1Mass;
        body1.LinearDamping = object1Damping;
        body1.SetPosition(object1.transform.position.x, object1.transform.position.y, object1.transform.position.z);

        body2.Mass = object2Mass;
        body2.LinearDamping = object2Damping;
        body2.SetPosition(object2.transform.position.x, object2.transform.position.y, object2.transform.position.z);

        // Create the particle springs.
        Cyclone.Spring spring1 = new Cyclone.Spring(new Cyclone.Math.Vector3(), body2, new Cyclone.Math.Vector3(), springConstant, restingLength);
        Cyclone.Spring spring2 = new Cyclone.Spring(new Cyclone.Math.Vector3(), body1, new Cyclone.Math.Vector3(), springConstant, restingLength);

        // Add the particle springs to the force registry.
        registry.Add(body1, spring1);
        registry.Add(body2, spring2);
    }

    /// <summary>
    /// Update the particle positions.
    /// </summary>
    void Update()
    {
        double duration = Time.deltaTime;
        registry.UpdateForces(duration);
        body1.Integrate(duration);
        body2.Integrate(duration);

        // TODO: Work on a conversion method for Cyclone.Math and UnityEngine Vector3
        object1.transform.position = new Vector3((float)body1.Position.x, (float)body1.Position.y, (float)body1.Position.z);
        object2.transform.position = new Vector3((float)body2.Position.x, (float)body2.Position.y, (float)body2.Position.z);
    }
}
