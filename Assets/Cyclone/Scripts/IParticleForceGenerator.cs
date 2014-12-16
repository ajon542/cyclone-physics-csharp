
namespace Cyclone
{
    /// <summary>
    /// A force generator can be asked to add a force to one or more particles.
    /// </summary>
    public interface IParticleForceGenerator
    {
        /// <summary>
        /// Apply a force to the particle.
        /// </summary>
        /// <param name="particle">The particle.</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        void UpdateForce(Particle particle, double duration);
    }
}
