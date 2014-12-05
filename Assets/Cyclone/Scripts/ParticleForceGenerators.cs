using System;
using System.Diagnostics;
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

    /// <summary>
    /// A basic spring force generator calculating the length of the
    /// spring and using Hook's law to calculate the force.
    /// </summary>
    public class ParticleSpring : IParticleForceGenerator
    {
        /// <summary>
        /// The particle at the other end of the spring.
        /// </summary>
        private Particle other;

        /// <summary>
        /// A value that gives the stiffness of the spring.
        /// </summary>
        private double springConstant;

        /// <summary>
        /// The natural length of the spring when no forces are acting upon it.
        /// </summary>
        private double restLength;

        /// <summary>
        /// Creates a new instance of the <see cref="ParticleSpring"/> class.
        /// </summary>
        /// <param name="other">The particle at the other end of the spring.</param>
        /// <param name="springConstant">A value that gives the stiffness of the spring.</param>
        /// <param name="restLength">The natural length of the spring when no forces are acting upon it.</param>
        public ParticleSpring(Particle other, double springConstant, double restLength)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            this.other = other;
            this.springConstant = springConstant;
            this.restLength = restLength;
        }

        /// <summary>
        /// Apply a spring force to the particle.
        /// </summary>
        /// <param name="particle">The particle</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public void UpdateForce(Particle particle, double duration)
        {
            // Calculate the vector of the spring.
            Vector3 force = particle.Position;
            force -= other.Position;

            // Calculate the magnitude of the force.
            double magnitude = force.Magnitude;
            // TODO: Not sure why this Abs calculation is used here.
            // If the distance between the two particles is less than the restLength,
            // the particles have a force which pulls them together. I would have expected
            // the two particles to push apart.
            //magnitude = System.Math.Abs(magnitude - restLength);
            magnitude -= restLength;
            magnitude *= springConstant;

            // Calculate the final force and apply it.
            force.Normalize();
            force *= -magnitude;
            particle.AddForce(force);
        }
    }

    /// <summary>
    /// A force generator that applies a spring force, where one end is
    /// attached to a fixed point in space.
    /// </summary>
    public class ParticleAnchoredSpring : IParticleForceGenerator
    {
        /// <summary>
        /// Location of the anchored end of the spring.
        /// </summary>
        private Vector3 anchor;

        /// <summary>
        /// A value that gives the stiffness of the spring.
        /// </summary>
        private double springConstant;

        /// <summary>
        /// The natural length of the spring when no forces are acting upon it.
        /// </summary>
        private double restLength;

        /// <summary>
        /// Creates a new instance of the <see cref="ParticleAnchoredSpring"/> class.
        /// </summary>
        /// <param name="anchor">Location of the anchored end of the spring.</param>
        /// <param name="springConstant">A value that gives the stiffness of the spring.</param>
        /// <param name="restLength">The natural length of the spring when no forces are acting upon it.</param>
        public ParticleAnchoredSpring(Vector3 anchor, double springConstant, double restLength)
        {
            //if (anchor == null)
            //{
            //    throw new ArgumentNullException("anchor");
            //}

            this.anchor = anchor;
            this.springConstant = springConstant;
            this.restLength = restLength;
        }

        /// <summary>
        /// Apply a spring force to the particle.
        /// </summary>
        /// <param name="particle">The particle</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public void UpdateForce(Particle particle, double duration)
        {
            // Calculate the vector of the spring.
            Vector3 force = particle.Position;
            force -= anchor;

            // Calculate the magnitude of the force.
            double magnitude = force.Magnitude;
            magnitude = (restLength - magnitude)*springConstant;

            // Calculate the final force and apply it.
            force.Normalize();
            force *= magnitude;
            particle.AddForce(force);
        }
    }

    /// <summary>
    /// A force generator that applies a spring force only when extended.
    /// </summary>
    public class ParticleBungee : IParticleForceGenerator
    {
        /// <summary>
        /// The particle at the other end of the spring.
        /// </summary>
        private Particle other;

        /// <summary>
        /// A value that gives the stiffness of the spring.
        /// </summary>
        private double springConstant;

        /// <summary>
        /// The natural length of the spring when no forces are acting upon it.
        /// </summary>
        private double restLength;

        /// <summary>
        /// Creates a new instance of the <see cref="ParticleBungee"/> class.
        /// </summary>
        /// <param name="other">The particle at the other end of the spring.</param>
        /// <param name="springConstant">A value that gives the stiffness of the spring.</param>
        /// <param name="restLength">The natural length of the spring when no forces are acting upon it.</param>
        public ParticleBungee(Particle other, double springConstant, double restLength)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            this.other = other;
            this.springConstant = springConstant;
            this.restLength = restLength;
        }

        /// <summary>
        /// Apply a spring force to the particle.
        /// </summary>
        /// <param name="particle">The particle</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public void UpdateForce(Particle particle, double duration)
        {
            // Calculate the vector of the spring.
            Vector3 force = particle.Position;
            force -= other.Position;

            // Check if the bungee is compressed.
            double magnitude = force.Magnitude;
            if (magnitude <= restLength)
            {
                return;
            }

            // Calculate the magnitude of the force.
            magnitude = (restLength - magnitude) * springConstant;

            // Calculate the final force and apply it.
            force.Normalize();
            force *= magnitude;
            particle.AddForce(force);
        }
    }

    public class ParticleBuoyancy : IParticleForceGenerator
    {
        public void UpdateForce(Particle particle, double duration)
        {
            throw new NotImplementedException();
        }
    }
}
