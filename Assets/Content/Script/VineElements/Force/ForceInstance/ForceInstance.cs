using UnityEngine;

namespace PVG
{
    public abstract class ForceInstance
    {
        public Particle particle;
        public WorldContext worldContext;

        public ForceDefinition def;

        public ForceInstance(Particle _particle, ForceDefinition _def, WorldContext _ctx)
        {
            particle = _particle;
            worldContext = _ctx;
            def = _def;
        }

        public abstract Vector3 Evaluate();

        public virtual bool CanEvaluate()
        {
            return particle.state.isAlive;
        }
        public abstract Vector3 GetDebugDirection(Particle p);
    }
}