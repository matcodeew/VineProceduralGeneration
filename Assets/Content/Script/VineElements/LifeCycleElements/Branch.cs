using System.Collections.Generic;

namespace PVG
{
    public class Branch
    {
        public List<Particle> particles = new();

        public Branch parent;

        public List<Branch> children = new();

        public BranchState state;

        public Branch()
        {
            state = new BranchState();
        }

        public void AddParticle(Particle particle)
        {
            particles.Add(particle);
            state.particleCount = particles.Count;
        }

        public void AddChild(Branch child)
        {
            child.parent = this;
            children.Add(child);
        }
    }
}