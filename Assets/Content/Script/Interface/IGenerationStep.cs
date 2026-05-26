using System;

namespace PVG
{
    public interface IGenerationStep
    {
        public Action<Particle> OnFinished { get; set; }
        public void Start();
        public bool IsFinished();
    }
}
