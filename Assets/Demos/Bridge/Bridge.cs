using UnityEngine;
using System.Collections.Generic;

public class Bridge : MonoBehaviour
{
    private const int NumParticles = 12;

    public List<Transform> gameObjects;

    private List<Cyclone.ParticleRod> rods = new List<Cyclone.ParticleRod>();
    private List<Cyclone.ParticleCable> cables = new List<Cyclone.ParticleCable>();
    private List<Cyclone.Particle> particles = new List<Cyclone.Particle>(); 

    private void Start()
    {
        // Create the particles.
        for (int i = 0; i < NumParticles; i++)
        {
            Cyclone.Particle particle = new Cyclone.Particle();
            particle.Mass = 2.0f;
            particle.Damping = 0.9f;
            //particle.SetAcceleration(0.0f, -10.0f, 0.0f);
            particle.SetPosition((i / 2) * 2.0f - 5.0f, 4, (i % 2) * 2.0f - 1.0f);
            particles.Add(particle);
        }

        for (int i = 0; i < NumParticles; i++)
        {
            Vector3 pos = new Vector3((float)particles[i].Position.x, (float)particles[i].Position.y, (float)particles[i].Position.z);
            gameObjects[i].transform.position = pos;
        }
    }
}
