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
        /// Applies the gavitational force to the given rigid body.
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
}
