using System;

namespace PVG
{
    public class WorldContext
    {
        public ParticleQuerySystem particleQuerySystem;
        public WorldContext()
        {
            particleQuerySystem = new ParticleQuerySystem();
        }
    }
}