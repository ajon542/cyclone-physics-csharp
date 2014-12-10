using System;
using Cyclone.Math;

namespace Cyclone
{
    /// <summary>
    /// Basic interface for contact generators applying to particles.
    /// TODO: This may be better as an interface rather than abstract class.
    /// </summary>
    public abstract class ParticleContactGenerator
    {
        /// <summary>
        /// Generates the contacts.
        /// </summary>
        /// TODO: Argument should be a list of contacts.
        /// <param name="contact">List of contacts.</param>
        /// <param name="limit">Maximum number of contacts.</param>
        /// <returns>The number of contacts that have been written.</returns>
        public abstract int AddContact(ParticleContact contact, int limit);
    }

    /// <summary>
    /// Cables link a pair of particles, generating a contact if the stray too
    /// far apart.
    /// </summary>
    public class ParticleCable : ParticleContactGenerator
    {
        /// <summary>
        /// Holds the pair of particles that are connected by this link.
        /// </summary>
        public Particle[] particle;

        /// <summary>
        /// Gets or sets the maximum length of this cable.
        /// </summary>
        public double MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the restitution (bounciness) of this cable.
        /// </summary>
        /// <remarks>
        /// A value of 0.0 means no bounciness.
        /// A value of 1.0 means very bouncy.
        /// </remarks>
        public double Restitution { get; set; }

        /// <summary>
        /// Calculates the current length of this cable.
        /// </summary>
        /// <returns></returns>
        protected double CurrentLength()
        {
            Vector3 relativePos = particle[0].GetPosition() - particle[1].GetPosition();
            return relativePos.Magnitude;
        }

        /// <summary>
        /// Generates the given contact structure with the contact needed
        /// to keep the cable from overextending.
        /// </summary>
        /// <param name="contact">List of contacts.</param>
        /// <param name="limit">Maximum number of contacts.</param>
        /// <returns>The number of contacts that have been written.</returns>
        public override int AddContact(ParticleContact contact, int limit)
        {
            // Find the length of the cable.
            double length = CurrentLength();

            // Check if we are overextended.
            if(length < MaxLength)
            {
                return 0;
            }

            // Otherwise, return the contact.
            contact.particle[0] = particle[0];
            contact.particle[1] = particle[1];

            // Calculate the normal.
            Vector3 normal = particle[1].GetPosition() - particle[0].GetPosition();
            normal.Normalize();
            contact.ContactNormal = normal;

            // Update the poperties of the contact.
            contact.Penetration = length - MaxLength;
            contact.Restitution = Restitution;

            return 1;
        }
    }

    /// <summary>
    /// Rods link a pair of particles, generating a contact if the stray too
    /// far apart or too close.
    /// </summary>
    public class ParticleRod : ParticleContactGenerator
    {
        /// <summary>
        /// Holds the pair of particles that are connected by this link.
        /// </summary>
        public Particle[] particle;

        /// <summary>
        /// Gets or sets the length of this rod.
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Calculates the current length of this cable.
        /// </summary>
        /// <returns></returns>
        protected double CurrentLength()
        {
            Vector3 relativePos = particle[0].GetPosition() - particle[1].GetPosition();
            return relativePos.Magnitude;
        }

        /// <summary>
        /// Generates the given contact structure with the contact needed
        /// to keep the cable from extending or compressing.
        /// </summary>
        /// <param name="contact">List of contacts.</param>
        /// <param name="limit">Maximum number of contacts.</param>
        /// <returns>The number of contacts that have been written.</returns>
        public override int AddContact(ParticleContact contact, int limit)
        {
            // Find the length of the cable.
            double length = CurrentLength();

            // Check if we are overextended.
            if (length == Length)
            {
                return 0;
            }

            // Otherwise, return the contact.
            contact.particle[0] = particle[0];
            contact.particle[1] = particle[1];

            // Calculate the normal.
            Vector3 normal = particle[1].GetPosition() - particle[0].GetPosition();
            normal.Normalize();

            // The contact normal depends on whether the particles are
            // extending or compressing.
            if(length > Length)
            {
                contact.ContactNormal = normal;
                contact.Penetration = length - Length;
            }
            else
            {
                contact.ContactNormal = normal * -1;
                contact.Penetration = Length - length;
            }

            // Always use a zero restitution (no bounciness).
            contact.Restitution = 0.0f;

            return 1;
        }
    }
}
