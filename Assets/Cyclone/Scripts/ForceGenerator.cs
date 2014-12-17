using Cyclone.Math;

namespace Cyclone
{
    /// <summary>
    /// A force generator can be asked to add a force to one or more bodies.
    /// </summary>
    public class Gravity : IForceGenerator
    {
        /// <summary>
        /// Holds the acceleration due to gravity.
        /// </summary>
        private Vector3 gravity;

        /// <summary>
        /// Creates a new instance of the <see cref="Gravity"/> class.
        /// </summary>
        /// <param name="gravity">The acceleration due to gravity.</param>
        public Gravity(Vector3 gravity)
        {
            this.gravity = gravity;
        }

        /// <summary>
        /// Applies the gravitational force to the given rigid body.
        /// </summary>
        /// <param name="body">The rigid body.</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public void UpdateForce(RigidBody body, double duration)
        {
            // Check that we do not have infinite mass.
            if(body.HasFiniteMass() == false)
            {
                return;
            }

            // Apply the mass-scaled force to the body.
            body.AddForce(gravity * body.Mass);
        }
    }

    /// <summary>
    /// A force generator that applies an aerodynamic force.
    /// </summary>
    public class Aero : IForceGenerator
    {

        /// <summary>
        /// Gets or sets the aerodynamic tensor for the surface in body
        /// space.
        /// </summary>
        private Matrix3 Tensor { get; set; }

        /// <summary>
        /// Gets or sets the relative position of the aerodynamic surface in
        /// body coordinates.
        /// </summary>
        private Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the windspeed of the environment. This is easier
        /// than managing a separate windspeed vector per generator
        /// and having to update it manually as the wind changes.
        /// </summary>
        private Vector3 Windspeed { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Aero"/> class.
        /// </summary>
        /// <param name="tensor">The aero dynamic tensor for the surface.</param>
        /// <param name="position">The position of the aerodynamic surface.</param>
        /// <param name="windspeed">The windspeed of the environment.</param>
        public Aero(Matrix3 tensor, Vector3 position, Vector3 windspeed)
        {
            Tensor = tensor;
            Position = position;
            Windspeed = windspeed;
        }

        /// <summary>
        /// Applies the aerodynamic force to the given rigid body.
        /// </summary>
        /// <param name="body">The rigid body.</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public void UpdateForce(RigidBody body, double duration)
        {
            UpdateForceFromTensor(body, duration, Tensor);
        }

        /// <summary>
        /// Uses an explicit tensor matrix to update the force on
        /// the given rigid body. This is exactly the same as for UpdateForce
        /// only it takes an explicit tensor.
        /// </summary>
        /// <param name="body">The rigid body.</param>
        /// <param name="duration">The time interval over which to update the force.</param>
        /// <param name="tensor">The tensor matrix to apply.</param>
        private void UpdateForceFromTensor(RigidBody body, double duration, Matrix3 tensor)
        {
            // Calculate total velocity (windspeed and body's velocity).
            Vector3 velocity = body.GetVelocity();
            velocity += Windspeed;

            // Calculate the velocity in body coordinates
            Vector3 bodyVel = body.GetTransform().TransformInverseDirection(velocity);

            // Calculate the force in body coordinates
            Vector3 bodyForce = Tensor.Transform(bodyVel);
            Vector3 force = body.GetTransform().TransformDirection(bodyForce);

            // Apply the force
            body.AddForceAtBodyPoint(force, Position);
        }
    };
}
