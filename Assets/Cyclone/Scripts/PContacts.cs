using System;
using Cyclone.Math;

namespace Cyclone
{
    /// <summary>
    /// A contact represents two objects in contact (in this case
    /// PartcileContact representing two particles). Resolving a contact
    /// removes their interpenetration, and applies sufficient impulse to
    /// keep them apart. Colliding bodies may also rebound.
    /// 
    /// The contact has no callable methods, it just hold the contact
    /// details. To resolve a set of contacts, use the particle contact
    /// resolver class.
    /// </summary>
    public class ParticleContact
    {
        /// <summary>
        /// Holds the particles that are involved in the contact. The second of
        /// these can be null for contacts with the scenery.
        /// </summary>
        public Particle[] particle;

        /// <summary>
        /// Holds the normal restitution coefficient at the contact.
        /// </summary>
        public double Restitution { get; set; }

        /// <summary>
        /// Holds the direction of the contact in world coordinates.
        /// </summary>
        public Vector3 ContactNormal { get; set; }

        /// <summary>
        /// Resolves this contact for both velocity and interpenetration.
        /// </summary>
        /// <param name="duration">Time interval over which to calculate velocity and interpenetration.</param>
        protected void Resolve(double duration)
        {
            ResolveVelocity(duration);
            ResolveInterpenetration(duration);
        }

        /// <summary>
        /// Calculates the separating velocity at this contact.
        /// </summary>
        /// <returns>The separating velocity.</returns>
        protected double CalculateSeparatingVelocity()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the impulse calculations for this collision.
        /// </summary>
        /// <param name="duration">Time interval over which to update the impulse.</param>
        private void ResolveVelocity(double duration)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the interpenetration resolution for this contact.
        /// </summary>
        /// <param name="duration">Time interval over which to update the interpenetration.</param>
        private void ResolveInterpenetration(double duration)
        {
            throw new NotImplementedException();
        }
    }
}
