using System.Collections.Generic;
using UnityEngine;

namespace PVG
{
    public class ForceSystem
    {
        public readonly List<ForceInstance> forces = new();

        public ForceSystem(List<ForceDefinition> defs, Particle owner, WorldContext _worldCtx)
        {
            CreateForces(defs, owner, _worldCtx);
        }

        private void CreateForces(List<ForceDefinition> defs, Particle owner, WorldContext _worldCtx)
        {
            foreach (ForceDefinition def in defs)
            {
                forces.Add(def.CreateInstance(owner, _worldCtx));
            }
        }

        public void ComputeForces(ref Vector3 accumulatedForce)
        {
            for (int i = 0; i < forces.Count; i++)
            {
                ForceInstance force = forces[i];

                if (!force.CanEvaluate())
                    continue;

                accumulatedForce += force.Evaluate();
            }
        }
    }
}