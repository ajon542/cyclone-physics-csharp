using UnityEngine;

/// <summary>
/// Class representing a simple particle affected by gravity.
/// </summary>
public class GravitationalPull : MonoBehaviour
{
    #region Unity Editor

    /// <summary>
    /// Default X position of the particle.
    /// </summary>
    public float position = 4.0f;

    /// <summary>
    /// Default X acceleration of the particle.
    /// </summary>
    public float acceleration = 4.0f;

    /// <summary>
    /// Default X threshold for when the particle changes the direction of acceleration.
    /// </summary>
    public float threshold = 0.01f;

    /// <summary>
    /// Default damping of the particle.
    /// </summary>
    public float damping = 0.8f;

    #endregion

    /// <summary>
    /// Create a particle instance.
    /// </summary>
    private Cyclone.Particle particle = new Cyclone.Particle();

    /// <summary>
    /// Flag indicating the minimum threshold has been reached.
    /// </summary>
    bool minThresholdReached;

    /// <summary>
    /// Flag indicating the maximum threshold has been reached.
    /// </summary>
    bool maxThresholdReached;

    /// <summary>
    /// Set the default properties of the particle.
    /// </summary>
    private void Start()
    {
        particle.Mass = 2.0f;
        particle.Velocity = new Cyclone.Math.Vector3(0.0f, 0.0f, 0.0f);
        particle.Acceleration = new Cyclone.Math.Vector3(acceleration, 0.0f, 0.0f);
        particle.Position = new Cyclone.Math.Vector3(position, 0.0f, 0.0f);
        particle.Damping = damping;

        SetObjectPosition(particle.Position);
    }

    /// <summary>
    /// Update the particle position.
    /// </summary>
    private void Update()
    {
        // TODO: It would be cool to simulate a planet orbiting around the sun.
        if(!minThresholdReached && particle.Position.x < -threshold)
        {
            minThresholdReached = true;
            maxThresholdReached = false;
            acceleration = -acceleration;
            particle.Acceleration.x = acceleration;
        }

        if(!maxThresholdReached && particle.Position.x > threshold)
        {
            maxThresholdReached = true;
            minThresholdReached = false;
            acceleration = -acceleration;
            particle.Acceleration.x = acceleration;
        }

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
