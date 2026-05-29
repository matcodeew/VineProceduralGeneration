using System.Collections.Generic;
using UnityEngine;

namespace PVG
{
    public class ParticleQuerySystem
    {
        public ParticleQuerySystem() { }

        public List<Particle> GetParticleByRange(Vector3 startPosition, float range)
        {
            List<IParticleQuery> nearParticles = WorldQuerySystem.GetAll<IParticleQuery>();

            List<Particle> registerParticle = new List<Particle>();

            if (nearParticles.Count <= 0)
                return new List<Particle>();

            foreach (IParticleQuery particle in nearParticles)
            {
                float distance = Vector3.Distance(startPosition, particle.GetPosition);
                if (distance > range)
                    continue;
                registerParticle.Add(particle.GetSelf);
            }
            return registerParticle;
        }
    }
}