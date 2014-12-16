
namespace Cyclone
{
    /// <summary>
    /// A force generator can be asked to add a force to one or more bodies.
    /// </summary>
    public interface IForceGenerator
    {
        /// <summary>
        /// Apply a force to the rigid body.
        /// </summary>
        /// <param name="body">The rigid body.</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        void UpdateForce(RigidBody body, double duration);
    }
}
