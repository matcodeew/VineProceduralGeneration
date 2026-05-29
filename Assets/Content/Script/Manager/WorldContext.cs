using System;

namespace PVG
{
    public class WorldContext
    {
        public ParticleQuerySystem particleQuerySystem;
        public OccupancyGridSystem occupencySystem;
        public WorldContext()
        {
            particleQuerySystem = new ParticleQuerySystem();
            occupencySystem = new OccupancyGridSystem(0.1f);
        }
    }
}