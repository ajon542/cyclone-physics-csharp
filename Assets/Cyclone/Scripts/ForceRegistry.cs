using System;
using System.Collections.Generic;

namespace Cyclone
{
    // Create an alias for the pair within this file only.
    using ForceRegistration = KeyValuePair<RigidBody, IForceGenerator>;

    /// <summary>
    /// Class for registering a rigidbody with associated force generator.
    /// </summary>
    public class ForceRegistry
    {
        /// <summary>
        /// List of registered rigid bodies.
        /// </summary>
        private List<ForceRegistration> registrations;

        /// <summary>
        /// Creates a new instance of the <see cref="ForceRegistry"/> class.
        /// </summary>
        public ForceRegistry()
        {
            registrations = new List<ForceRegistration>();
        }

        /// <summary>
        /// Add a rigid body and the associated force generator.
        /// </summary>
        /// <param name="body">The rigid body.</param>
        /// <param name="fg">The force generator.</param>
        public void Add(RigidBody body, IForceGenerator fg)
        {
            ForceRegistration pair = new ForceRegistration(body, fg);
            registrations.Add(pair);
        }

        /// <summary>
        /// Remove a rigid body and the associated force generator.
        /// </summary>
        /// <param name="body">The rigid body.</param>
        /// <param name="fg">The force generator.</param>
        public void Remove(RigidBody body, IForceGenerator fg)
        {
            registrations.Remove(new KeyValuePair<RigidBody, IForceGenerator>(body, fg));
        }

        /// <summary>
        /// Clear the list of registered rigid bodies.
        /// </summary>
        public void Clear()
        {
            registrations.Clear();
        }

        /// <summary>
        /// Update the rigid bodies with their respective forces.
        /// </summary>
        /// <param name="duration"></param>
        public void UpdateForces(double duration)
        {
            foreach (ForceRegistration registration in registrations)
            {
                registration.Value.UpdateForce(registration.Key, duration);
            }
        }
    }
}
