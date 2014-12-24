using UnityEngine;

/// <summary>
/// Rigid body spring demo. The spring can be attached to a particular point
/// on the rigid body to cause a rotation around the center of mass.
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
    /// The object representing the first rigid body.
    /// </summary>
    public Transform object1;

    /// <summary>
    /// The object representing the second rigid body.
    /// </summary>
    public Transform object2;

    /// <summary>
    /// The force registry.
    /// </summary>
    private Cyclone.ForceRegistry registry = new Cyclone.ForceRegistry();

    /// <summary>
    /// The first rigid body.
    /// </summary>
    private Cyclone.RigidBody body1 = new Cyclone.RigidBody();

    /// <summary>
    /// The second rigid body.
    /// </summary>
    private Cyclone.RigidBody body2 = new Cyclone.RigidBody();

    /// <summary>
    /// Set the spring properties.
    /// </summary>
    void Start()
    {
        // Set rigid body properties.
        body1.Mass = object1Mass;
        body1.SetDamping(object1Damping, object1Damping);
        body1.SetPosition(object1.transform.position.x, object1.transform.position.y, object1.transform.position.z);

        body2.Mass = object2Mass;
        body2.SetDamping(object2Damping, object2Damping);
        body2.SetPosition(object2.transform.position.x, object2.transform.position.y, object2.transform.position.z);

        // Set the inertia tensor of a block.
        Cyclone.Math.Matrix3 it = new Cyclone.Math.Matrix3();
        it.SetBlockInertiaTensor(new Cyclone.Math.Vector3(1, 1, 1), 2);
        body1.SetInertiaTensor(it);
        body2.SetInertiaTensor(it);

        // Create the springs.
        Cyclone.Spring spring1 = new Cyclone.Spring(new Cyclone.Math.Vector3(0.1, 0.1, 0.1), body2, new Cyclone.Math.Vector3(-0.1, -0.1, -0.1), springConstant, restingLength);
        Cyclone.Spring spring2 = new Cyclone.Spring(new Cyclone.Math.Vector3(-0.1, -0.1, -0.1), body1, new Cyclone.Math.Vector3(0.1, 0.1, 0.1), springConstant, restingLength);

        // Add the springs to the force registry.
        registry.Add(body1, spring1);
        registry.Add(body2, spring2);
    }

    /// <summary>
    /// Update the object position and rotation.
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

        Cyclone.Math.Quaternion r1 = body1.GetOrientation();
        object1.transform.rotation = new Quaternion((float)r1.i, (float)r1.j, (float)r1.k, (float)r1.r);

        Cyclone.Math.Quaternion r2 = body2.GetOrientation();
        object2.transform.rotation = new Quaternion((float)r2.i, (float)r2.j, (float)r2.k, (float)r2.r);
    }
}
