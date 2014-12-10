using System;
using Cyclone.Math;

namespace Cyclone
{
    /// <summary>
    /// A contact represents two objects in contact (in this case
    /// ParticleContact representing two particles). Resolving a contact
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
        public double Penetration { get; set; }

        /// <summary>
        /// Resolves this contact for both velocity and interpenetration.
        /// </summary>
        /// <param name="duration">Time interval over which to calculate velocity and interpenetration.</param>
        public void Resolve(double duration)
        {
            ResolveVelocity(duration);
            ResolveInterpenetration(duration);
        }

        /// <summary>
        /// Calculates the separating velocity at this contact.
        /// </summary>
        /// <returns>The separating velocity.</returns>
        public double CalculateSeparatingVelocity()
        {
            // Calculate the relative velocity.
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
                ParticleMovement[1] = movePerIMass * -particle[1].InverseMass;
            }

            // Apply the penetration resolution.
            particle[0].Position += ParticleMovement[0];

            if(particle[1] != null)
            {
                particle[1].Position += ParticleMovement[1];
            }
        }
    }

    /// <summary>
    /// The contact resolution implementation for particle contacts.
    /// One resolver instance can be shared for the entire simulation.
    /// </summary>
    public class ParticleContactResolver
    {
        /// <summary>
        /// Gets or sets the number of iterations allowed.
        /// </summary>
        public int Iterations { get; set; }

        /// <summary>
        /// This is a performance tracking value; we keep track of the
        /// number of iterations used.
        /// </summary>
        protected int IterationsUsed { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ParticleContactResolver"/> class.
        /// </summary>
        /// <param name="iterations">The number of iterations allowed.</param>
        public ParticleContactResolver(int iterations)
        {
            Iterations = iterations;
        }

        /// <summary>
        /// Resolves a set of particle contacts for both penetration and velocity.
        /// TODO: Revisit the method arguments, we could possibly use a list.
        /// </summary>
        /// <param name="contactArray">The set of particle contacts.</param>
        /// <param name="numberOfContacts">The number of contacts.</param>
        /// <param name="duration">Time interval over which to resolve the contacts.</param>
        public void ResolveContacts(ParticleContact[] contactArray, int numberOfContacts, double duration)
        {
            IterationsUsed = 0;

            while(IterationsUsed < Iterations)
            {
                // Find the contact with the largest closing separating velocity.
                double max = Double.MaxValue;
                int maxIndex = numberOfContacts;

                for(int i = 0; i < numberOfContacts; ++i)
                {
                    double sepVelocity = contactArray[i].CalculateSeparatingVelocity();

                    if((sepVelocity < max) && (sepVelocity < 0 || contactArray[i].Penetration > 0))
                    {
                        max = sepVelocity;
                        maxIndex = i;
                    }
                }

                // Do we have anything worth resolving?
                if(maxIndex == numberOfContacts)
                {
                    break;
                }

                // Resolve this contact.
                contactArray[maxIndex].Resolve(duration);

                ++IterationsUsed;
            }

        }
    }
}
