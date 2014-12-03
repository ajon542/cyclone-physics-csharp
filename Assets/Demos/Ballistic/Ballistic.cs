using UnityEngine;

/// <summary>
/// Class representing a simple particle affected by gravity.
/// </summary>
public class Ballistic : MonoBehaviour
{
    #region Unity Editor

    /// <summary>
    ///  The velocity of the particle.
    /// </summary>
    public Vector3 velocity;

    /// <summary>
    /// The acceleration of the particle.
    /// </summary>
    public Vector3 acceleration;

    /// <summary>
    /// The damping of the particle.
    /// </summary>
    public float damping;

    #endregion

    /// <summary>
    /// Create a particle instance.
    /// </summary>
    private Cyclone.Particle particle = new Cyclone.Particle();

    /// <summary>
    /// Set the default properties of the particle.
    /// </summary>
    private void Start()
    {
        particle.Mass = 2.0f;
        particle.Velocity = new Cyclone.Math.Vector3(velocity.x, velocity.y, velocity.z);
        particle.Acceleration = new Cyclone.Math.Vector3(acceleration.x, acceleration.y, acceleration.z);
        particle.Position = new Cyclone.Math.Vector3(0.0f, 2.0f, 0.0f);
        particle.Damping = damping;

        SetObjectPosition(particle.Position);
    }

    /// <summary>
    /// Update the particle position.
    /// </summary>
    private void Update()
    {
        particle.Integrate(Time.deltaTime);
        SetObjectPosition(particle.Position);
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
