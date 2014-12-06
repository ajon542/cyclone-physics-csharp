using System;
using Cyclone.Math;

namespace Cyclone
{
    public class Particle
    {
        /// <summary>
        /// Gets or sets the damping of the particle.
        /// </summary>
        /// <remarks>
        /// Damping represents a rough approximation of drag. A proportion of the
        /// objects velocty is removed at each update based on this value. A value
        /// of 1 means the object keeps all its velocity.
        /// </remarks>
        public double Damping { get; set; }

        /// <summary>
        /// Gets or sets the position of the particle.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the velocity of the particle.
        /// </summary>
        public Vector3 Velocity { get; set; }

        /// <summary>
        /// Gets or sets the accumulated force applied to the particle.
        /// </summary>
        protected Vector3 ForceAccum { get; set; }

        /// <summary>
        /// Gets or sets the acceleration of the particle.
        /// </summary>
        public Vector3 Acceleration { get; set; }

        /// <summary>
        /// Gets or sets the inverse mass of the particle.
        /// </summary>
        /// <remarks>
        /// It is more practicle to use an inverse mass as integration is simpler
        /// and it makes more sense to have an object of infinite mass i.e. on that
        /// cannot be moved.
        /// </remarks>
        public double InverseMass { get; set; }

        /// <summary>
        /// Gets or sets the mass of the particle.
        /// </summary>
        public double Mass
        {
            get
            {
                if (Core.Equals(InverseMass, 0))
                {
                    return Double.MaxValue;
                }
                return 1.0 / InverseMass;
            }

            set
            {
                // Mass cannot be zero, since force will generate infinite acceleration.
                if (Core.Equals(value, 0))
                {
                    throw new Exception("Mass cannot be zero");
                }

                InverseMass = 1.0 / value;
            }

        }

        /// <summary>
        /// Creates a new instance of the <see cref="Particle"/> class.
        /// </summary>
        public Particle()
        {
            Position = new Vector3();
            Velocity = new Vector3();
            ForceAccum = new Vector3();
            Acceleration = new Vector3();
        }

        /// <summary>
        /// Determines if this particle has finite mass.
        /// </summary>
        /// <returns><c>true</c> if particle has finite mass; otherwise, <c>false</c>.</returns>
        public bool HasFiniteMass()
        {
            return InverseMass >= 0.0;
        }

        /// <summary>
        /// Set the velocity of this particle.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        public void SetVelocity(double x, double y, double z)
        {
            Velocity.x = x;
            Velocity.y = y;
            Velocity.z = z;
        }

        /// <summary>
        /// Set the position of this particle.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        public void SetPosition(double x, double y, double z)
        {
            Position.x = x;
            Position.y = y;
            Position.z = z;
        }

        /// <summary>
        /// Set the acceleration of this particle.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        public void SetAcceleration(double x, double y, double z)
        {
            Acceleration.x = x;
            Acceleration.y = y;
            Acceleration.z = z;
        }

        /// <summary>
        /// Get a copy of the particle position.
        /// </summary>
        /// <returns>A copy of the particle position vector.</returns>
        public Vector3 GetPosition()
        {
            return new Vector3(Position.x, Position.y, Position.z);
        }

        /// <summary>
        /// Get a copy of the particle velocity.
        /// </summary>
        /// <returns>A copy of the particle velocity vector.</returns>
        public Vector3 GetVelocity()
        {
            return new Vector3(Velocity.x, Velocity.y, Velocity.z);
        }

        /// <summary>
        /// Get a copy of the particle acceleration.
        /// </summary>
        /// <returns>A copy of the particle acceleration vector.</returns>
        public Vector3 GetAcceleration()
        {
            return new Vector3(Acceleration.x, Acceleration.y, Acceleration.z);
        }

        /// <summary>
        /// Calculate the new position and velocity of the particle.
        /// </summary>
        /// <param name="duration">
        /// Time interval over which to update the position and velocity.
        /// This is currently the time between frames.
        /// </param>
        public void Integrate(double duration)
        {
            // We don't integrate things with zero mass.
            if (InverseMass <= 0.0f)
            {
                return;
            }

            // Make sure duration is positive.
            if (duration <= 0.0)
            {
                throw new ArgumentOutOfRangeException("duration", "must be greater than 0");
            }

            // Update linear position.
            Position.AddScaledVector(Velocity, duration);

            // Work out the acceleration from the force.
            Vector3 resultingAcc = GetAcceleration();
            resultingAcc.AddScaledVector(ForceAccum, InverseMass);

            // Update linear velocity from the acceleration.
            Velocity.AddScaledVector(resultingAcc, duration);

            // Impose drag.
            Velocity *= System.Math.Pow(Damping, duration);

            // Clear the forces.
            ClearAccumulator();
        }

        /// <summary>
        /// Add a force to this particle.
        /// </summary>
        /// <param name="force">The force to add.</param>
        public void AddForce(Vector3 force)
        {
            ForceAccum += force;
        }

        /// <summary>
        /// Clear the accumulated force.
        /// </summary>
        private void ClearAccumulator()
        {
            ForceAccum.Clear();
        }
    }
}
