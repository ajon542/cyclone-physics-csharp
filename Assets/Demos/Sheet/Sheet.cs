using UnityEngine;
using System.Collections.Generic;

public class Sheet : MonoBehaviour
{
    public int GridSize = 10;

    private const float BaseMass = 2.0f;

    public Transform connectionParticle;

    /// <summary>
    /// The particle game object.
    /// </summary>
    private List<GameObject> gameObjects = new List<GameObject>();

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
                particle.Damping = 0.9f;
                particle.SetAcceleration(0.0f, -10.0f, 0.0f);
                particle.SetPosition(x, 0, z);
                particles.Add(particle);

                // Instantiate the particle prefabs.
                Vector3 pos = new Vector3((float)particle.Position.x, (float)particle.Position.y, (float)particle.Position.z);
                gameObjects.Add(Instantiate(connectionParticle, pos, Quaternion.identity) as GameObject);
            }
        }


        // Add the cables.
        /*for (int i = 0; i < CableCount; i++)
        {
            Cyclone.ParticleCable cable = new Cyclone.ParticleCable();
            cable.particle = new Cyclone.Particle[2];
            cable.particle[0] = particles[i];
            cable.particle[1] = particles[i + 2];
            cable.MaxLength = 1.9f;
            cable.Restitution = 0.3f;
            contactGenerators.Add(cable);
        }

        // Add the supports.
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
            contactGenerators.Add(cableAnchor);
        }

        // Add the rods.
        for (int i = 0; i < RodCount; i++)
        {
            Cyclone.ParticleRod rod = new Cyclone.ParticleRod();
            rod.particle = new Cyclone.Particle[2];
            rod.particle[0] = particles[i * 2];
            rod.particle[1] = particles[i * 2 + 1];
            rod.Length = 2;
            contactGenerators.Add(rod);
        }*/

        // Update the game object positions.
        //PositionGameObjects();
    }

    /// <summary>
    /// Update the particles used in the simulation of the bridge.
    /// </summary>
    private void Update()
    {
        // Move the imaginary mass on the bridge to the left.
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            if (massPos > 0)
            {
                foreach (Cyclone.Particle particle in particles)
                {
                    particle.Mass = BaseMass;
                }

                massPos--;
                particles[massPos * 2].Mass = 100.0f;
                particles[massPos * 2 + 1].Mass = 100.0f;
            }
        }

        // Move the imaginary mass on the bridge to the right.
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (massPos < 5)
            {
                foreach (Cyclone.Particle particle in particles)
                {
                    particle.Mass = BaseMass;
                }

                massPos++;
                particles[massPos * 2].Mass = 100.0f;
                particles[massPos * 2 + 1].Mass = 100.0f;
            }
        }

        double duration = Time.deltaTime;

        // Create the particle contact.
        Cyclone.ParticleContact particleContact = new Cyclone.ParticleContact();
        particleContact.particle = new Cyclone.Particle[2];
        particleContact.ParticleMovement = new Cyclone.Math.Vector3[2];

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
        PositionGameObjects();*/
    }

    /// <summary>
    /// Update the game object positions.
    /// </summary>
    /*private void PositionGameObjects()
    {
        for (int i = 0; i < GridSize; i++)
        {
            Vector3 pos = new Vector3((float)particles[i].Position.x, (float)particles[i].Position.y, (float)particles[i].Position.z);
            gameObjects[i].transform.position = pos;
        }
    }*/
}
