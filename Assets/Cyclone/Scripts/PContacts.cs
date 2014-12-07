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

        public Vector3[] ParticleMovement;

        /// <summary>
        /// Holds the normal restitution coefficient at the contact.
        /// </summary>
        public double Restitution { get; set; }

        /// <summary>
        /// Holds the direction of the contact in world coordinates.
        /// </summary>
        public Vector3 ContactNormal { get; set; }

        /// <summary>
        /// Holds the depth of the penetration at the contact.
        /// </summary>
        private double Penetration { get; set; }

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
            // Cacluclate the relative velocity.
            Vector3 relativeVelocity = particle[0].GetVelocity();
            if (particle[1] != null)
            {
                relativeVelocity -= particle[1].GetVelocity();
            }

            // Return the dot product of velocity and contact normal.
            return relativeVelocity * ContactNormal;
        }

        /// <summary>
        /// Handles the impulse calculations for this collision.
        /// </summary>
        /// <param name="duration">Time interval over which to update the impulse.</param>
        private void ResolveVelocity(double duration)
        {
            // Find the velocity in the direction of the contact.
            double separatingVelocity = CalculateSeparatingVelocity();

            // Check if it needs to be resolved.
            if(separatingVelocity > 0)
            {
                // The contact is either separating or stationary;
                // there's no impulse required.
                return;
            }

            // Calculate the new separating velocity.
            double newSepVelocity = -separatingVelocity * Restitution;

            // Check the velocity buildup due to acceleration only.
            Vector3 accCausedVelocity = particle[0].GetAcceleration();

            if(particle[1] != null)
            {
                accCausedVelocity -= particle[1].GetAcceleration();
            }

            double accCausedSepVelocity = accCausedVelocity * ContactNormal * duration;

            // If there is a closing velocity due to acceleration buildup,
            // remove it from the separating velocity.
            if(accCausedSepVelocity < 0)
            {
                newSepVelocity += Restitution * accCausedSepVelocity;

                if(newSepVelocity < 0)
                {
                    newSepVelocity = 0;
                }
            }

            double deltaVelocity = newSepVelocity - separatingVelocity;

            // We apply the change in velocity to each object in proportion
            // to their inverse mass (those with lower inverse mass get less
            // change in velocity).
            double totalInverseMass = particle[0].InverseMass;

            if(particle[1] != null)
            {
                totalInverseMass += particle[1].InverseMass;
            }

            // If all particles have infinite mass, then impulses have no effect.
            if(totalInverseMass <= 0)
            {
                return;
            }

            // Calculate the impulse to apply.
            double impulse = deltaVelocity / totalInverseMass;

            // Find the amount of impulse per unit of inverse mass.
            Vector3 impulsePerIMass = ContactNormal * impulse;
            
            // Apply impulses: they are applied in the direction of the contact,
            // and ae propertional to inverse mass.
            particle[0].Velocity += impulsePerIMass * particle[0].InverseMass;

            if(particle[1] !=null)
            {
                // Particle 1 goes in the opposite direction.
                particle[1].Velocity += impulsePerIMass * -particle[1].InverseMass;
            }
        }

        /// <summary>
        /// Handles the interpenetration resolution for this contact.
        /// </summary>
        /// <param name="duration">Time interval over which to update the interpenetration.</param>
        private void ResolveInterpenetration(double duration)
        {
            // If there is no penetration, nothing to do.
            if(Penetration <= 0)
            {
                return;
            }

            // The movement of each object is based on their inverse mass.
            double totalInverseMass = particle[0].InverseMass;

            if(particle[1] != null)
            {
                totalInverseMass += particle[1].InverseMass;
            }

            // If all particles have infinite mass, nothing to do.
            if(totalInverseMass <= 0)
            {
                return;
            }

            // Find the amount of penetration resolution per unit of inverse mass.
            Vector3 movePerIMass = ContactNormal * (Penetration / totalInverseMass);

            // Calculate the movement amounts.
            ParticleMovement[0] = movePerIMass * particle[0].InverseMass;

            if(particle[1] != null)
            {
                ParticleMovement[1] = movePerIMass * particle[1].InverseMass;
            }
            else
            {
                ParticleMovement[1].Clear();
            }

            // Apply the penetration resolution.
            particle[0].Position += ParticleMovement[0];

            if(particle[1] != null)
            {
                particle[1].Position += ParticleMovement[1];
            }
        }
    }
}
