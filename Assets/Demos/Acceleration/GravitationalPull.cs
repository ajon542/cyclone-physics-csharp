using UnityEngine;

/// <summary>
/// Class representing a simple particle affected by gravity.
/// </summary>
public class GravitationalPull : MonoBehaviour
{
    /// <summary>
    /// Create a particle instance.
    /// </summary>
    private Cyclone.Particle particle = new Cyclone.Particle();

    public float acceleration = 4.0f;
    public float threshold = 0.5f;
    bool minThresholdReached;
    bool maxThresholdReached;
    /// <summary>
    /// Set the default properties of the particle.
    /// </summary>
    private void Start()
    {
        particle.Mass = 2.0f;
        particle.Velocity = new Cyclone.Math.Vector3(0.0f, 0.0f, 0.0f);
        particle.Acceleration = new Cyclone.Math.Vector3(acceleration, 0.0f, 0.0f);
        particle.Position = new Cyclone.Math.Vector3(0.0f, 2.0f, 0.0f);
        particle.Damping = 0.95f;

        SetObjectPosition(particle.Position);
    }

    /// <summary>
    /// Update the particle position.
    /// </summary>
    private void Update()
    {
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
