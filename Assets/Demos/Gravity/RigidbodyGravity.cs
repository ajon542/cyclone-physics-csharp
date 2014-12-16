using UnityEngine;

public class RigidbodyGravity : MonoBehaviour
{
    /// <summary>
    /// Create a rigidbody instance.
    /// </summary>
    private Cyclone.RigidBody body = new Cyclone.RigidBody();

    /// <summary>
    /// Set the default properties of the rigidbody.
    /// </summary>
    private void Start()
    {
        body.Mass = 2.0f;
        body.SetPosition(transform.position.x, transform.position.y, transform.position.z);
        body.SetAcceleration(0.0f, -10.0f, 0.0f);
        body.SetVelocity(0.0f, 10.0f, 30.0f);
        body.LinearDamping = 0.95f;
        body.SetAwake();
    }

    /// <summary>
    /// Update the particle position.
    /// </summary>
    private void Update()
    {
        body.Integrate(Time.deltaTime);
        SetObjectPosition(body.Position);
    }

    /// <summary>
    /// Helper method to convert a Cyclone.Math.Vector3 to a UnityEngine.Vector3 position.
    /// </summary>
    /// <param name="position">The position.</param>
    private void SetObjectPosition(Cyclone.Math.Vector3 position)
    {
        transform.position = new Vector3((float)position.x, (float)position.y, (float)position.z);
    }
}
