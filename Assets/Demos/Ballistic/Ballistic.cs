using UnityEngine;

public class Ballistic : MonoBehaviour
{
    private Cyclone.Particle particle = new Cyclone.Particle();

    public Vector3 velocity;
    public Vector3 acceleration;
    public float damping;

    private void Start()
    {
        particle.Mass = 2.0f;
        particle.Velocity = new Cyclone.Math.Vector3(velocity.x, velocity.y, velocity.z);
        particle.Acceleration = new Cyclone.Math.Vector3(acceleration.x, acceleration.y, acceleration.z);
        particle.Position = new Cyclone.Math.Vector3(0.0f, 2.0f, 0.0f);
        particle.Damping = damping;

        SetObjectPosition(particle.Position);
    }

    private void Update()
    {
        SetObjectPosition(particle.Position);
        particle.Integrate(Time.deltaTime);
    }

    private void SetObjectPosition(Cyclone.Math.Vector3 position)
    {
        transform.position = new Vector3((float)position.x, (float)position.y, (float)position.z);
    }
}
