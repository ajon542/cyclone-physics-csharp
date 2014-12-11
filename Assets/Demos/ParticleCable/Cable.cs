using UnityEngine;

public class Cable : MonoBehaviour
{
    private Cyclone.ParticleCable particleCable = new Cyclone.ParticleCable();

    private Cyclone.ParticleContactResolver contactResolver = new Cyclone.ParticleContactResolver(1);

    /// <summary>
    /// The object representing the first particle.
    /// </summary>
    public Transform object1;

    /// <summary>
    /// The object representing the second particle.
    /// </summary>
    public Transform object2;

    /// <summary>
    /// The first particle.
    /// </summary>
    private Cyclone.Particle particle1 = new Cyclone.Particle();

    /// <summary>
    /// The second particle.
    /// </summary>
    private Cyclone.Particle particle2 = new Cyclone.Particle();

    private void Start()
    {
        particle1.Mass = 2.0f;
        particle1.Damping = 0.95f;
        particle1.SetVelocity(2.0f, 0.0f, 0.0f);
        particle1.SetAcceleration(0.0f, -0.2f, 0.0f);
        particle1.SetPosition(object1.transform.position.x, object1.transform.position.y, object1.transform.position.z);

        particle2.Mass = 2.0f;
        particle2.Damping = 0.95f;
        particle2.SetAcceleration(0.0f, -0.2f, 0.0f);
        particle2.SetPosition(object2.transform.position.x, object2.transform.position.y, object2.transform.position.z);

        particleCable.MaxLength = 3.0f;
        particleCable.Restitution = 1.0f;
        particleCable.particle = new Cyclone.Particle[2];
        particleCable.particle[0] = particle1;
        particleCable.particle[1] = particle2;
    }

    private void Update()
    {
        double duration = Time.deltaTime;

        // Create the particle contact.
        Cyclone.ParticleContact particleContact = new Cyclone.ParticleContact();
        
        // Obtain a pair of contacting particles from the particle cable.
        if (particleCable.AddContact(particleContact, 1) > 0)
        {
            // Resolve the contacts.
            Cyclone.ParticleContact[] contacts = new Cyclone.ParticleContact[1];
            contacts[0] = particleContact;
            contactResolver.ResolveContacts(contacts, 1, duration);
        }

        particle1.Integrate(duration);
        particle2.Integrate(duration);

        // TODO: Work on a conversion method for Cyclone.Math and UnityEngine Vector3
        object1.transform.position = new Vector3((float)particle1.Position.x, (float)particle1.Position.y, (float)particle1.Position.z);
        object2.transform.position = new Vector3((float)particle2.Position.x, (float)particle2.Position.y, (float)particle2.Position.z);
    }
}