using UnityEngine;
using System.Collections.Generic;

public class Sheet : MonoBehaviour
{
    public int GridSize = 5;

    private const float BaseMass = 0.1f;

    public Transform connectionParticle;

    /// <summary>
    /// The particle game object.
    /// </summary>
    private List<Transform> gameObjects = new List<Transform>();

    /// <summary>
    /// The list of particles that represent the game objects.
    /// </summary>
    private List<Cyclone.Particle> particles = new List<Cyclone.Particle>();

    /// <summary>
    /// The contact resolver for the particles in simulation.
    /// </summary>
    private Cyclone.ParticleContactResolver contactResolver = new Cyclone.ParticleContactResolver(1);

    /// <summary>
    /// The contact generators i.e. rods, cables and supports in the scene.
    /// </summary>
    private List<Cyclone.ParticleContactGenerator> contactGenerators = new List<Cyclone.ParticleContactGenerator>();

    /// <summary>
    /// Constructs the particles, cables, supports and rods used in the bridge model.
    /// </summary>
    private void Start()
    {
        // Create the particles.
        for (int x = 0; x < GridSize; x++)
        {
            for (int z = 0; z < GridSize; z++)
            {
                Cyclone.Particle particle = new Cyclone.Particle();
                particle.Mass = BaseMass;
                particle.Damping = 0.3f;
                particle.SetAcceleration(0.0f,-11.0f, 0.0f);
                particle.SetPosition(x, 0, z);
                particles.Add(particle);

                // Instantiate the particle prefabs.
                Vector3 pos = new Vector3((float)particle.Position.x, (float)particle.Position.y, (float)particle.Position.z);
                Transform go = Instantiate(connectionParticle, pos, Quaternion.identity) as Transform;
                gameObjects.Add(go);
            }
        }

        Cyclone.ParticleCableAnchor cableAnchor1 = new Cyclone.ParticleCableAnchor();
        cableAnchor1.particle = new Cyclone.Particle[2];
        cableAnchor1.particle[0] = particles[0];
        cableAnchor1.Anchor = new Cyclone.Math.Vector3(0.0f, 0.01f, 0.0f);
        cableAnchor1.Restitution = 0.0f;
        cableAnchor1.MaxLength = 0.01f;
        contactGenerators.Add(cableAnchor1);

        Cyclone.ParticleCableAnchor cableAnchor2 = new Cyclone.ParticleCableAnchor();
        cableAnchor2.particle = new Cyclone.Particle[2];
        cableAnchor2.particle[0] = particles[particles.Count - 1];
        cableAnchor2.Anchor = new Cyclone.Math.Vector3(GridSize, 0.01f, GridSize);
        cableAnchor2.Restitution = 0.0f;
        cableAnchor2.MaxLength = 0.01f;
        contactGenerators.Add(cableAnchor2);


        Cyclone.ParticleCableAnchor cableAnchor3 = new Cyclone.ParticleCableAnchor();
        cableAnchor3.particle = new Cyclone.Particle[2];
        cableAnchor3.particle[0] = particles[GridSize - 1];
        cableAnchor3.Anchor = new Cyclone.Math.Vector3(0.0f, 0.01f, GridSize);
        cableAnchor3.Restitution = 0.0f;
        cableAnchor3.MaxLength = 0.01f;
        contactGenerators.Add(cableAnchor3);

        Cyclone.ParticleCableAnchor cableAnchor4 = new Cyclone.ParticleCableAnchor();
        cableAnchor4.particle = new Cyclone.Particle[2];
        cableAnchor4.particle[0] = particles[particles.Count - GridSize];
        cableAnchor4.Anchor = new Cyclone.Math.Vector3(GridSize, 0.01f, 0.0f);
        cableAnchor4.Restitution = 0.0f;
        cableAnchor4.MaxLength = 0.01f;
        contactGenerators.Add(cableAnchor4);

        for (int x = 0; x < GridSize; x++)
        {
            for (int z = 0; z < GridSize - 1; z++)
            {
                Cyclone.ParticleRod rod = new Cyclone.ParticleRod();
                rod.particle = new Cyclone.Particle[2];
                rod.particle[0] = particles[x * GridSize + z];
                rod.particle[1] = particles[x * GridSize + z + 1];
                rod.Length = 1.0f;
                contactGenerators.Add(rod);
            }
        }

        for (int z = 0; z < GridSize - 1; z++)
        {
            for (int x = 0; x < GridSize; x++)
            {
                Cyclone.ParticleRod rod = new Cyclone.ParticleRod();
                rod.particle = new Cyclone.Particle[2];
                rod.particle[0] = particles[z * GridSize + x];
                rod.particle[1] = particles[((z + 1) * GridSize) + x];
                rod.Length = 1.0f;
                contactGenerators.Add(rod);
            }
        }

        for (int z = 0; z < GridSize - 1; z++)
        {
            for (int x = 0; x < GridSize - 1; x++)
            {
                Cyclone.ParticleRod rodA = new Cyclone.ParticleRod();
                rodA.particle = new Cyclone.Particle[2];

                rodA.particle[0] = particles[z * GridSize + x];
                rodA.particle[1] = particles[((z + 1) * GridSize) + x + 1];

                rodA.Length = 1.4f;
                contactGenerators.Add(rodA);

                Cyclone.ParticleRod rodB = new Cyclone.ParticleRod();
                rodB.particle = new Cyclone.Particle[2];

                rodB.particle[0] = particles[z * GridSize + x + 1];
                rodB.particle[1] = particles[((z + 1) * GridSize) + x];

                rodB.Length = 1.42f;
                contactGenerators.Add(rodB);
            }
        }
    }

    /// <summary>
    /// Update the particles used in the simulation of the bridge.
    /// </summary>
    private void Update()
    {
        double duration = Time.deltaTime;

        // Create the particle contact.
        Cyclone.ParticleContact particleContact = new Cyclone.ParticleContact();

        foreach (Cyclone.ParticleContactGenerator generator in contactGenerators)
        {
            // Obtain a pair of contacting particles from the particle cable.
            if (generator.AddContact(particleContact, 1) > 0)
            {
                // Resolve the contacts.
                Cyclone.ParticleContact[] contacts = new Cyclone.ParticleContact[1];
                contacts[0] = particleContact;
                contactResolver.ResolveContacts(contacts, 1, duration);
            }
        }

        // Integrate the particles.
        foreach (Cyclone.Particle particle in particles)
        {
            particle.Integrate(duration);
        }

        // Update the game object positions.
        for (int i = 0; i < particles.Count; i++)
        {
            Vector3 pos = new Vector3((float)particles[i].Position.x, (float)particles[i].Position.y, (float)particles[i].Position.z);
            gameObjects[i].transform.position = pos;
        }
    }
}
