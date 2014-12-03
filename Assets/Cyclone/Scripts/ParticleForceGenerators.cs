using Cyclone.Math;

namespace Cyclone
{
    // TODO: Springs and things Chapter 6

    /// <summary>
    /// A force generator that applies a gravitational force to the particle.
    /// </summary>
    public class ParticleGravity : IParticleForceGenerator
    {
        /// <summary>
        /// The acceleration due to gravity.
        /// </summary>
        private Vector3 gravity;

        /// <summary>
        /// Creates a new instance of the <see cref="ParticleGravity"/> class.
        /// </summary>
        /// <param name="gravity">The acceleration due to gravity.</param>
        public ParticleGravity(Vector3 gravity)
        {
            this.gravity = gravity;
        }

        /// <summary>
        /// Apply a gravitational force to the particle.
        /// </summary>
        /// <param name="particle">The particle</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public void UpdateForce(Particle particle, double duration)
        {
            // Do not handle particles with infinite mass.
            if (!particle.HasFiniteMass())
            {
                return;
            }

            // Apply the mass-scaled force to the particle.
            particle.AddForce(gravity * particle.Mass);
        }
    }

    /// <summary>
    /// A force generator that applies a drag force to the particle.
    /// </summary>
    public class ParticleDrag : IParticleForceGenerator
    {
        /// <summary>
        /// The velocity drag coefficient.
        /// </summary>
        private double k1;

        /// <summary>
        /// The velocity squared drag coefficient.
        /// </summary>
        private double k2;

        /// <summary>
        /// Creates a new instance of the <see cref="ParticleDrag"/> class.
        /// </summary>
        /// <param name="k1">The velocity drag coefficient.</param>
        /// <param name="k2">The velocity squared drag coefficient.</param>
        public ParticleDrag(double k1, double k2)
        {
            this.k1 = k1;
            this.k2 = k2;
        }

        /// <summary>
        /// Apply a drag force to the particle.
        /// </summary>
        /// <param name="particle">The particle</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public void UpdateForce(Particle particle, double duration)
        {
            Vector3 force = particle.Velocity;

            // Calculate the total drag coefficient.
            double dragCoeff = force.Magnitude;
            dragCoeff = k1 * dragCoeff + k2 * dragCoeff * dragCoeff;

            // Calculate the final force and apply it.
            force.Normalize();
            force *= -dragCoeff;
            particle.AddForce(force);
        }
    }
}
