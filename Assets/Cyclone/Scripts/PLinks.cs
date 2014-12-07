using System;
using Cyclone.Math;

namespace Cyclone
{
    /// <summary>
    /// Links connect two particles together, generating a contact if
    /// they violate the constraints of their link. It is used as a base
    /// class for cables and rods, and could be used as a base class for
    /// springs with a limit to their extension.
    /// </summary>
    public abstract class ParticleLink // TODO: ParticleContactGenerator
    {
        /// <summary>
        /// Holds the pair of particles that are connected by this link.
        /// </summary>
        public Particle[] particle;

        /// <summary>
        /// Gets or sets the curent length of this link.
        /// </summary>
        protected abstract double CurrentLength();

        /// <summary>
        /// Generates the contacts to keep this link from being violated.
        /// This class can only ever generate a single contact.
        /// </summary>
        /// TODO: Finish the comment.
        /// <param name="contact"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public abstract int AddContact(ParticleContact contact, int limit);
    }

    public class ParticleCable : ParticleLink
    {
        public double MaxLength { get; set; }
        public double Restitution { get; set; }

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
            // TODO: Probably need to have 'contact' as a reference or an out.
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

        protected override double CurrentLength()
        {
            Vector3 relativePos = particle[0].GetPosition() - particle[1].GetPosition();
            return relativePos.Magnitude;
        }
    }
}
