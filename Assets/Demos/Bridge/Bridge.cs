using UnityEngine;
using System.Collections.Generic;

public class Bridge : MonoBehaviour
{
    private const int NumParticles = 12;
    private const int CableCount = 10;
    private const int SupportCount = 12;
    private const int RodCount = 6;

    public List<Transform> gameObjects;

    private List<Cyclone.ParticleCableAnchor> supports = new List<Cyclone.ParticleCableAnchor>();
    private List<Cyclone.ParticleRod> rods = new List<Cyclone.ParticleRod>();
    private List<Cyclone.ParticleCable> cables = new List<Cyclone.ParticleCable>();
    private List<Cyclone.Particle> particles = new List<Cyclone.Particle>();
    private Cyclone.ParticleContactResolver contactResolver = new Cyclone.ParticleContactResolver(1);


    private void Start()
    {
        // Create the particles.
        for (int i = 0; i < NumParticles; i++)
        {
            Cyclone.Particle particle = new Cyclone.Particle();
            particle.Mass = 2.0f;
            particle.Damping = 0.9f;
            particle.SetAcceleration(0.0f, -10.0f, 0.0f);
            particle.SetPosition((i / 2) * 2.0f - 5.0f, 4, (i % 2) * 2.0f - 1.0f);
            particles.Add(particle);
        }

        // Add the links
        for (int i = 0; i < CableCount; i++)
        {
            Cyclone.ParticleCable pc = new Cyclone.ParticleCable();
            pc.particle = new Cyclone.Particle[2];
            pc.particle[0] = particles[i];
            pc.particle[1] = particles[i + 2];
            pc.MaxLength = 1.9f;
            pc.Restitution = 0.3f;
            cables.Add(pc);
            //world.getContactGenerators().push_back(&cables[i]);
        }

        for (int i = 0; i < SupportCount; i++)
        {
            Cyclone.ParticleCableAnchor cableAnchor = new Cyclone.ParticleCableAnchor();
            cableAnchor.particle = new Cyclone.Particle[2];
            cableAnchor.particle[0] = particles[i];
            cableAnchor.Anchor = new Cyclone.Math.Vector3((i / 2) * 2.2f - 5.5f, 6, (i % 2) * 1.6f - 0.8f);

            if (i < 6)
            {
                cableAnchor.MaxLength = (i / 2) * 0.5f + 3.0f;
            }
            else
            {
                cableAnchor.MaxLength = 5.5f - (i / 2) * 0.5f;
            }
            cableAnchor.Restitution = 0.5f;
            supports.Add(cableAnchor);
            // world.getContactGenerators().push_back(&supports[i]);
        }

        for (int i = 0; i < RodCount; i++)
        {
            Cyclone.ParticleRod rod = new Cyclone.ParticleRod();
            rod.particle = new Cyclone.Particle[2];

            rod.particle[0] = particles[i * 2];
            rod.particle[1] = particles[i * 2 + 1];
            rod.Length = 2;
            rods.Add(rod);
            //world.getContactGenerators().push_back(&rods[i]);
        }

        for (int i = 0; i < NumParticles; i++)
        {
            Vector3 pos = new Vector3((float)particles[i].Position.x, (float)particles[i].Position.y, (float)particles[i].Position.z);
            gameObjects[i].transform.position = pos;
        }
    }

    private void Update()
    {
        double duration = Time.deltaTime;

        // Create the particle contact.
        Cyclone.ParticleContact particleContact = new Cyclone.ParticleContact();
        particleContact.particle = new Cyclone.Particle[2];
        particleContact.ParticleMovement = new Cyclone.Math.Vector3[2];

        foreach(Cyclone.ParticleRod rod in rods)
        {
            // Obtain a pair of contacting particles from the particle cable.
            if (rod.AddContact(particleContact, 1) > 0)
            {
                // Resolve the contacts.
                Cyclone.ParticleContact[] contacts = new Cyclone.ParticleContact[1];
                contacts[0] = particleContact;
                contactResolver.ResolveContacts(contacts, 1, duration);
            }
        }

        particleContact = new Cyclone.ParticleContact();
        particleContact.particle = new Cyclone.Particle[2];
        particleContact.ParticleMovement = new Cyclone.Math.Vector3[2];
        foreach (Cyclone.ParticleCableAnchor support in supports)
        {
            // Obtain a pair of contacting particles from the particle cable.
            if (support.AddContact(particleContact, 1) > 0)
            {
                // Resolve the contacts.
                Cyclone.ParticleContact[] contacts = new Cyclone.ParticleContact[1];
                contacts[0] = particleContact;
                contactResolver.ResolveContacts(contacts, 1, duration);
            }
        }

        particleContact = new Cyclone.ParticleContact();
        particleContact.particle = new Cyclone.Particle[2];
        particleContact.ParticleMovement = new Cyclone.Math.Vector3[2];
        foreach (Cyclone.ParticleCable cable in cables)
        {
            // Obtain a pair of contacting particles from the particle cable.
            if (cable.AddContact(particleContact, 1) > 0)
            {
                // Resolve the contacts.
                Cyclone.ParticleContact[] contacts = new Cyclone.ParticleContact[1];
                contacts[0] = particleContact;
                contactResolver.ResolveContacts(contacts, 1, duration);
            }
        }

        foreach(Cyclone.Particle particle in particles)
        {
            particle.Integrate(duration);
        }

        for (int i = 0; i < NumParticles; i++)
        {
            gameObjects[i].transform.position = new Vector3((float)particles[i].Position.x, (float)particles[i].Position.y, (float)particles[i].Position.z);
        }
    }
}
